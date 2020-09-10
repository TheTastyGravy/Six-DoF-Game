using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    // The projectile prefab
    public GameObject proj;
    public float projForce;

    // Where should the bullet be created?
    [SerializeField] protected Transform firePoint;

    [Min(0f)] public float damage = 1f;
    [Min(0)] public int ammoCost = 1;

    /// <summary>
    /// Not implemented by the ShootScript, must be done by the script calling Shoot
    /// </summary>
    public bool isSemiAuto = true;
    /// <summary>
    /// How many shots can be fired in a second?
    /// </summary>
    [SerializeField] protected float fireRate = 6f;

    // Time of last shot
    protected float lastShot = 0f;



    void Awake()
    {
        // Convert from shots per second to seconds per shot
        fireRate = 1f / fireRate;
    }


    public virtual void Shoot()
    {
        if (lastShot + fireRate <= Time.time)
        {
            lastShot = Time.time;

            // Create new projectile
            GameObject newProj = Instantiate(proj, firePoint.position, firePoint.rotation);
            newProj.GetComponent<ProjectileScript>().Setup(1f, projForce);
            // The projectile cant collide with the object that fired it
            Physics.IgnoreCollision(newProj.GetComponent<Collider>(), GetComponent<Collider>());

            // Reduce the players ammo
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().CollectAmmo(-ammoCost);
        }
    }
}