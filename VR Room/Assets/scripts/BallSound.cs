using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip impact;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null) {
            Debug.Log("No rigid body found.");
        }

        audioSource = rigidBody.GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.Log("Audio source not found");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float volumeScale = rigidBody.velocity.magnitude;
        audioSource.PlayOneShot(impact, volumeScale);
    }
}
