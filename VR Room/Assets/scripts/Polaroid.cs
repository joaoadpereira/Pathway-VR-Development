using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class Polaroid : MonoBehaviour
{

    private XRGrabInteractable xRGrabInteractable;
    private AudioSource audioSource;
    private Rigidbody rigidBody;

    [SerializeField]
    private GameObject photoPaper;

    [SerializeField]
    private GameObject photoinitPrint;

    [SerializeField]
    private GameObject photoFinalPrint;

    [SerializeField]
    private Camera polaroidCamera;

    [SerializeField]
    private AudioClip photoSound;

    private bool printPhoto = false;
    private GameObject photo = null;
    float elapsedTime = 0f;

    private bool canTakePhoto = false;

    /// <summary>
    /// Checks and sets if polaroid can take a photo
    /// </summary>
    public bool CanTakePhoto 
    {
        get { return canTakePhoto; }
        set { canTakePhoto = value; }
    }

    private void OnValidate()
    {
        // set xr grab interactable 
        xRGrabInteractable = GetComponent<XRGrabInteractable>();

        if (xRGrabInteractable == null)
        {
            Debug.LogError("Missing XR Grab Interactable.");
        }

        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("No rigid body found.");
        }

        audioSource = rigidBody.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Audio source not found");
        }

        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Adds the function to Activate event listener
    /// </summary>
    private void Awake()
    {
        // add OnOffFlash to Activated Event
        xRGrabInteractable.activated.AddListener(TakePhoto);

        //enable intially take photo
        canTakePhoto = true;
    }

    private void Update()
    {
        // enables animation print 
        if (printPhoto) 
        {
            PrintPhoto(photo);
        }
    }

    /// <summary>
    /// Handles the logic to take the photo
    /// </summary>
    /// <param name="args"></param>
    private void TakePhoto(ActivateEventArgs args)
    {
        // control if photo can be taken. If yes, disable take photo
        if (!canTakePhoto)
        {
            return;
        }

        canTakePhoto = false;

        photo = CreatePhoto();

        // set photo as child of polaroid
        photo.transform.parent = this.transform;

        // play sound
        audioSource.PlayOneShot(photoSound);

        // enable print photo animation
        PrintPhotoAction(true);
    }


    /// <summary>
    /// Creates the photo game object
    /// </summary>
    /// <returns></returns>
    private GameObject CreatePhoto()
    {
        GameObject photo = Instantiate(photoPaper);
        photo.SetActive(true);

        photo.transform.position = photoinitPrint.transform.position;
        photo.transform.rotation = Quaternion.Euler(photoinitPrint.transform.rotation.eulerAngles);

        // take photo and add to texture 
        Texture2D photoTexture = RTImage(polaroidCamera);

        Transform photographFilmTransform = photo.transform.Find("Photograph_Film_2");

        if (photographFilmTransform == null) 
        {
            Debug.LogError("photographFilmTransform not found!");
        }

        Material photographFilmMaterial = photographFilmTransform.GetComponent<MeshRenderer>().material;
        photographFilmMaterial.SetTexture("_BaseMap", photoTexture);

        return photo;
    }


    /// <summary>
    /// Takes the photo texture from camera
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    private Texture2D RTImage(Camera camera) 
    {
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = currentRT;
        return image;
    }

    /// <summary>
    /// Controls the photo print action
    /// </summary>
    /// <param name="print"></param>
    private void PrintPhotoAction(bool print)
    {
        if (print)
        {
            elapsedTime = 0f;
            printPhoto = true;
        }
        else {
            printPhoto = false;
        }
        
    }

    /// <summary>
    /// Makes the print animation
    /// </summary>
    /// <param name="photo"></param>
    private void PrintPhoto(GameObject photo)
    {
        Vector3 initialPosition = photoinitPrint.transform.position;
        Vector3 finalPosition = photoFinalPrint.transform.position;

        float photoSoundDuration = photoSound.length;

        if (elapsedTime < photoSoundDuration) 
        {
            elapsedTime += Time.deltaTime;
            photo.transform.position = Vector3.Lerp(initialPosition, finalPosition, elapsedTime / photoSoundDuration);
        }
        else
        {
            photo.transform.position = finalPosition;
            PrintPhotoAction(false);

            // add logic to VR interaction into phot
            photo.GetComponent<PhotoPolaroid>().AddVRInteraction(this);
        }
    }

}
