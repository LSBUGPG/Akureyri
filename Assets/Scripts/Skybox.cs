using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    //the secondsPerMinute changes the length of a minute. A lower value 
    public float secondsPerMinute; 

    //starting time in hours, use decimal points for minutes
    public float startTime = 12; 

    //this variable is for the position of the area in degrees from the equator, therfore it must stay between 0 and 90.
    //It determines now high the sun rises throughout the day, but not the length of the day yet.
    public float latitudeAngle = 45;

    //The transform component of the empty that tilts the sun's roataion.(the SunTilt object, not the Sun object itself)
    public Transform sunTilt;


    private float day;
    private float min;
    private float smoothMin;

    private float texOffset;
    private Material skyMat;
    private Transform sunOrbit;

    void Start()
    {
        skyMat = GetComponent<Renderer>().sharedMaterial;
        sunOrbit = sunTilt.GetChild(0);

        sunTilt.transform.eulerAngles = new Vector3(latitudeAngle, 0, 90); //set the sun tilt

        if (secondsPerMinute == 0)
        {
            Debug.LogError("Error! Can't have a time of zero, changed to 0.01 instead.");
            secondsPerMinute = 0.01f;
        }
    }

    void UpdateSky()
    {
        smoothMin = (Time.time / secondsPerMinute) + (startTime * 60);
        day = Mathf.Floor(smoothMin / 1440) + 1;

        smoothMin = smoothMin - (Mathf.Floor(smoothMin / 1440) * 1440); //clamp smoothMin between 0-1440
        min = Mathf.Round(smoothMin);
        sunOrbit.localEulerAngles = new Vector3(0,(smoothMin / 4),0);
        
        texOffset = Mathf.Cos((((smoothMin) / 1440) * 2) * Mathf.PI) * 0.25f + 0.25f;
        skyMat.mainTextureOffset = new Vector2(Mathf.Round((texOffset - (Mathf.Floor(texOffset / 360) * 360)) * 1000) / 1000, 0);
        
    }

    void Update()
    {
        UpdateSky();
    }

}
