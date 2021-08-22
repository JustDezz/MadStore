using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IncomeLabel : MonoBehaviour
{
    private RectTransform rectTransform;
    private Text text;
    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        text = this.GetComponent<Text>();
    }
    private void OnEnable()
    {
        this.gameObject.transform.localPosition = Vector3.right * Random.Range(50, 150) + Vector3.up * 75;
        this.gameObject.transform.DOLocalMoveY(this.gameObject.transform.localPosition.y + 50, 0.95f);
        rectTransform.DOScale(Random.Range(1f, 2f), 0.5f).OnComplete(() => rectTransform.DOScale(0, 0.5f).
            OnComplete(() => IncomeLabelPool.Instance.RetunInPool(text)));
    }
}
