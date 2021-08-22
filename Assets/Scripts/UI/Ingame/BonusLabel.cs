using UnityEngine;
using UnityEngine.UI;

public class BonusLabel : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image mask;
    [SerializeField] private Color color;
    private float remainedTime = 0f;
    private float duration;

    private bool isDestroying = false;

    public void ActivateLabel(float duration)
    {
        this.duration = duration;
        remainedTime = duration;
        fill.color = color;
    }
    private void Update()
    {
        remainedTime -= Time.deltaTime;
        if (remainedTime > 0)
        {
            mask.fillAmount = remainedTime / duration;
        }
        else
        {
            remainedTime = 0;
            duration = 0;
            if (!isDestroying)
            {
                this.GetComponent<TweenedEmerge>().DestroyObject();
                isDestroying = true;
            }
        }
    }
}
