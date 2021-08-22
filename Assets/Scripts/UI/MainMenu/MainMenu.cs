using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> OnShopToggled;
    [SerializeField] private UnityEvent<bool> OnSettingsToggled;
    [SerializeField] private List<GameObject> uiElements = new List<GameObject>();

    public void Play()
    {
        Time.timeScale = 1;
        DG.Tweening.DOTween.KillAll(false);
        SceneManager.LoadSceneAsync("Game");
    }
    public void Quit()
    {
        DG.Tweening.DOTween.KillAll();
        Application.Quit();
    }
    public void ToggleShop(bool isShopOpened)
    {
        ToggleUI(!isShopOpened);
        OnShopToggled?.Invoke(isShopOpened);
    }
    public void ToggleSettings(bool isSettingsOpened)
    {
        ToggleUI(!isSettingsOpened);
        OnSettingsToggled?.Invoke(isSettingsOpened);
    }
    private void ToggleUI(bool mode)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(mode);
        }
    }
}
