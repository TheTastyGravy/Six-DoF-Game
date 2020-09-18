using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootScript))]
public class TurretScript : EnemyScript
{
    ShootScript shootScript;
    LayerMask mask;

        [Space]
    [Tooltip("If left empty, it will be set to the player")]
    [SerializeField] Transform target;
    [SerializeField] Transform rotationPoint;
    [SerializeField] Transform firePoint;


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
    // Start is called after other scnes have loaded, so the player will exist
    void Start()
    {
        // If the target hasnt been set, use the player
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void FixedUpdate()
    {
        // Prevent the turret from being able to fire behind it
        if (Vector3.Angle(transform.forward, target.position - transform.position) >= 90)
            return;

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
}