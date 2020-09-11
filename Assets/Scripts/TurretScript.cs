using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootScript))]
public class TurretScript : MonoBehaviour
{
    ShootScript shootScript;
    LayerMask mask;

    [Tooltip("If left empty, it will be set to the player")]
    [SerializeField] Transform target;
    [SerializeField] Transform rotationPoint;
    [SerializeField] Transform firePoint;
        [Space]
    public float health = 5;


    void Awake()
    {
        // Get transforms of this GameObject if not set
        if (rotationPoint == null)
            rotationPoint = transform.Find("RotationPoint");
        if (firePoint == null)
            firePoint = transform.Find("RotationPoint/Barrel/FirePoint");

        // Get ShootScript, which is requiered, so must exist
        shootScript = GetComponent<ShootScript>();
        // Create a mask to ignore the two layers
        mask = ~LayerMask.GetMask("TurretIgnore", "Projectiles");
    }
    // Start is called after other scnes have loaded, s the player will exist
    void Start()
    {
        // If the target hasnt been set, use the player
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
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
            rotationPoint.localRotation = Quaternion.identity;
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