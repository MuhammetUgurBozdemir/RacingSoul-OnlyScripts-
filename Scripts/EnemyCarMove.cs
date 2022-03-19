using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct WheelAi
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class EnemyCarMove : MonoBehaviour
{

    public float speed;
    public float maxSteer = 20f;
    public float currentSpeed;
    private carSpawner maxcars;
    public Transform centerOfMass;
    public GameObject[] Target;
    public int lane;
    public Rigidbody _rb;
    public GameObject car;

    public bool frontSensor=false, leftSensor=false, rightSensor=false, frontLeftSensor= false, frontRightSensor = false;
 
    private float sensorLength = 15f;
    public float frontSensorAngle = 30f;
   
    public float frontSideSensorPos = 0.5f;
  
   public bool projectileTriggered;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public bool rearLight;
    public GameObject[] rearLights;
    bool avoiding=false;

    [SerializeField] private List<WheelAi> wheels;
    public float hitDistance;
    private float timer = 1f;
  
    public void Start()
    {

        projectileTriggered = false;
        leftSensor = false;
        frontSensor = false;
        rightSensor = false;
        speed = UnityEngine.Random.Range(70f, 90f);
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = centerOfMass.localPosition;

        car = GameObject.Find("carSpawnPosition");
        maxcars = car.GetComponent<carSpawner>();
        rearLight = false;
        Target[0]= GameObject.FindWithTag("GameController").GetComponent<Settings>().lanes[0];
        Target[1] = GameObject.FindWithTag("GameController").GetComponent<Settings>().lanes[1];
        Target[2] = GameObject.FindWithTag("GameController").GetComponent<Settings>().lanes[2];
        Target[3] = GameObject.FindWithTag("GameController").GetComponent<Settings>().lanes[3];
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Drive();
        Wheels();
        LookForward();
       // Destroy();
       
        Brake();
        
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

    
            distance();
        

    }

    
   public void Drive()
    {
       
        
            if (timer > 0)
            {
                _rb.velocity = new Vector3(0, 0, 18);
                timer -= Time.deltaTime;
            }

            if (currentSpeed < speed)
            {
                if (avoiding) return;
                foreach (var wheel in wheels)
                {

                    wheel.collider.motorTorque = 1500 * hitDistance;
                }

            }

            else if (currentSpeed > speed)
            {
                foreach (var wheel in wheels)
                {

                    wheel.collider.motorTorque = 0;
                }

            }    
        

    }

    public void Brake()
    {
        RaycastHit brake;


        if (Physics.Raycast(transform.position, transform.forward, out brake, 10))
        {
            if (brake.collider.tag == "EnemyCar1")
            {                
                Debug.DrawLine(transform.position, brake.point);
                hitDistance = brake.distance / 20;
              
            }
            else
            {
                hitDistance = 1;
               
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        
        /*if(frontSensor == true  && leftSensor==true && rightSensor == true)
        {
            foreach (var rearLight in rearLights)
            {
                rearLight.SetActive(true);
            }
            foreach (var wheel in wheels)
            {

                wheel.collider.brakeTorque = 300;

            }
        }
        else
        {
            foreach (var rearLight in rearLights)
            {
                rearLight.SetActive(false);
            }
            foreach (var wheel in wheels)
            {

                wheel.collider.brakeTorque = 0;

            }
        }*/
        
    }

    private void Wheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;

            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;

        }
    }


    private void LookForward()
    {
        if (avoiding) return;
        
        if (centerOfMass.position.x < -3.93)
            lane = 0;
        if (centerOfMass.position.x > -3.93 && centerOfMass.position.x < 0)
            lane = 1;
        if (centerOfMass.position.x > 0 && centerOfMass.position.x < 3.96)
            lane = 2;
        if (centerOfMass.position.x > 3.96)
            lane = 3;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {

                Vector3 relativeVector = transform.InverseTransformPoint(Target[lane].transform.position);
                float newSteer = (relativeVector.x / relativeVector.magnitude) * 30;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, newSteer, maxSteer);
               
            }
        }
    }
    public void distance()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.forward * frontSensorPosition.y;
        float avoidMultipler = 0f;
        avoiding = false;

       

        // Right Sensor
        if (Physics.Raycast(sensorStartPos, transform.right, out hit, 5f))
        {
            if (hit.collider.gameObject.tag == "EnemyCar1" || hit.collider.gameObject.tag == "Barrier")
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                rightSensor = true;
            }
            
        }
        else rightSensor = false;

        // Left Sensor
        if (Physics.Raycast(sensorStartPos, -transform.right, out hit, 5f))
        {
            if (hit.collider.gameObject.tag == "EnemyCar1" || hit.collider.gameObject.tag == "Barrier")
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                leftSensor = true;
            }
            
        }
        else leftSensor = false;
        sensorStartPos += transform.right * frontSideSensorPos;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            //Front Right Sensor
            if (hit.collider.gameObject.tag == "EnemyCar1")
            {
                if (leftSensor == false)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    frontRightSensor = true;
                    frontSensor = true;
                    avoiding = true;
                    avoidMultipler -= 0.2f;
                }

            }
        }
        //Front Angle Right Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "EnemyCar1")
            {
                if (leftSensor == false)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                  
                    avoiding = true;
                    avoidMultipler -= 0.2f;
                }

            }
        }
       

        sensorStartPos -= transform.right * frontSideSensorPos * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            //Front Left Sensor
            if (hit.collider.gameObject.tag == "EnemyCar1")
            {
                frontSensor = true;
                if (rightSensor == false)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    frontLeftSensor = true;
                    avoiding = true;
                    avoidMultipler += 0.3f;
                }
            }
        }

        //Front Angle Left Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "EnemyCar1" )
            {
                if (rightSensor == false)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    
                    avoiding = true;
                    avoidMultipler += 0.2f;
                }
            }
        }
       
        if (avoidMultipler==0)
        {

            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                //Front Sensor
                if (hit.collider.gameObject.tag == "EnemyCar1")
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    
                    avoiding = true;
                    if (hit.normal.x < 0)
                    {
                        avoidMultipler = -0.3f;
                    }
                    else
                    {
                        avoidMultipler = +0.3f;
                    }
                }
            }
            
        }


        if (avoiding)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    wheel.collider.steerAngle = maxSteer * avoidMultipler;
                }
            }


        }       
       
    }   


    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.tag == "Player")
        {

            hitDistance = 0;

            foreach (var wheel in wheels)
            {

                wheel.collider.brakeTorque = 1000;

            }

        }

    }
}
