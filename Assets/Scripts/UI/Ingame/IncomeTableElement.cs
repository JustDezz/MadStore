using UnityEngine;
using UnityEngine.UI;

public class IncomeTableElement : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text sourceName;
    [SerializeField] private Text income;
    public void Set(Sprite icon, string sourceName, string income)
    {
        this.icon.sprite = icon;
        this.sourceName.text = sourceName;
        this.income.text = income;
    }
    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
