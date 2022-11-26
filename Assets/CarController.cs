using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Wheel[] wheels;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;

    [Header("Inputs")]
    public float steerInput;

    public float ackermannAngleLeft;
    public float ackermannAngleRight;
    public float rearTrackSlideAngleMultiplyer;

    private void Update()
    {
        steerInput = Input.GetAxis("Horizontal");


        if (steerInput > 0)
        {
            // is turning right
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        }
        else if (steerInput < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach(Wheel w in wheels)
        {
            if (w.wheelFrontLeft)
                w.steerAngle = ackermannAngleLeft;
            if (w.wheelFrontRight)
                w.steerAngle = ackermannAngleRight;
            if (w.wheelRearLeft && Input.GetAxis("Vertical")>0)
                w.steerAngle = ackermannAngleLeft / rearTrackSlideAngleMultiplyer;
            if (w.wheelRearRight && Input.GetAxis("Vertical") > 0)
                w.steerAngle = ackermannAngleRight / rearTrackSlideAngleMultiplyer;
        }
    }
}
    