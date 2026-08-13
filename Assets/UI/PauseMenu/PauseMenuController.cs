using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PauseMenuController : MonoBehaviour
{
    [Header("Scene Names — must match Build Settings exactly")]
    public string currentScene = "Lvl1";
    public string menuScene = "Main Menu";

    bool musicOn = true;
    bool sfxOn = true;
    bool shakeOn = true;
    float masterVolume = 80f;

    VisualElement _root;
    VisualElement _overlaySettings;
    VisualElement _thumbMusic, _thumbSfx, _thumbShake;
    bool _isPaused = false;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        // ── wire buttons via RegisterCallback, not .clicked ──────
        _root.Q<Button>("btn-resume")?.RegisterCallback<ClickEvent>(_ => Resume());
        _root.Q<Button>("btn-restart")?.RegisterCallback<ClickEvent>(_ => Restart());
        _root.Q<Button>("btn-mainmenu")?.RegisterCallback<ClickEvent>(_ => GoToMenu());
        _root.Q<Button>("btn-settings")?.RegisterCallback<ClickEvent>(_ => OpenSettings());

        _overlaySettings = _root.Q<VisualElement>("overlay-settings");
        _root.Q<Button>("btn-close-settings")?.RegisterCallback<ClickEvent>(_ => CloseSettings());

        _thumbMusic = _root.Q<VisualElement>("thumb-music");
        _thumbSfx = _root.Q<VisualElement>("thumb-sfx");
        _thumbShake = _root.Q<VisualElement>("thumb-shake");

        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        shakeOn = PlayerPrefs.GetInt("ShakeOn", 1) == 1;
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 80f);

        RefreshToggle(_thumbMusic, musicOn);
        RefreshToggle(_thumbSfx, sfxOn);
        RefreshToggle(_thumbShake, shakeOn);

        _root.Q<VisualElement>("toggle-music")?.RegisterCallback<ClickEvent>(_ => {
            musicOn = !musicOn;
            RefreshToggle(_thumbMusic, musicOn);
            ApplyAudioSettings();
        });
        _root.Q<VisualElement>("toggle-sfx")?.RegisterCallback<ClickEvent>(_ => {
            sfxOn = !sfxOn;
            RefreshToggle(_thumbSfx, sfxOn);
            ApplyAudioSettings();
        });
        _root.Q<VisualElement>("toggle-shake")?.RegisterCallback<ClickEvent>(_ => {
            shakeOn = !shakeOn;
            RefreshToggle(_thumbShake, shakeOn);
        });

        var volSlider = _root.Q<Slider>("slider-volume");
        if (volSlider != null)
        {
            volSlider.value = masterVolume;
            volSlider.RegisterValueChangedCallback(evt => {
                masterVolume = evt.newValue;
                ApplyAudioSettings();
            });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        AudioManager.Instance?.StopAllAudio();
        gameObject.SetActive(true);
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        AudioManager.Instance?.ResumeAudio();
        gameObject.SetActive(false);
    }

    void Restart()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.SwitchToGameMusic();
        SceneManager.LoadScene(currentScene);
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.SwitchToMenuMusic();
        SceneManager.LoadScene(menuScene);
    }

    void OpenSettings() => _overlaySettings?.RemoveFromClassList("hidden");

    void CloseSettings()
    {
        _overlaySettings?.AddToClassList("hidden");
        SaveSettings();
    }

    void RefreshToggle(VisualElement thumb, bool isOn)
    {
        if (thumb == null) return;
        if (isOn) thumb.AddToClassList("toggle-on");
        else thumb.RemoveFromClassList("toggle-on");
    }

    void ApplyAudioSettings()
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.SetMusicEnabled(musicOn);
        AudioManager.Instance.SetSfxEnabled(sfxOn);
        AudioManager.Instance.SetMasterVolume(masterVolume / 100f);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.SetInt("SfxOn", sfxOn ? 1 : 0);
        PlayerPrefs.SetInt("ShakeOn", shakeOn ? 1 : 0);
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.Save();
    }
}