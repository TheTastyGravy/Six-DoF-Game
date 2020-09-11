using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SaveGameScript))]
public class SceneTransitionScript : MonoBehaviour
{
    void Start()
    {
        // Add the level value to the index of level 1 (this scene is before the first level)
        int level = SceneManager.GetActiveScene().buildIndex + 1 + SaveGameScript.saveData.level;

        // If the index is invalid, reset level
        if (SceneManager.sceneCountInBuildSettings <= level)
        {
            // Revert to level 1
            level -= SaveGameScript.saveData.level;

            // Reset level settings and save to disk
            SaveGameScript.saveData.level = 0;
            SaveGameScript.saveData.savePoint = 0;
            GetComponent<SaveGameScript>().SaveDataToDisk(false);
        }

        // Load the current level, with the base scene containing the player, canvas, etc
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        SceneManager.LoadScene("BaseGameScene", LoadSceneMode.Additive);
    }
}