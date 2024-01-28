using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject helpScreen;

    private bool helpVisible = false;
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Help()
    {
        if(helpVisible)
        {
            helpScreen.SetActive(false);
            helpVisible = false;
        }
        else
        {
            helpScreen.SetActive(true);
            helpVisible = true;
        }
    }
}
