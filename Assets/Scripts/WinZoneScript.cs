using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WinZoneScript : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Win();
        }
    }


    public void Win()
    {
        FindObjectOfType<PauseManager>().GameOver(true);
    }
}