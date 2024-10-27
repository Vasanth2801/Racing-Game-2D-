using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSfxHandler : MonoBehaviour
{
    [Header("AudioSource")]
    public AudioSource engineAudioSource; // Assign in Inspector
    public AudioSource carHitAudioSource; // Collision sound source, assign in Inspector
    public float minPitch = 1.0f; // Minimum pitch of engine audio
    public float maxPitch = 3.0f; // Maximum pitch of engine audio
    public float speedThreshold = 10f; // Speed at which engine audio changes

    private CarController carController;

    void Awake()
    {
        carController = GetComponentInParent<CarController>(); // Get the CarController component
    }

    void Update()
    {
        UpdateEngineSfx();
    }

    void UpdateEngineSfx()
    {
        float velocityMagnitude = carController.GetVelocityMagnitude();

        if (velocityMagnitude > 0)
        {
            float normalizedSpeed = Mathf.Clamp(velocityMagnitude / speedThreshold, 0, 1);
            engineAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);

            if (!engineAudioSource.isPlaying)
            {
                engineAudioSource.Play();
            }
        }
        else
        {
            if (engineAudioSource.isPlaying)
            {
                engineAudioSource.Stop();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (carHitAudioSource != null)
        {
            // Adjust the volume based on collision force
            float relativeVelocity = collision.relativeVelocity.magnitude;
            float volume = Mathf.Clamp(relativeVelocity * 0.1f, 0.1f, 1f); // Scale as needed

            carHitAudioSource.pitch = Random.Range(0.95f, 1.05f); // Slight pitch variation
            carHitAudioSource.volume = volume;
            carHitAudioSource.Play();
        }
    }
}
