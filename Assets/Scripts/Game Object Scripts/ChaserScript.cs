using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChaserScript : EnemyScript
{
    Rigidbody rb;

        [Space]
    [Tooltip("If left empty, it will be set to the player")]
    [SerializeField] protected Transform target;

    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float rebound = 7f;

    [SerializeField] protected float damage = 1f;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected void Start()
    {
        // If the target hasnt been set, use the player
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    protected void FixedUpdate()
    {
        // Get vector toward target with a magnitude of 'speed'
        Vector3 force = target.position - transform.position;
        force = force.normalized * speed;
        // Steering force
        force -= (rb.velocity);

        rb.AddForce(force, ForceMode.Force);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        // If the player has been hit, deal damge and move back
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("DealDamage", damage);


            // Get a vector away from the target
            Vector3 force = transform.position - target.position;
            force = force.normalized * rebound;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}