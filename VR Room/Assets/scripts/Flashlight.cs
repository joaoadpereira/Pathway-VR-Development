using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flashlight : MonoBehaviour
{
    private XRGrabInteractable xRGrabInteractable;
    [SerializeField]
    private GameObject flashlight;
    private bool flashlightState;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioOn;
    [SerializeField]
    private AudioClip audioOff;

    /// <summary>
    /// Sets components references and flushlight initial state
    /// </summary>
    void Awake()
    {
        //set xr grab interactable 
        xRGrabInteractable = GetComponent<XRGrabInteractable>();

        if (xRGrabInteractable == null)
        {
            Debug.LogError("Missing XR Grab Interactable.");
        }

        //set flashLight
        if (flashlight == null)
        {
            Debug.LogError("Missing Flushlight component.");
        }

        // set audio source
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null) {
            Debug.Log("Audio source missing");
        }

        audioSource.playOnAwake = false;

        //flushLightState off initially
        flashlightState = false;

    }

    /// <summary>
    /// Adds the function to Activate event listener
    /// </summary>
    private void OnEnable()
    {
        //add OnOffFlash to Activated Event
        xRGrabInteractable.activated.AddListener(OnOffFlash);
    }

    /// <summary>
    /// Activates or deactivates the flushlight
    /// </summary>
    private void OnOffFlash(ActivateEventArgs args)
    {
        flashlightState = !flashlightState;

        flashlight.SetActive(flashlightState);

        if (flashlightState) {
            audioSource.PlayOneShot(audioOff);
        } else
        {
            audioSource.PlayOneShot(audioOn);
        }

    }
}
