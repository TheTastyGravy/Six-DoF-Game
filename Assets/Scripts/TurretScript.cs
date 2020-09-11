using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootScript))]
public class TurretScript : MonoBehaviour
{
    ShootScript shootScript;
    LayerMask mask;

    [SerializeField] Transform target;
    [SerializeField] Transform rotationPoint;
    [SerializeField] Transform firePoint;
        [Space]
    public float health = 5;

    
    void Awake()
    {
        // If transforms are not already set, try to set them
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        if (rotationPoint == null)
            rotationPoint = transform.Find("RotationPoint");
        if (firePoint == null)
            firePoint = transform.Find("RotationPoint/Barrel/FirePoint");
        
        // Get ShootScript, which is requiered, so must exist
        shootScript = GetComponent<ShootScript>();
        // Create a mask to ignore the two layers
        mask = ~LayerMask.GetMask("TurretIgnore", "Projectiles");
    }


    void FixedUpdate()
    {
        // If a raycast from the barrel hits the player
        if (Physics.Raycast(firePoint.position, target.position - firePoint.position, out RaycastHit hit, 50, mask) && hit.transform == target)
        {
            // Point the barrel at the player
            rotationPoint.LookAt(target);
            shootScript.Shoot();
        }
        else
        {
            // Face forward
            rotationPoint.rotation = Quaternion.identity;
        }
    }


    public void DealDamage(float damage)
    {
        // Ignore negitive damage
        if (damage <= 0)
            return;

        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}