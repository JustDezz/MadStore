using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform bonusLabelsHolder;

    [Header("Score related")]
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text bonusLabel;
    [SerializeField] private Text maxScoreLabel;
    [SerializeField] private Text incomeLabel;

    [Header("HealthBar")]
    [Space(10)] [SerializeField] private List<Image> healthBar;
    [SerializeField] private Sprite healthSprite;
    [SerializeField] private Sprite shieldSprite;

    [Header("Events")]
    [Space(20)] [SerializeField] private UnityEvent<bool> OnPauseToggled;
    [SerializeField] private UnityEvent OnPlayerDeath;
    [SerializeField] private UnityEvent OnPlayerRevive;

    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        maxScoreLabel.text = PlayerPrefs.HasKey("BestScore") ? $"Max: {PlayerPrefs.GetInt("BestScore")}" : "";
    }
    public void ScoreUpdate()
    {
        scoreLabel.text = $"Score: {Player.Instance.Score}";
    }
    public void Income(int amount)
    {
        if (amount != 0)
        {
            Text income = IncomeLabelPool.Instance.Get();
            income.gameObject.transform.SetParent(bonusLabel.transform, false);
            income.text = $"+{amount}";
            income.gameObject.transform.rotation = Quaternion.identity;
            income.color = Color.green;
            ScoreUpdate();
        }
    }
    public void BonusUI()
    {
        if (Player.Instance.IncomeBonus > 1)
        {
            bonusLabel.text = $"{Player.Instance.IncomeBonus}x";
        }
        else
        {
            bonusLabel.text = "";
        }
    }
    public void ActivateBonusLabel(BonusLabel label, float duration)
    {
        Instantiate(label, bonusLabelsHolder).ActivateLabel(duration);
    }
    public void TogglePause(bool isPaused)
    {
        OnPauseToggled?.Invoke(isPaused);
    }
    public void Die()
    {
        OnPlayerDeath?.Invoke();
    }
    public void Revive()
    {
        OnPlayerRevive?.Invoke();
    }
    public void HealthBarUpdate()
    {
        for (int i = 0; i < healthBar.Count; i++)
        {
            healthBar[i].sprite = Player.Instance.Immortal ? shieldSprite : healthSprite;
            if (i < Player.Instance.Health)
            {
                healthBar[i].gameObject.SetActive(true);
            }
            else
            {
                healthBar[i].gameObject.SetActive(false);
            }
        }
    }
    public void MainMenu()
    {
        DOTween.KillAll(true);
        SoundManager.Instance.StopSound("PlayerRun");
        SceneManager.LoadSceneAsync("MainMenu");
    }
}