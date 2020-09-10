using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add listener for interaction
            other.gameObject.GetComponent<PlayerInteract>().interact.AddListener(Interaction);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove listener for interaction
            other.gameObject.GetComponent<PlayerInteract>().interact.RemoveListener(Interaction);
        }
    }


    /// <summary>
    /// This function is called when the player interacts with the attached object
    /// </summary>
    public abstract void Interaction();
}