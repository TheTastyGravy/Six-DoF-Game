using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveScript : MonoBehaviour
{
    [SerializeField] protected float moveTime;
    [SerializeField] protected Vector3 movement;


    /// <summary>
    /// Call the 'Move' corutine
    /// </summary>
    public virtual void StartMoving()
    {
        StartCoroutine("Move");
    }


    /// <summary>
    /// Move the object by 'movement' over 'moveTime'
    /// </summary>
    public virtual IEnumerator Move()
    {
        // Get the two positions to lerp between
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + movement;

        // Loop each frame for the duration of 'moveTime'
        for (float timePassed = 0f; timePassed < moveTime; timePassed += Time.deltaTime)
        {
            // Lerp position and update the object
            Vector3 newPos = Vector3.Lerp(startPos, endPos, timePassed / moveTime);
            transform.position = newPos;

            // Continue next frame
            yield return null;
        }
    }
}