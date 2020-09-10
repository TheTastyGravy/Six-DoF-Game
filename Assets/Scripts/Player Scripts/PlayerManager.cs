using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class PlayerManager : MonoBehaviour
{
    float health;
    int ammo;
    MobileControllerScript mobileScript;

#pragma warning disable 0649
    [SerializeField] Text healthDisplay;
    [SerializeField] Text ammoDisplay;
#pragma warning restore 0649

    /// <summary>
    /// The 'weapon' being used by the player
    /// </summary>
    public ShootScript shootScript;

    [Space]
    /// <summary>
    /// Triggered when the players health reaches 0
    /// </summary>
    public UnityEvent playerDieEvent;


    void Awake()
    {
        // Get mobile controller script
        mobileScript = GameObject.Find("Mobile Controller").GetComponent<MobileControllerScript>();
    }
    void Start()
    {
        // Get data from file
        health = SaveGameScript.saveData.health;
        ammo = SaveGameScript.saveData.ammo;
        if (health <= 0f)
        {
            playerDieEvent.Invoke();
        }

        // Get all save points
        GameObject[] savePoints = GameObject.FindGameObjectsWithTag("SavePoint");
        // Find the correct save point and set the player there
        foreach (GameObject savePoint in savePoints)
        {
            if (savePoint.GetComponent<SavePointScript>().index == SaveGameScript.saveData.savePoint)
            {
                transform.position = savePoint.transform.position;
                break;
            }
        }


        if (mobileScript.onMobile)
        {
            // Create new touch button
            mobileScript.CreateNewButton(new Vector2(150, 250), 60);
        }

        // Set UI text
        healthDisplay.text = "Health: " + health;
        ammoDisplay.text = "Ammo: " + ammo;
    }


    // Used for semi auto weapons
    bool canShoot = true;
    void Update()
    {
        // Get input. Either use touch button for mobile or axis for mouse and controller
        float fire;
        if (mobileScript.onMobile)
            fire = (mobileScript.touchButtonDown[0]) ? 1 : 0;
        else
            fire = Input.GetAxisRaw("Fire1");
        

        if (fire >= 0.9f && ammo > 0 && !PauseManager.isPaused && canShoot)
        {
            shootScript.Shoot();

            if (shootScript.isSemiAuto)
                canShoot = false;
        }

        // In semi auto mode, it cant shoot again until the player releases fire
        if (shootScript.isSemiAuto && !canShoot)
            canShoot = (fire <= 0.7f);
        else
            canShoot = true;
    }

    // Collect pickups
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            // Call the pickups function, and destroy it if it returns true
            if (other.GetComponent<PickupScript>().Pickup(gameObject))
            {
                Destroy(other.gameObject);
            }
        }
    }


    public void UpdateSaveData()
    {
        SaveGameScript.saveData.health = health;
        SaveGameScript.saveData.ammo = ammo;
    }


    /// <summary>
    /// Reduce the players health, update the display, and trigger playerDieEvent if health drops below 0
    /// </summary>
    /// <param name="damage"> How much health should be removed. Negitive values are ignored </param>
    public void DealDamage(float damage)
    {
        // Cant lose negitive health
        if (damage <= 0f)
            return;

        health -= damage;

        // Update displayed health
        healthDisplay.text = "Health: " + health;


        // Trigger the death event if health reaches 0
        if (health <= 0f)
        {
            playerDieEvent.Invoke();
        }
    }
    /// <summary>
    /// Increase the players health, and update the displayd value
    /// </summary>
    /// <param name="health"> How much health should be gained? Negitive values are ignored </param>
    public void Heal(float health)
    {
        // Cant gain negitive health
        if (health <= 0f)
            return;

        this.health += health;

        // Update displayed health
        healthDisplay.text = "Health: " + this.health;
    }

    /// <summary>
    /// Increase th players ammo, and update the display. The value can be negitive to take away ammo
    /// </summary>
    /// <param name="ammo"> The ammo to be gained, or lost if negitive </param>
    public void CollectAmmo(int ammo)
    {
        this.ammo += ammo;

        // Cant have negitive ammo
        if (this.ammo < 0)
            this.ammo = 0;

        // Update displayed ammo
        ammoDisplay.text = "Ammo: " + this.ammo;
    }
}