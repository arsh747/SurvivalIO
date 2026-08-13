using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Player SFX")]
    public AudioClip playerShootSfx;
    public AudioClip playerHurtSfx;
    public AudioClip playerDeathSfx;

    [Header("Weapon SFX")]
    public AudioClip weaponPickupSfx;

    [Header("Enemy SFX")]
    public AudioClip[] zombieGroanSfx;
    public AudioClip zombieAttackSfx;

    [Header("Pickup SFX")]
    public AudioClip healthPickupSfx;

    [Header("UI SFX")]
    public AudioClip uiClickSfx;

    // ── State ──────────────────────────────────────────────────────
    bool musicEnabled = true;
    bool sfxEnabled = true;
    float masterVolume = 0.8f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
        ApplyAllSettings(); // apply saved settings immediately on awake
    }

    void Start() => PlayMusic(menuMusic);

    // ── Music ──────────────────────────────────────────────────────
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = musicEnabled ? masterVolume : 0f;
        musicSource.Play();
    }

    public void SwitchToGameMusic() => PlayMusic(gameMusic);
    public void SwitchToMenuMusic() => PlayMusic(menuMusic);

    /// <summary>
    /// Call this on death to immediately silence everything.
    /// </summary>
    public void StopAllAudio()
    {
        if (musicSource != null) musicSource.Pause();
        if (sfxSource != null) sfxSource.Stop();
    }

    /// <summary>
    /// Call this when restarting or returning to menu.
    /// </summary>
    public void ResumeAudio()
    {
        if (musicSource != null && musicEnabled) musicSource.UnPause();
    }

    // ── SFX ────────────────────────────────────────────────────────
    public void PlayPlayerShoot() => PlaySfx(playerShootSfx);
    public void PlayPlayerHurt() => PlaySfx(playerHurtSfx);
    public void PlayPlayerDeath() => PlaySfx(playerDeathSfx);
    public void PlayWeaponPickup() => PlaySfx(weaponPickupSfx);
    public void PlayHealthPickup() => PlaySfx(healthPickupSfx);
    public void PlayZombieAttack() => PlaySfx(zombieAttackSfx);
    public void PlayClickSfx() => PlaySfx(uiClickSfx);

    public void PlayZombieGroan()
    {
        if (zombieGroanSfx == null || zombieGroanSfx.Length == 0) return;
        var valid = System.Array.FindAll(zombieGroanSfx, c => c != null);
        if (valid.Length == 0) return;
        PlaySfx(valid[Random.Range(0, valid.Length)]);
    }

    // ── Settings ───────────────────────────────────────────────────
    public void SetMusicEnabled(bool isOn)
    {
        musicEnabled = isOn;
        if (musicSource != null)
            musicSource.volume = isOn ? masterVolume : 0f;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSfxEnabled(bool isOn)
    {
        sfxEnabled = isOn;
        // Stop any currently playing sfx immediately when turned off
        if (!isOn && sfxSource != null) sfxSource.Stop();
        PlayerPrefs.SetInt("SfxOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        // Apply to music immediately
        if (musicSource != null && musicEnabled)
            musicSource.volume = masterVolume;
        // sfxSource uses PlayOneShot so volume is per-call — reflected next play
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.Save();
    }

    public bool MusicEnabled => musicEnabled;
    public bool SfxEnabled => sfxEnabled;
    public float MasterVolume => masterVolume;

    // ── Private ────────────────────────────────────────────────────
    void PlaySfx(AudioClip clip)
    {
        if (!sfxEnabled || sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip, masterVolume);
    }

    void LoadSettings()
    {
        musicEnabled = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 0.8f);
    }

    // Applies loaded settings to audio sources right away
    void ApplyAllSettings()
    {
        if (musicSource != null)
            musicSource.volume = musicEnabled ? masterVolume : 0f;
    }
}