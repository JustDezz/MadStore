using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float secondsBetweenIncomeBonuses;
    [SerializeField] private GameObject waitTillTap;
    [SerializeField] private GameObject incomeBonus;
    private DateTime lastIncomeBonus;

    private void Start()
    {
        if (PlayerPrefs.HasKey("LastIncomeBonus"))
        {
            lastIncomeBonus = DateTime.Parse(PlayerPrefs.GetString("LastIncomeBonus"));
        }
        if ((float)DateTime.UtcNow.Subtract(lastIncomeBonus).TotalSeconds > secondsBetweenIncomeBonuses)
        {
            PlayerPrefs.SetString("LastIncomeBonus", DateTime.UtcNow.ToString());
            incomeBonus.gameObject.SetActive(true);
        }
        else
        {
            waitTillTap.gameObject.SetActive(true);
        }
    }
}
