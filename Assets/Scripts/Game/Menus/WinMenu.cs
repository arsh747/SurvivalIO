using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public string sceneName;
    public void NextLvl()
    {
        SceneManager.LoadScene(sceneName);
    }
}
