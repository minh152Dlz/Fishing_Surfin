using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed;

    private int currentWaypointIndex = 0;
    private void Start()
    {
        StartCoroutine(IncreaseSpeedAfterDelay());
    }
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
    private IEnumerator IncreaseSpeedAfterDelay()
    {
        yield return new WaitForSeconds(60);
        speed *= 2.5f;
    }
}
