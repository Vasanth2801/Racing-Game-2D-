using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    public float speed = 5f;            // Movement speed of the AI car
    public float turnSpeed = 5f;        // How quickly the car turns to face the next waypoint
    public List<Transform> waypoints;   // Waypoints assigned manually in the Inspector
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogError("Waypoints are not assigned to the AI car.");
            return;
        }
    }

    void Update()
    {
        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints.Count == 0) return;

        // Get the current waypoint's position
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;

        // Move the car towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Calculate direction and rotation
        Vector2 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector2.zero)
        {
            // Rotate the car to face the current waypoint
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust for 2D
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Check if we are close enough to the waypoint to move to the next one
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Only increment the index if it's not the last waypoint
            if (currentWaypointIndex < waypoints.Count - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                // Optional: Loop back to the first waypoint or stop the car.
                // Uncomment the next line if you want the car to loop back to the start.
                // currentWaypointIndex = 0;

                // If you want the car to stop, you can disable movement here:
                // speed = 0; // This stops the car after reaching the last waypoint.
            }
        }
    }
}
