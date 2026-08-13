using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameCompleteController : MonoBehaviour
{
    [Header("Scene Names — must match Build Settings exactly")]
    public string firstScene = "Lvl1";      // Scene to load on Play Again
    public string menuScene  = "Main Menu"; // Main menu scene name

    private VisualElement _root;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _root.Q<Button>("btn-restart")?.RegisterCallback<ClickEvent>(_ => PlayAgain());
        _root.Q<Button>("btn-menu")   ?.RegisterCallback<ClickEvent>(_ => GoToMenu());
    }

    void OnDisable()
    {
        if (_root == null) return;
        _root.Q<Button>("btn-restart")?.UnregisterCallback<ClickEvent>(_ => PlayAgain());
        _root.Q<Button>("btn-menu")   ?.UnregisterCallback<ClickEvent>(_ => GoToMenu());
    }

    /// <summary>
    /// Call this from your WinMenuController or nextScene.cs on the LAST level (Lvl3).
    /// e.g. FindObjectOfType&lt;GameCompleteController&gt;(true)?.ShowGameComplete();
    /// </summary>
    public void ShowGameComplete()
    {
        AudioManager.Instance?.StopAllAudio();
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    void PlayAgain()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.SwitchToGameMusic();
        SceneManager.LoadScene(firstScene);
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.SwitchToMenuMusic();
        SceneManager.LoadScene(menuScene);
    }
}
