using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SoundToggle : MonoBehaviour
{
    [SerializeField] private string valueName;
    [SerializeField] private GameObject checkMark;
    private Toggle toggle;

    private void Awake()
    {
        toggle = this.gameObject.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { SoundManager.Instance.ToggleAudio(valueName, toggle.isOn); });
        toggle.onValueChanged.AddListener(delegate { checkMark.SetActive(!toggle.isOn); });
    }

    private void OnEnable()
    {
        toggle.isOn = SoundManager.Instance[valueName];
        checkMark.SetActive(!toggle.isOn);
    }
}
