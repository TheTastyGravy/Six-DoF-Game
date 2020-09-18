using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyScript))]
public class DropPickupScript : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] GameObject pickup;
#pragma warning restore 0649


    void Start()
    {
        //add listener
        GetComponent<EnemyScript>().onEnemyDie += CreatePickup;
    }

    public void CreatePickup()
    {
        Instantiate(pickup, transform.position, transform.rotation);
    }
}