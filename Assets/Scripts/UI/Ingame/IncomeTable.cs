using System.Collections.Generic;
using UnityEngine;

public class IncomeTable : MonoBehaviour
{
    private Dictionary<string, Sprite> incomeSourcesIcons;
    [SerializeField] private List<string> sourcesNames;
    [SerializeField] private List<Sprite> sourcesValues;

    [SerializeField] private IncomeTableElement tableElementPrefab;
    private Vector2 baseOffsetMin;
    private RectTransform rectTransform;


    private void Awake()
    {
        if (sourcesNames.Count != sourcesValues.Count)
        {
            throw new System.Exception("Income table: sourcesNames and sourcesValues must be same size");
        }
        incomeSourcesIcons = new Dictionary<string, Sprite>();
        for (int i = 0; i < sourcesNames.Count; i++)
        {
            incomeSourcesIcons.Add(sourcesNames[i], sourcesValues[i]);
        }
        rectTransform = this.GetComponent<RectTransform>();
        baseOffsetMin = rectTransform.offsetMin;
    }

    private void OnEnable()
    {
        Dictionary<string, float> incomeSources = Player.Instance.IncomeSources;
        foreach (string source in incomeSources.Keys)
        {
            if (incomeSourcesIcons.ContainsKey(source))
            {
                var element = Instantiate(tableElementPrefab, this.transform);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y - element.GetComponent<RectTransform>().offsetMax.y * 2 - 5);
                element.Set(incomeSourcesIcons[source], source, (((int)(incomeSources[source] * 100 + 0.5f)) / 100f).ToString());

            }
        }
    }
    private void OnDisable()
    {
        rectTransform.offsetMin = baseOffsetMin;
    }
}
