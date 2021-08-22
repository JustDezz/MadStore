using DG.Tweening;
using System.Collections;
using UnityEngine;

public class WaitForTap : MonoBehaviour
{
    private IEnumerator Start()
    {
        DOTween.defaultTimeScaleIndependent = true;
        Tween waiting = this.gameObject.transform.DOScale(2, 2f).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        waiting.Kill();
        DOTween.defaultTimeScaleIndependent = false;
        Destroy(this.gameObject);
    }
}
