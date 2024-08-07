using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed = 2f;

    private int currentWaypointIndex = 0;
    void Update()
    {
        waterAreaPatrol();
    }
    public void waterAreaPatrol()
    {
        Transform targetWaypoint = wayPoints[currentWaypointIndex];


        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);


        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
        }
    }
}
