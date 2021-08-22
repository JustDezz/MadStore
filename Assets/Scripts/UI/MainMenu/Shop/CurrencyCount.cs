using UnityEngine;
using UnityEngine.UI;

public class CurrencyCount : MonoBehaviour
{
    [SerializeField] private Text currencyCount;
    public void UpdateCount()
    {
        currencyCount.text = PlayerPrefs.GetFloat("Currency").ToString();
    }
    private void OnEnable()
    {
        UpdateCount();
    }
}