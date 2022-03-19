using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRoll_Bar : MonoBehaviour
{
    string wheelLname;
    string wheelRname;
    public WheelCollider FL;
    public WheelCollider FR;
    public float AntiRoll = 5000f;

    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = FL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-FL.transform.InverseTransformPoint(hit.point).y - FL.radius) / FL.suspensionDistance;

        bool groundedR = FR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-FR.transform.InverseTransformPoint(hit.point).y - FR.radius) / FR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            _rb.AddForceAtPosition(FL.transform.up * -antiRollForce, FL.transform.position);
        if (groundedR)
            _rb.AddForceAtPosition(FR.transform.up * antiRollForce, FR.transform.position);
    }
}


