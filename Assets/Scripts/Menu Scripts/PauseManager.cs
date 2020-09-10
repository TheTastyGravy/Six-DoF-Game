using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public enum PauseState
    {
        pause,
        unpause,
        toggle
    }

#pragma warning disable 0649
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button mobilePauseButton;
#pragma warning restore 0649

    /// <summary>
    /// When set to true, no actions should be made
    /// </summary>
    public static bool isPaused = false;
    private bool gameOver = false;

    MobileControllerScript mobileScript;
    SaveGameScript saveScript;


    void Awake()
    {
        // Get mobile controller and save game scripts
        mobileScript = GameObject.Find("Mobile Controller").GetComponent<MobileControllerScript>();
        saveScript = FindObjectOfType<SaveGameScript>();
    }
    void Start()
    {
        // Have the game running on start up. To do this, the flag must be true
        isPaused = true;
        ChangePauseState(PauseState.unpause);

        // Activate the mobile pause button when on mobile
        mobilePauseButton.gameObject.SetActive(mobileScript.onMobile);
    }


    bool canPause = true;
    void Update()
    {
        float pause = Input.GetAxisRaw("Pause");

        if (pause == 1f && canPause)
        {
            canPause = false;
            ChangePauseState();
        }
        if (pause == 0f && !canPause)
        {
            canPause = true;
        }
    }



    public void PauseButtonClick()
    {
        ChangePauseState(PauseState.pause);
    }
    public void ResumeButtonClick()
    {
        ChangePauseState(PauseState.unpause);
    }
    public void RespawnButtonClick()
    {
        // Unpause and reload the scene. The player will be loaded at their last save point
        ChangePauseState(PauseState.unpause);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void RestartButtonClick()
    {
        // Reset game data, and write to file without updating
        saveScript.ResetToDefault();
        saveScript.SaveDataToDisk(false);

        ChangePauseState(PauseState.unpause);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void NextLevelButtonClicK()
    {
        // Go to the next level and save data
        SaveGameScript.saveData.savePoint = 0;
        SaveGameScript.saveData.level++;
        saveScript.SaveDataToDisk(true);

        ChangePauseState(PauseState.unpause);
        // Load the transition scene
        SceneManager.LoadScene("LevelTransitionScene", LoadSceneMode.Single);
    }
    public void ExitButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    

    public void ChangePauseState(PauseState newState = PauseState.toggle)
    {
        if (newState == PauseState.toggle)
            newState = (isPaused) ? PauseState.unpause : PauseState.pause;

        // Cant unpause in game over
        if (gameOver)
            newState = PauseState.pause;
        

        if (newState == PauseState.pause && !isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseMenu.SetActive(true);

            if (mobileScript.onMobile)
            {
                mobilePauseButton.gameObject.SetActive(false);
            }
            else
            {
                // Unlock the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else if (newState == PauseState.unpause && isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);

            if (mobileScript.onMobile)
            {
                mobilePauseButton.gameObject.SetActive(true);
            }
            else
            {
                // Lock the cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }


    public void GameOver(bool hasWon = false)
    {
        gameOver = true;
        ChangePauseState(PauseState.pause);
        mobilePauseButton.gameObject.SetActive(false);

        // Disable the play button
        pauseMenu.transform.Find("PlayButton").gameObject.SetActive(false);

        // Set menu text
        pauseMenu.transform.Find("PausedText").GetComponent<Text>().text = (hasWon) ? "You won" : "Game Over";

        if (hasWon)
        {
            // Disable respawn button and enable next level button
            pauseMenu.transform.Find("RespawnButton").gameObject.SetActive(false);
            pauseMenu.transform.Find("NextLevelButton").gameObject.SetActive(true);
        }
    }
}