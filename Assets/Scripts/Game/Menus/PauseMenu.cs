using UnityEngine;

/// <summary>
/// Lightweight bridge — attach this to any GameObject that already
/// has a pause button (e.g. TouchControls / PauseButton).
/// It just calls PauseMenuController.Pause() when the button is tapped.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public void OpenPauseMenu()
    {
        FindObjectOfType<PauseMenuController>(true)?.Pause();
    }
}
