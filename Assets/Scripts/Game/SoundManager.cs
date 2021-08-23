using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    internal class Sound
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup group;
        [Range(0, 1)] public float volume = 1;
        [Range(0, 3)] public float pitch = 1;
        public bool loop;
        public bool playOnAwake;
    }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<Sound> soundList;
    private Dictionary<string, AudioSource> sounds = new Dictionary<string, AudioSource>();
    private static SoundManager instance;
    private bool isMusicOn = true;
    private bool isEffectsOn = true;
    private bool tempEffectsMute = false;
    private PlayerRunning runner;

    public static SoundManager Instance { get { return instance; } }
    public bool IsMusicOn { get { return isMusicOn; } }
    public bool IsEffectsOn { get { return isEffectsOn; } }
    public bool this[string name]
    {
        get
        {
            if (name.Equals("MusicVolume"))
            {
                return isMusicOn;
            }
            else if (name.Equals("EffectsVolume"))
            {
                return isEffectsOn;
            }
            else
            {
                return true;
            }
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        Product.OnAnyProductCollected += delegate { PlayOneShot("ProductCollected", true); };
        TimeScaleToggler.OnTimeScaled += delegate { MainThemePitch(); };
        SceneManager.sceneLoaded += delegate
        {
            runner = FindObjectOfType<PlayerRunning>();
            if (runner == null) StopSound("PlayerRun");
        };
        foreach (Sound sound in soundList)
        {
            if (!sounds.ContainsKey(sound.name))
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = sound.clip;
                source.outputAudioMixerGroup = sound.group;
                source.volume = sound.volume;
                source.pitch = sound.pitch;
                source.loop = sound.loop;
                sounds.Add(sound.name, source);
                if (sound.playOnAwake)
                {
                    source.Play();
                }
            }
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            isMusicOn = Mathf.Approximately(PlayerPrefs.GetFloat("MusicVolume"), 0);
            audioMixer.SetFloat("MusicVolume", isMusicOn ? 0 : -80);
        }
        else
        {
            isMusicOn = true;
            audioMixer.SetFloat("MusicVolume", 0);
        }
        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            isEffectsOn = Mathf.Approximately(PlayerPrefs.GetFloat("EffectsVolume"), 0);
            audioMixer.SetFloat("EffectsVolume", isEffectsOn ? 0 : -80);
        }
        else
        {
            isEffectsOn = true;
            audioMixer.SetFloat("EffectsVolume", 0);
        }
    }
    public void ToggleAudio(string source, bool isMuted)
    {
        switch (source)
        {
            case "MusicVolume":
                ToggleMusic(isMuted);
                break;
            case "EffectsVolume":
                ToggleEffects(isMuted);
                break;
        }
    }
    private void ToggleMusic(bool isMuted)
    {
        if (audioMixer.GetFloat("MusicVolume", out float value))
        {
            isMusicOn = isMuted;
            audioMixer.SetFloat("MusicVolume", isMusicOn ? 0 : -80);
        }
    }
    private void ToggleEffects(bool isMuted)
    {
        if (audioMixer.GetFloat("EffectsVolume", out float value))
        {
            isEffectsOn = isMuted;
            if (!tempEffectsMute)
            {
                audioMixer.SetFloat("EffectsVolume", isEffectsOn ? 0 : -80);
            }
        }
    }
    public void PlaySound(string soundName)
    {
        sounds[soundName].Play();
    }
    public void PlaySound(string soundName, bool randomPitch)
    {
        if (randomPitch)
        {
            sounds[soundName].pitch = Random.Range(0.8f, 1.2f);
        }
        sounds[soundName].Play();
    }
    public void PlayOneShot(string soundName)
    {
        sounds[soundName].PlayOneShot(sounds[soundName].clip);
    }
    public void PlayOneShot(string soundName, bool randomPitch)
    {
        if (randomPitch)
        {
            sounds[soundName].pitch = Random.Range(0.8f, 1.2f);
        }
        sounds[soundName].PlayOneShot(sounds[soundName].clip);
    }
    public void StopSound(string soundName)
    {
        sounds[soundName]?.Stop();
    }
    public void MuteEffects(bool isMuted)
    {
        if (audioMixer.GetFloat("EffectsVolume", out float value))
        {
            tempEffectsMute = isMuted;
            if (isEffectsOn)
            {
                audioMixer.SetFloat("EffectsVolume", isMuted ? -80 : 0);
            }
        }
    }
    private void MainThemePitch()
    {
        if (Time.timeScale < 1)
        {
            DOTween.defaultTimeScaleIndependent = true;
            DOTween.To(x => sounds["MusicTheme"].pitch = x, sounds["MusicTheme"].pitch, 0.85f, 1f);
            DOTween.defaultTimeScaleIndependent = false;
        }
        else
        {
            DOTween.To(x => sounds["MusicTheme"].pitch = x, sounds["MusicTheme"].pitch, 1f, 1f);
        }
    }
    private void FixedUpdate()
    {
        if (runner != null)
        {
            sounds["PlayerRun"].pitch = Mathf.Lerp(0.5f, 1.25f, runner.Speed / runner.PlayerMaxSpeed / 3);
        }
    }
    private void OnDestroy()
    {
        if (audioMixer.GetFloat("MusicVolume", out float value))
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
        if (audioMixer.GetFloat("EffectsVolume", out value))
        {
            PlayerPrefs.SetFloat("EffectsVolume", value);
        }
    }
}
