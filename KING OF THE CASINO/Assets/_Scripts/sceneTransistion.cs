using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script will manage button functions on all menu screens.
//Austin, Robert, group 7
//11/2/2024

public class sceneTransistion : MonoBehaviour
{
    public void OnPlayButton()
        {
        SceneManager.LoadScene(1);
        }

    public void OnHelpButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
