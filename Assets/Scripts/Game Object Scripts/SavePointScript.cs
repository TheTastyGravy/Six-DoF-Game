﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointScript : Interactable
{
    SaveGameScript saveScript;

    [Tooltip("This value must be different for each save point")]
    [Min(0)] public int index;


    // Start is called after other scenes have been loaded, so the player will exist
    void Start()
    {
        // Get the save script
        saveScript = FindObjectOfType<SaveGameScript>();
    }


    public override void Interaction()
    {
        // Update save point
        SaveGameScript.saveData.savePoint = index;
        // Save data, updating the player's health and ammo
        saveScript.SaveDataToDisk(true);
    }
}