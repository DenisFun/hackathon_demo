using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject wheelMesh;
    public LayerMask whatIsGround;

    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;
    [Header("Suspension")]
    public float restLegth;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;
    public float camberFront;
    public float camberRear;
    public float caster;
    public float backForceMultiplyer;
    

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;
    [Header("Steering")]
    public float steerAngle;
    public float steerTime;
    private bool isSteeringRight;
    private bool isSteeringLeft;
    private bool isNotSteering;

    private Vector3 suspensionForce;
    private Vector3 wheelVelocityLS; //Local Space
    private float Fx;
    private float Fy;
    private float nFy;

    private float wheelAngle;
    [Header("Wheel")]
    public float wheelRadius;
    public float motorTorque;
    public float accelerationMuliplyer;

    public float frontTrackForceReducer;
    public float minfrontTrackForceReducer;
    public float maxfrontTrackForceReducer;

    //drift
    private float maxAngle;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
       
        minLength = restLegth - springTravel;
        maxLength = restLegth + springTravel;
    }

    private void Update()
    {
       wheelMesh.transform.position = new Vector3(transform.position.x, transform.position.y - springLength, transform.position.z);
    
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);

      //  Debug.Log(wheelAngle);
        Debug.DrawRay(transform.position, -transform.up * (springLength), Color.green);
        Debug.DrawRay(transform.position, Fy * -transform.right, Color.red);
       
        Debug.DrawRay(transform.position,backForceMultiplyer*  Fx * transform.forward+ Fy * -transform.right, Color.yellow);

        if (wheelFrontLeft)
        {
            transform.localRotation = Quaternion.Euler(caster, wheelAngle, -camberFront);
            wheelMesh.transform.localRotation = Quaternion.Euler(caster, wheelAngle, -camberFront);
        }
        if (wheelFrontRight)
        {
            transform.localRotation = Quaternion.Euler(caster, wheelAngle, camberFront);
            wheelMesh.transform.localRotation = Quaternion.Euler(caster, 180+wheelAngle, -camberFront);
        }
        if (wheelRearLeft)
        {
            
            transform.localRotation = Quaternion.Euler(0, wheelAngle, -camberRear);
           
        }
        if (wheelRearRight)
        {
            transform.localRotation = Quaternion.Euler(0, wheelAngle, camberRear);
           
        }
        //Debug.Log(Fy);

      
      

       
       

    }
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position,-transform.up,out RaycastHit hit, maxLength + wheelRadius, whatIsGround))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLegth - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce+damperForce) * transform.up;

            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            Fx = Input.GetAxis("Vertical") * motorTorque;
            Fy = wheelVelocityLS.x*motorTorque;
            frontTrackForceReducer = 1 - Input.GetAxis("Vertical");

            // Rear drive
            if (wheelFrontLeft)
            {
                rb.AddForceAtPosition(suspensionForce + (Fy * -transform.right* frontTrackForceReducer), hit.point);
            }
            if (wheelFrontRight)
            { 
                rb.AddForceAtPosition(suspensionForce + (Fy * -transform.right* frontTrackForceReducer), hit.point);
            }
            if (wheelRearLeft)
            {
                Debug.DrawRay(transform.position, Fx * transform.forward, Color.blue);
                rb.AddForceAtPosition(suspensionForce + (Fx * transform.forward* accelerationMuliplyer) + (Fy * -transform.right ), hit.point);
            }
            if (wheelRearRight)
            {
                Debug.DrawRay(transform.position, Fx * transform.forward, Color.blue);
                rb.AddForceAtPosition(suspensionForce + (Fx * transform.forward* accelerationMuliplyer) + (Fy * -transform.right ), hit.point);
            }
            Debug.Log(Fx);
            //frontTrackForceReducer = Mathf.Clamp(frontTrackForceReducer, minfrontTrackForceReducer, maxfrontTrackForceReducer);
           
        }
    }
}
