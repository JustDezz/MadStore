using DG.Tweening;
using UnityEngine;

public class TweenedEmerge : MonoBehaviour
{
    [SerializeField] private Ease ease = Ease.InOutBack;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private float duration = 0.5f;

    private void OnEnable()
    {
        this.transform.localScale = startScale;
        DOTween.defaultTimeScaleIndependent = true;
        this.transform.DOScale(endScale, duration).SetEase(ease).
            OnComplete(() => DOTween.defaultTimeScaleIndependent = false);
    }
    public void HideObject()
    {
        Disappear(delegate { this.gameObject.SetActive(false); });
    }
    public void DestroyObject()
    {
        Disappear(delegate { Destroy(this.gameObject); });
    }
    private void Disappear(System.Action onComplete)
    {
        DOTween.defaultTimeScaleIndependent = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOScale(startScale, duration).SetEase(ease).
            OnComplete(() => DOTween.defaultTimeScaleIndependent = false)).
            AppendCallback(() => onComplete?.Invoke()).Play();
    }
}
