using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public VisualTreeAsset mainMenuUXML;
    public StyleSheet mainMenuUSS;

    [Header("Scene to Load")]
    public string gameSceneName = "Lvl1";

    // ── Audio state ──────────────────────────────────────────────
    bool musicOn = true;
    bool sfxOn = true;
    bool shakeOn = true;
    float masterVolume = 80f;

    UIDocument doc;
    VisualElement root;

    // ── Overlay references ───────────────────────────────────────
    VisualElement overlayHTP;
    VisualElement overlaySettings;

    // ── Toggle references ────────────────────────────────────────
    VisualElement toggleMusic, thumbMusic;
    VisualElement toggleSfx, thumbSfx;
    VisualElement toggleShake, thumbShake;

    void OnEnable()
    {
        doc = GetComponent<UIDocument>();
        if (doc == null) { Debug.LogError("UIDocument missing!"); return; }

        if (mainMenuUXML != null) doc.visualTreeAsset = mainMenuUXML;
        if (mainMenuUSS != null) doc.rootVisualElement.styleSheets.Add(mainMenuUSS);

        root = doc.rootVisualElement;

        // ── Main menu buttons ────────────────────────────────────
        root.Q<Button>("btn-play")?.RegisterCallback<ClickEvent>(_ => PlayGame());
        root.Q<Button>("btn-howtoplay")?.RegisterCallback<ClickEvent>(_ => ShowOverlay(overlayHTP));
        root.Q<Button>("btn-settings")?.RegisterCallback<ClickEvent>(_ => ShowOverlay(overlaySettings));
        root.Q<Button>("btn-quit")?.RegisterCallback<ClickEvent>(_ => QuitGame());

        // ── Overlays ─────────────────────────────────────────────
        overlayHTP = root.Q<VisualElement>("overlay-howtoplay");
        overlaySettings = root.Q<VisualElement>("overlay-settings");

        root.Q<Button>("btn-close-htp")?.RegisterCallback<ClickEvent>(_ => HideOverlay(overlayHTP));
        root.Q<Button>("btn-close-settings")?.RegisterCallback<ClickEvent>(_ => HideOverlay(overlaySettings));

        // ── Load saved prefs ─────────────────────────────────────
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        shakeOn = PlayerPrefs.GetInt("ShakeOn", 1) == 1;
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 80f);

        // ── Toggle references ────────────────────────────────────
        toggleMusic = root.Q<VisualElement>("toggle-music");
        thumbMusic = root.Q<VisualElement>("thumb-music");
        toggleSfx = root.Q<VisualElement>("toggle-sfx");
        thumbSfx = root.Q<VisualElement>("thumb-sfx");
        toggleShake = root.Q<VisualElement>("toggle-shake");
        thumbShake = root.Q<VisualElement>("thumb-shake");

        // Apply saved state to visuals
        RefreshToggle(thumbMusic, musicOn);
        RefreshToggle(thumbSfx, sfxOn);
        RefreshToggle(thumbShake, shakeOn);

        // Apply saved state to AudioManager immediately on menu load
        ApplyAudioSettings();

        // ── Toggle click events ──────────────────────────────────
        toggleMusic?.RegisterCallback<ClickEvent>(_ =>
        {
            musicOn = !musicOn;
            RefreshToggle(thumbMusic, musicOn);
            ApplyAudioSettings();
        });

        toggleSfx?.RegisterCallback<ClickEvent>(_ =>
        {
            sfxOn = !sfxOn;
            RefreshToggle(thumbSfx, sfxOn);
            ApplyAudioSettings();
        });

        toggleShake?.RegisterCallback<ClickEvent>(_ =>
        {
            shakeOn = !shakeOn;
            RefreshToggle(thumbShake, shakeOn);
            // shakeOn is game-feel only, no AudioManager call needed
            PlayerPrefs.SetInt("ShakeOn", shakeOn ? 1 : 0);
            PlayerPrefs.Save();
        });

        // ── Volume slider ─────────────────────────────────────────
        var volSlider = root.Q<Slider>("slider-volume");
        if (volSlider != null)
        {
            volSlider.value = masterVolume;
            volSlider.RegisterValueChangedCallback(evt =>
            {
                masterVolume = evt.newValue;
                ApplyAudioSettings();
            });
        }

        // ── Pulsing dot ──────────────────────────────────────────
        var dot = root.Q<VisualElement>("pulse-dot");
        if (dot != null) dot.schedule.Execute(() => PulseTick(dot)).Every(50);
    }

    // ── Toggle visual helper ─────────────────────────────────────
    void RefreshToggle(VisualElement thumb, bool isOn)
    {
        if (thumb == null) return;
        if (isOn) thumb.AddToClassList("toggle-on");
        else thumb.RemoveFromClassList("toggle-on");
    }

    // ── Overlay helpers ──────────────────────────────────────────
    void ShowOverlay(VisualElement overlay)
    {
        overlay?.RemoveFromClassList("hidden");
    }

    void HideOverlay(VisualElement overlay)
    {
        overlay?.AddToClassList("hidden");
        SaveSettings();
    }

    // ── Audio ─────────────────────────────────────────────────────
    void ApplyAudioSettings()
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.SetMusicEnabled(musicOn);
        AudioManager.Instance.SetSfxEnabled(sfxOn);
        AudioManager.Instance.SetMasterVolume(masterVolume / 100f); // slider is 0-100, AudioManager expects 0-1
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.SetInt("SfxOn", sfxOn ? 1 : 0);
        PlayerPrefs.SetInt("ShakeOn", shakeOn ? 1 : 0);
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.Save();
    }

    // ── Pulse animation ──────────────────────────────────────────
    float pulseT = 0f;
    void PulseTick(VisualElement dot)
    {
        pulseT += 0.05f * 1.8f;
        float a = Mathf.PingPong(pulseT, 1f);
        dot.style.backgroundColor = new Color(0.29f, 0.67f, 0.29f, 0.4f + a * 0.6f);
    }

    // ── Scene management ─────────────────────────────────────────
    void PlayGame()
    {
        SaveSettings();
        AudioManager.Instance?.SwitchToGameMusic();
        SceneManager.LoadScene(gameSceneName);
    }

    void QuitGame()
    {
        SaveSettings();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}