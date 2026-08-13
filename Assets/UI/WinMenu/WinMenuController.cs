using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class WinMenuController : MonoBehaviour
{
    [Header("Scene Names — must match Build Settings exactly")]
    public string nextScene = "Lvl2";       // Next level to load
    public string menuScene = "Main Menu";  // Main menu scene name

    private VisualElement _root;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        var nextBtn = _root.Q<Button>("btn-nextlvl");
        var menuBtn = _root.Q<Button>("btn-menu");

        if (nextBtn != null) nextBtn.clicked += NextLevel;
        if (menuBtn != null) menuBtn.clicked += GoToMenu;
    }

    void OnDisable()
    {
        if (_root == null) return;
        var nextBtn = _root.Q<Button>("btn-nextlvl");
        var menuBtn = _root.Q<Button>("btn-menu");
        if (nextBtn != null) nextBtn.clicked -= NextLevel;
        if (menuBtn != null) menuBtn.clicked -= GoToMenu;
    }

    /// <summary>
    /// Call this from your WinCondition / GameManager when the player wins.
    /// e.g. FindObjectOfType&lt;WinMenuController&gt;().ShowWinMenu();
    /// </summary>
    public void ShowWinMenu()
    {
        // Stop game audio
        AudioManager.Instance?.StopAllAudio();

        // Freeze the game
        Time.timeScale = 0f;

        // Show the win menu
        gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.SwitchToGameMusic();
        SceneManager.LoadScene(nextScene);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.SwitchToMenuMusic();
        SceneManager.LoadScene(menuScene);
    }
}
