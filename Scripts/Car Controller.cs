using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float accelerationFactor = 20f;
    public float maxSpeed = 10f;
    public float turnFactor = 3.5f;
    public float driftFactor = 0.95f;

    public int currentLap = 0;
    public int maxLaps = 3;
    private UIManager uiManager;
   

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;
    private Rigidbody2D myRigidbody2D;

   

    // Public property to access steering input
    public float SteeringInput
    {
        get { return steeringInput; }
    }

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
       uiManager = FindObjectOfType<UIManager>();
        UpdateLapUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FinishLine")) // Set the FinishLine trigger tag
        {
            OnFinishLineCrossed();
        }
    }

    void OnFinishLineCrossed()
    {
        currentLap++;

        if (currentLap <= maxLaps)
        {
            UpdateLapUI();
            UpdatePositionUI();
        }
        else
        {
            // Optionally handle race completion here
        }
    }

    void UpdateLapUI()
    {
        uiManager.UpdateLapCount(currentLap, maxLaps);
    }

    void UpdatePositionUI()
    {
        int position = CalculatePlayerPosition();
        uiManager.UpdatePosition(position);
    }

    int CalculatePlayerPosition()
    {
        int position = 1;

        // Assuming `aiCar` is assigned to your AI car instance in the Inspector
        GameObject aiCar = GameObject.FindWithTag("AICar");
        if (aiCar != null)
        {
            // Calculate position based on distance to finish line
            if (Vector3.Distance(transform.position, aiCar.transform.position) > 0)
            {
                position++; // AI car is closer to the finish line
            }
        }
        return position;
    }

    void Update()
    {
        accelerationInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");

      
    }


    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        ApplyDrift();
    }

    void ApplyEngineForce()
    {
        Vector2 engineForce = transform.up * accelerationInput * accelerationFactor;
        if (myRigidbody2D.velocity.magnitude < maxSpeed)
        {
            myRigidbody2D.AddForce(engineForce, ForceMode2D.Force);
        }
    }

    void ApplySteering()
    {
        rotationAngle -= steeringInput * turnFactor;
        myRigidbody2D.MoveRotation(rotationAngle);
    }

    void ApplyDrift()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(myRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(myRigidbody2D.velocity, transform.right);
        myRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, myRigidbody2D.velocity);
    }

    public bool isTireScreehing(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        float velocityVsUp = Vector2.Dot(transform.up, myRigidbody2D.velocity);

        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        return Mathf.Abs(lateralVelocity) > 2.0f; // Adjust this threshold to detect drifting
    }

    public float GetVelocityMagnitude()
    {
        return myRigidbody2D.velocity.magnitude; // Returns the speed of the car
    }

   
    
}
