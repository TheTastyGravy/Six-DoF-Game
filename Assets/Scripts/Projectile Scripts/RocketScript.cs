using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : ProjectileScript
{
    [SerializeField] float radius = 1f;


    protected override void HitTarget(Collider other)
    {
        // Create an explosion at the impact. (The object destroys itself)
        if (explosion != null)
            Instantiate(explosion, transform.position, transform.rotation);


        // Find all objects in 'range', and send message to them
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in colliders)
        {
            col.gameObject.SendMessage("DealDamage", damage, SendMessageOptions.DontRequireReceiver);
        }


        // Destroy this object
        Destroy(gameObject);
    }
}