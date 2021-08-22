using UnityEngine;

public class TimeScaleToggler : MonoBehaviour
{
    private static int count = 0;
    [SerializeField] private bool startOnDisable = true;
    public static System.Action<bool> OnTimeScaled;
    private void OnEnable()
    {
        if (startOnDisable)
        {
            count++;
        }
        else
        {
            count--;
        }
        if (count == 0)
        {
            Time.timeScale = 1;
            OnTimeScaled?.Invoke(true);
        }
        else
        {
            Time.timeScale = 0;
            OnTimeScaled?.Invoke(false);
        }
    }
    private void OnDisable()
    {
        if (startOnDisable)
        {
            count--;
        }
        else
        {
            count++;
        }
        if (count == 0)
        {
            Time.timeScale = 1;
            OnTimeScaled?.Invoke(true);
        }
        else
        {
            Time.timeScale = 0;
            OnTimeScaled?.Invoke(false);
        }
    }
}
