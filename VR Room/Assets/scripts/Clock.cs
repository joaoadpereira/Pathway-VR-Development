using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private GameObject secondsClock;

    [SerializeField]
    private GameObject minutesClock;

    [SerializeField]
    private GameObject hoursClock;

    private int currentSeconds;
    private int currentMinutes;
    private int currentHours;

    // Start is called before the first frame update
    private void Awake()
    {
        if (secondsClock == null) {
            Debug.LogError("Seconds Clock should not be null!");
        }

        //update seconds, minutes and hour every second
        InvokeRepeating("setSeconds", 0.0f, 1.0f);
        InvokeRepeating("setMinutes", 0.0f, 1.0f);
        InvokeRepeating("setHours", 0.0f, 1.0f);
    }

    /// <summary>
    /// Sets seconds and translates the secondsClock gameObject
    /// </summary>
    private void setSeconds() {
        currentSeconds = System.DateTime.Now.Second;

        // each second increments 6º 
        secondsClock.transform.localRotation = Quaternion.Euler(currentSeconds * 6, 0, 0);
    }

    /// <summary>
    /// Sets minutes and translates to minutes gameObject
    /// </summary>
    private void setMinutes()
    {
        currentMinutes = System.DateTime.Now.Minute;
        minutesClock.transform.localRotation = Quaternion.Euler(currentMinutes * 6, 0, 0);
    }

    /// <summary>
    /// Sets hours and translates to hours gameObject
    /// </summary>
    private void setHours()
    {
        currentHours = System.DateTime.Now.Hour;
        hoursClock.transform.localRotation = Quaternion.Euler(currentHours * 6, 0, 0);
    }
}
