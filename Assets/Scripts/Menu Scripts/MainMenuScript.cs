using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    

    public void PlayButtonClick()
    {
        SceneManager.LoadScene("LevelTransitionScene", LoadSceneMode.Single);
    }
    public void QuitButtonClick()
    {
        Application.Quit();
    }
    public void OptionButtonClick()
    {
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Single);
    }
}