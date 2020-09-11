using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class OptionsMenuScript : MonoBehaviour
{
    public Toggle invertY;


    void Awake()
    {
        // Set the toggle to the current player pref and add listener
        invertY.isOn = PlayerPrefs.GetInt("InvertY", 0) == 1;
        invertY.onValueChanged.AddListener(new UnityAction<bool>(InvertYChange));
    }


    public void ReturnButtonClick()
    {
        // Save player prefs to disc
        PlayerPrefs.Save();
        // Return to main menu
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void InvertYChange(bool value)
    {
        // Update player prefs
        PlayerPrefs.SetInt("InvertY", value ? 1 : 0);
    }


    public GameObject activePanel;
    public void ControlsButtonClick(GameObject panel)
    {
        activePanel.SetActive(false);
        activePanel = panel;
        activePanel.SetActive(true);
    }
}