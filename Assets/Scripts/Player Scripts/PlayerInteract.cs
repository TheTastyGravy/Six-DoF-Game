using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    /// <summary>
    /// Invoked when the player attempts to interact with something.
    /// If something is interactable, it should add a listener to this event on trigger enter, and remove it on trigger exit
    /// </summary>
    [HideInInspector] public UnityEvent interact;

    MobileControllerScript mobileScript;


    void Awake()
    {
        // Get mobile controller script
        mobileScript = GameObject.Find("Mobile Controller").GetComponent<MobileControllerScript>();
    }
    void Start()
    {
        if (mobileScript.onMobile)
        {
            // Create new touch button at the bottom right
            mobileScript.CreateNewButton(new Vector2(-150, 250), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), 60);
        }
    }


    bool canPress = true;
    void Update()
    {
        // Get input. Either use touch button for mobile, or axis for mouse and controller
        float input;
        if (mobileScript.onMobile)
            input = (mobileScript.touchButtonDown[1]) ? 1 : 0;
        else
            input = Input.GetAxisRaw("Interact");

        // On button down, invoke the interact event
        if (input == 1f && canPress)
        {
            interact.Invoke();

            canPress = false;
        }
        if (input == 0f && !canPress)
        {
            canPress = true;
        }
    }
}