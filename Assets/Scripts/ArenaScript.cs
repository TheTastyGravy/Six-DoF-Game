using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{
    Transform player;
    float timePassed = 0f;

#pragma warning disable 0649
    [SerializeField] GameObject enemy;
#pragma warning restore 0649
    [SerializeField] Vector3 min = new Vector3(5, 5, 5);
    [SerializeField] Vector3 max = new Vector3(45, 45, 45);

    [SerializeField] float timeDelay = 5f;
    


    void Start()
    {
        // Get the player
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= timeDelay)
        {
            CreateEnemy();
            timePassed = 0f;
        }
    }

    private void CreateEnemy()
    {
        // Set to the player to enter the loop
        Vector3 randPos = player.position;
        // While the random position is too close to the player, get a new position
        while (Vector3.Distance(randPos, player.position) <= 10f)
        {
            randPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }


        // Create new enemy
        Instantiate(enemy, randPos, Quaternion.identity);
    }
}