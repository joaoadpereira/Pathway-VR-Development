using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    private XRGrabInteractable xRGrabInteractable;

    [SerializeField]
    private GameObject bulletGameObject;
    [SerializeField]
    private GameObject bulletPositionFire;
    private Vector3 fireGunPosition;

    private Queue<GameObject> bullets = new Queue<GameObject>();
    [SerializeField]
    private int numberOfBullets = 50;

    [SerializeField]
    private float bulletSpeed = 5.0f;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip fireGunSound;
    [SerializeField]
    private AudioClip gunEmptySound;

    private void Awake()
    {
        // set xr grab interactable 
        xRGrabInteractable = GetComponent<XRGrabInteractable>();

        if (xRGrabInteractable == null)
        {
            Debug.LogError("Missing XR Grab Interactable.");
        }

        // define fire position in gun
        if (fireGunPosition != null) {
            fireGunPosition = bulletPositionFire.transform.localPosition;
        }

        // add bullets to object pool
        for (int i = 0; i < numberOfBullets; i++) {
            GameObject bullet = Instantiate(bulletGameObject);
            bullet.transform.position = new Vector3(999, 999, 999);

            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }

        // set audio source
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.Log("Audio source missing");
        }

        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Adds the function to Activate event listener
    /// </summary>
    private void OnEnable()
    {
        // add OnOffFlash to Activated Event
        xRGrabInteractable.activated.AddListener(Shoot);
    }

    /// <summary>
    /// Handles the gun shooting
    /// </summary>
    /// <param name="args"></param>
    private void Shoot(ActivateEventArgs args)
    {
        if (bullets.Count > 0)
        {
            GameObject bullet = bullets.Dequeue();

            bullet.SetActive(true);
            bullet.transform.position = bulletPositionFire.transform.position;
            bullet.transform.rotation = Quaternion.Euler(bulletPositionFire.transform.rotation.eulerAngles);

            bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

            //play fire sound 
            audioSource.PlayOneShot(fireGunSound);
        }
        else {
            // play gun empty sound
            audioSource.PlayOneShot(gunEmptySound);
        }
    }
}
