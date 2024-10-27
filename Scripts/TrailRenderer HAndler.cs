using UnityEngine;

public class DriftingTrailHandler : MonoBehaviour
{
    public CarController carController; // Reference to the CarController script
    private TrailRenderer trailRenderer; // Reference to the TrailRenderer component

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false; // Start with TrailRenderer off
        }

        if (carController == null)
        {
            Debug.LogError("CarController reference is missing in DriftingTrailHandler.");
        }
    }

    void Update()
    {
        if (carController != null && trailRenderer != null)
        {
            // Check if the car is drifting or turning
            float lateralVelocity;
            bool isBraking;
            bool isDrifting = carController.isTireScreehing(out lateralVelocity, out isBraking);
            bool isTurning = Mathf.Abs(carController.SteeringInput) > 0.1f; // Detects any turning input

            // Enable trail if car is drifting or turning
            trailRenderer.emitting = isDrifting || isTurning;
        }
    }
}
