using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private UnityEvent<bool> OnPauseToggled;
    [Header("Crash")]
    [SerializeField] private UnityEvent OnPlayerCrash;
    [Header("Death")]
    [SerializeField] private UnityEvent OnPlayerDeath;
    [Header("Revive")]
    [SerializeField] private UnityEvent OnPlayerRevive;

    public void TogglePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        OnPauseToggled?.Invoke(isPaused);
    }
    public void Restart()
    {
        DG.Tweening.DOTween.KillAll(true);
        SceneManager.LoadSceneAsync("Game");
    }
    public void Crash()
    {
        OnPlayerCrash?.Invoke();
        foreach (BonusLabel label in FindObjectsOfType<BonusLabel>())
        {
            label.GetComponent<TweenedEmerge>().DestroyObject();
        }
    }
    public void Die()
    {
        OnPlayerDeath?.Invoke();
        foreach (BonusLabel label in FindObjectsOfType<BonusLabel>())
        {
            label.GetComponent<TweenedEmerge>().DestroyObject();
        }
    }
    public void Revive()
    {
        OnPlayerRevive?.Invoke();
    }
}
