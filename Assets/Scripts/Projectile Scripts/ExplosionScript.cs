using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void Start()
    {
        // Destroy the object shortly after creation
        Destroy(gameObject, 0.2f);
    }
}