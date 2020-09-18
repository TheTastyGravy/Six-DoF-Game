using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    [SerializeField] protected float health = 5.0f;

    // Use a C# event
    public delegate void OnEnemyDie();
    public event OnEnemyDie onEnemyDie;

    

    /// <summary>
    /// Deal damge to the objects health, and destroy it if its health reaches 0
    /// </summary>
    /// <param name="damage"> Health to be removed </param>
    public virtual void DealDamage(float damage)
    {
        // Ignore negitive damage
        if (damage <= 0)
            return;

        health -= damage;

        if (health <= 0)
        {
            // Invoke C# event and destroy object
            onEnemyDie?.Invoke();
            Destroy(gameObject);
        }
    }
}