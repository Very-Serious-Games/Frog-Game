using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void PlayGame(string escenas)
    {
        SceneManager.LoadScene(escenas);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
