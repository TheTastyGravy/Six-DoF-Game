using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillEnemiesConditionEvent : MonoBehaviour
{
    [Tooltip("Invoked when all enemies to kill have been killed")]
    public UnityEvent allEnemiesKilled;

    public List<EnemyScript> enemiesToKill;
    protected int enemyCount;


    protected void Start()
    {
        // Add listeners to enemies
        foreach (EnemyScript enemy in enemiesToKill)
        {
            // Enemies use a C# event
            enemy.onEnemyDie += OnEnemyDie;
        }

        enemyCount = enemiesToKill.Count;
    }

    protected void OnEnemyDie()
    {
        enemyCount--;

        // When all enemies are killed, invoke the event
        if (enemyCount <= 0)
        {
            allEnemiesKilled.Invoke();
        }
    }
}