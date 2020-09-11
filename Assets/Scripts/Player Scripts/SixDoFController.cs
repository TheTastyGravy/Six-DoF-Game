using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SixDoFController : MonoBehaviour
{
    Rigidbody rb;
    MobileControllerScript mobileScript;

        [Header("Movement Defaults")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float yTurnSpeed = 1f;
    [SerializeField] float xTurnSpeed = 1f;
    [SerializeField] float rollSpeed = 0.1f;
    [SerializeField] bool invertY = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Get mobile controller script
        mobileScript = GameObject.Find("Mobile Controller").GetComponent<MobileControllerScript>();

        // Load player prefs
        yTurnSpeed = PlayerPrefs.GetFloat("YSensitivity", yTurnSpeed);
        xTurnSpeed = PlayerPrefs.GetFloat("XSensitivity", xTurnSpeed);
        rollSpeed = PlayerPrefs.GetFloat("RollSpeed", rollSpeed);
        invertY = PlayerPrefs.GetInt("InvertY", invertY ? 1 : 0) == 1;
    }
    void Start()
    {
        if (mobileScript.onMobile)
        {
            // Bottom left and bottom right
            mobileScript.CreateNewJoystick("Left Stick", new Vector2(100, 100));
            mobileScript.CreateNewJoystick("Right Stick", new Vector2(-100, 100), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0));
        }
        else
        {
            // Lock the cursor and hide it.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void FixedUpdate()
    {
        rb.AddRelativeTorque(GetRotation(), ForceMode.VelocityChange);
        rb.AddRelativeForce(GetDirection() * moveSpeed, ForceMode.VelocityChange);
    }
    void Update()
    {
        LockRightAngle();
    }


    Vector3 GetDirection()
    {
        if (mobileScript.onMobile)
        {
            return new Vector3
            {
                x = mobileScript.joystickValues["Left Stick"].x,
                y = 0f,
                z = mobileScript.joystickValues["Left Stick"].y
            };
        }
        else
        {
            return new Vector3
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Lateral"),
                z = Input.GetAxis("Vertical")
            };
        }
    }
    Vector3 GetRotation()
    {
        // Get rotation from input
        Vector3 rot;
        if (mobileScript.onMobile)
        {
            rot = new Vector3
            {
                x = mobileScript.joystickValues["Right Stick"].y * yTurnSpeed * 0.1f * (invertY ? 1 : -1),
                y = mobileScript.joystickValues["Right Stick"].x * xTurnSpeed * 0.1f,
                z = 0f
            };
        }
        else
        {
            rot = new Vector3
            {
                x = Input.GetAxis("Mouse Y") * yTurnSpeed * (invertY ? 1 : -1),
                y = Input.GetAxis("Mouse X") * xTurnSpeed,
                z = Input.GetAxis("Roll") * rollSpeed
            };
        }

        // Update flag and return rotation
        smoothRoll = (rot.magnitude <= 0.4f && rot.z == 0);
        return rot;
    }


    bool smoothRoll = true;
    float timeSinceInput = 0f;
    private void LockRightAngle()
    {
        // If the user is moving the camera, dont adjust the rotation
        if (!smoothRoll)
        {
            timeSinceInput = 0f;
            return;
        }
        // Wait 1 second after the player stops rotating to adjust
        else if (timeSinceInput < 1f)
        {
            timeSinceInput += Time.deltaTime;
            return;
        }


        // Get current rotation, rounding roll to the nearest 90 degree angle
        Vector3 roundedRot = transform.eulerAngles;
        roundedRot.z = Mathf.Round(roundedRot.z / 90) * 90;

        // Rotate the player to the new angle
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(roundedRot), Time.deltaTime);
    }
}