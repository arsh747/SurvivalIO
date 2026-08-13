using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour
{
    public UnityEvent OnEnter;
    [SerializeField] private AudioSource jse;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jse.Play();
            OnEnter.Invoke();

            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Lvl3")
            {
                // Last level — show game complete screen
                FindObjectOfType<GameCompleteController>(true)?.ShowGameComplete();
            }
            else
            {
                // Lvl1 or Lvl2 — show normal win menu
                FindObjectOfType<WinMenuController>(true)?.ShowWinMenu();
            }
        }
    }
}