using DG.Tweening;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    [SerializeField] protected float duration;
    [SerializeField] protected GameObject bonusLabel;
    public abstract void Activate();
    protected virtual void Start()
    {
        this.transform.DOLocalMoveY(2, 0.5f).SetLoops(-1, LoopType.Yoyo);
        this.transform.DORotate(Vector3.up * 180, 1f).SetLoops(-1, LoopType.Incremental);
    }
    protected void OnDestroy()
    {
        DOTween.Kill(this.transform, true);
    }
}
