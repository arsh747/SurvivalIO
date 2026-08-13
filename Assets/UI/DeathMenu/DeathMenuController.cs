using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DeathMenuController : MonoBehaviour
{
    [Header("Scene Names — must match Build Settings exactly")]
    public string retscene = "Lvl1";
    public string menuScene = "Main Menu";

    private VisualElement _root;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        var retryBtn = _root.Q<Button>("btn-retry");
        var menuBtn = _root.Q<Button>("btn-menu");
        if (retryBtn != null) retryBtn.clicked += Retry;
        if (menuBtn != null) menuBtn.clicked += GoToMenu;
    }

    void OnDisable()
    {
        if (_root == null) return;
        var retryBtn = _root.Q<Button>("btn-retry");
        var menuBtn = _root.Q<Button>("btn-menu");
        if (retryBtn != null) retryBtn.clicked -= Retry;
        if (menuBtn != null) menuBtn.clicked -= GoToMenu;
    }

    public void ShowDeathMenu()
    {
        // Stop audio via AudioManager (bypasses AudioListener limitation)
        AudioManager.Instance?.StopAllAudio();

        // Freeze the game completely
        Time.timeScale = 0f;

        // Show the menu
        gameObject.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.ResumeAudio();
        SceneManager.LoadScene(retscene);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        // AudioManager is DontDestroyOnLoad so it carries over — switch to menu music
        AudioManager.Instance?.SwitchToMenuMusic();
        SceneManager.LoadScene(menuScene);
    }
}