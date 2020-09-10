using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SaveGameScript : MonoBehaviour
{
    /// <summary>
    /// Data structure holds all data that needs to be saved
    /// </summary>
    [System.Serializable]
    public struct SaveData
    {
        public float health;
        public int ammo;
        [HideInInspector] public int savePoint;
        [HideInInspector] public int level;
    }

    /// <summary>
    /// Data loaded from and saved to the save file
    /// </summary>
    [HideInInspector] public static SaveData saveData;
    private string savePath;

#pragma warning disable 0649
    [SerializeField] Text notificationText;

    [Tooltip("The default values used for new games")]
    [SerializeField] private SaveData defaultValues;
#pragma warning restore 0649

    [Space]
    /// <summary>
    /// Invoked when the save data needs to be updated
    /// </summary>
    public UnityEvent gameSaveEvent;



    void Awake()
    {
        savePath = Application.persistentDataPath + "/save.dat";

        // Try to load data file
        if (!LoadDataFromDisk())
        {
            Debug.Log("No save file exists; Using default values");
            ResetToDefault();
        }


        // Update default values
        defaultValues.savePoint = 0;
        defaultValues.level = saveData.level;
    }


    /// <summary>
    /// Reset the save data to the default values
    /// </summary>
    public void ResetToDefault()
    {
        saveData = defaultValues;
    }

    /// <summary>
    /// Saves the current save data to the disk
    /// </summary>
    /// <param name="updateData"> Should the data be updated before saving? </param>
    public void SaveDataToDisk(bool updateData = true)
    {
        if (updateData)
        {
            gameSaveEvent.Invoke();
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, saveData);
        file.Close();

        if (notificationText != null)
        {
            // Show notification for 1 second
            notificationText.enabled = true;
            notificationText.text = "Game Saved";
            Invoke("HideNotification", 1f);
        }
    }
    /// <summary>
    /// Loads the save data from the disk
    /// </summary>
    /// <returns> Was the data loaded sucessfuly? </returns>
    public bool LoadDataFromDisk()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            return true;
        }

        return false;
    }


    void HideNotification()
    {
        notificationText.enabled = false;
    }
}