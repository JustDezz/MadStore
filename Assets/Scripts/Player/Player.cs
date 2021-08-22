using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerMaxSpeed = 2f;
    [SerializeField] private float playerMinSpeed = 1f;
    [Min(0)] [SerializeField] private float timeToMaxSpeed = 60;
    [Min(0)] [SerializeField] private float incomeTimeRate = 0.1f;
    [Min(0)] [SerializeField] private float crashImmortalityTime = 3f;
    private int health = 3;
    private int score = 0;
    private Dictionary<string, float> incomeSources;

    private float speedBonus = 1;
    private int incomeBonus = 1;
    private int minIncomeBonus = 1;
    private bool immortal = false;

    [SerializeField] private EventManager eventManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AdManager adManager;

    public float PlayerMaxSpeed { get { return Instance.playerMaxSpeed; } }
    public float PlayerMinSpeed { get { return Instance.playerMinSpeed; } }
    public float TimeToMaxSpeed { get { return Instance.timeToMaxSpeed; } }
    public float SpeedBonus { get { return Instance.speedBonus; } }
    public int IncomeBonus { get { return Instance.incomeBonus; } }
    public float IncomeTimeRate { get { return Instance.incomeTimeRate; } }
    public int Score { get { return Instance.score; } }
    public int Health { get { return Instance.health; } }
    public bool Immortal { get { return Instance.immortal; } }
    public Dictionary<string, float> IncomeSources { get { return Instance.incomeSources; } }

    private static Player instance;
    public static Player Instance { get { return instance; } }

    private List<Coroutine> bonusCoroutines;
    private Coroutine immortalityCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        bonusCoroutines = new List<Coroutine>();
        incomeSources = new Dictionary<string, float>();
    }
    public void Crash()
    {
        if (--health == 0)
        {
            eventManager.Die();
        }
        else
        {
            eventManager.Crash();
            Immortality(crashImmortalityTime);
        }
    }
    public void Income(string source, int amount)
    {
        int income = amount * incomeBonus;
        score += income;
        if (!incomeSources.ContainsKey(source))
        {
            incomeSources.Add(source, 0);
        }
        incomeSources[source] += income / 10;
        uiManager.Income(income);
    }
    public void Income(Product product)
    {
        if (product != null)
        {
            score += incomeBonus;
            if (!incomeSources.ContainsKey(product.ProductName))
            {
                incomeSources.Add(product.ProductName, 0);
            }
            incomeSources[product.ProductName] += product.ProductPrice * incomeBonus;
            uiManager.Income(incomeBonus);
        }
    }
    public void Immortality(float duration)
    {
        if (immortalityCoroutine != null)
        {
            StopCoroutine(immortalityCoroutine);
        }
        immortalityCoroutine = (StartCoroutine(ActivateImmortality(duration)));
    }
    public void AddBonus(float duration, int incomeBonus = 0, float speedBonus = 0)
    {
        uiManager.BonusUI();
        bonusCoroutines.Add(StartCoroutine(ActivateBonus(duration, incomeBonus, speedBonus)));
    }
    public void ClearBonuses()
    {
        foreach (Coroutine bonus in bonusCoroutines)
        {
            StopCoroutine(bonus);
        }
        speedBonus = 1;
        incomeBonus = minIncomeBonus;
        uiManager.BonusUI();
    }
    private IEnumerator ActivateBonus(float duration, int incomeBonus = 0, float speedBonus = 0)
    {
        this.speedBonus += speedBonus;
        this.incomeBonus += incomeBonus;
        uiManager.BonusUI();
        yield return new WaitForSeconds(duration);
        this.speedBonus -= speedBonus;
        this.incomeBonus -= incomeBonus;
        if (this.speedBonus < 1)
        {
            this.speedBonus = 1;
        }
        if (this.incomeBonus < minIncomeBonus)
        {
            this.incomeBonus = minIncomeBonus;
        }
        uiManager.BonusUI();
    }
    private IEnumerator ActivateImmortality(float duration)
    {
        immortal = true;
        uiManager.HealthBarUpdate();
        yield return new WaitForSeconds(duration);
        immortal = false;
        uiManager.HealthBarUpdate();
    }
    public void WatchAdForIncomeBonus()
    {
        adManager.ShowRewardedAd(AdIncomeBonus);
    }
    private void AdIncomeBonus()
    {
        minIncomeBonus = 2;
        incomeBonus = minIncomeBonus;
        uiManager.BonusUI();
    }
    public void WatchAdToRevive()
    {
        adManager.ShowRewardedAd(Revive);
    }
    private void Revive()
    {
        health = 3;
        uiManager.HealthBarUpdate();
        Immortality(crashImmortalityTime);
        eventManager.Revive();
    }
    private void OnDestroy()
    {
        if (!PlayerPrefs.HasKey("BestScore") || PlayerPrefs.GetInt("BestScore") < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
        float currencyIncome = 0;
        foreach (float income in incomeSources.Values)
        {
            currencyIncome += income;
        }
        currencyIncome = ((int)(currencyIncome * 100 + 0.5f)) / 100f;
        if (PlayerPrefs.HasKey("Currency"))
        {
            PlayerPrefs.SetFloat("Currency", PlayerPrefs.GetFloat("Currency") + currencyIncome);
        }
        else
        {
            PlayerPrefs.SetFloat("Currency", currencyIncome);
        }
    }
}
