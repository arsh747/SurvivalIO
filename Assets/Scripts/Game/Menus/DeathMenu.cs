using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public string retscene;
    public void Retry()
    {
        SceneManager.LoadScene(retscene);
    }
}
