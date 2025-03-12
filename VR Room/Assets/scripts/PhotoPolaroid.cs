using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoPolaroid : MonoBehaviour
{
    private XRGrabInteractable xRGrabInteractable;
    private Rigidbody rigidbodyPhoto;

    private bool changedInitialSettings = false;

    private Polaroid polaroid;

    [SerializeField]
    private GameObject attach;

    #region private Methods
    /// <summary>
    /// It will check if inital setting changes needs to be done
    /// </summary>
    /// <param name="args"></param>
    private void CheckIfNeedsToChangeSettings(SelectEnterEventArgs args) 
    {
        if (!changedInitialSettings) 
        {
            xRGrabInteractable.selectExited.AddListener(GrabPhoto);            
        }
    }

    /// <summary>
    /// Removes inital settings after photo is printed
    /// </summary>
    /// <param name="args"></param>
    private void GrabPhoto(SelectExitEventArgs args)
    {
        // change rigid bodies settings
        rigidbodyPhoto.useGravity = true;
        rigidbodyPhoto.isKinematic = false;

        // disable this function
        changedInitialSettings = true;
        xRGrabInteractable.selectExited.RemoveListener(GrabPhoto);

        // remove polaroid as parent
        transform.parent = null;
        polaroid.CanTakePhoto = true;
    }


    #endregion

    #region Public Methods
    /// <summary>
    /// It adds the VR Grab logic and settings until photo is grabbed
    /// </summary>
    /// <param name="photo"></param>
    public void AddVRInteraction(Polaroid polaroidCamera)
    {
        polaroid = polaroidCamera;

        // add XR grab script
        this.gameObject.AddComponent<XRGrabInteractable>();
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
        xRGrabInteractable.smoothPosition = true;
        xRGrabInteractable.smoothRotation = true;
        xRGrabInteractable.attachTransform = attach.transform;

        // disable RB settings 
        rigidbodyPhoto = GetComponent<Rigidbody>();
        if (rigidbodyPhoto == null)
        {
            Debug.LogError("rigid body was not added into photo");
        }

        rigidbodyPhoto.useGravity = false;
        rigidbodyPhoto.isKinematic = true;
        rigidbodyPhoto.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // add listner funciton
        xRGrabInteractable.selectEntered.AddListener(CheckIfNeedsToChangeSettings);
    }

    #endregion
}
