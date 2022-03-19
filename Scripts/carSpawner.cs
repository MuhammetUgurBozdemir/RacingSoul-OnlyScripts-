using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawner : MonoBehaviour
{

    public GameObject[] cars;
    public GameObject car;
    public float Speed;
    int carNo;
    float x; 
    float[] lines;
    public Vector3 carPos;
    public float minTimeToNextCar; // change to taste
  
    private float timeToNextCar;
    public float maxCars = 0;

    public void Start()
    {
        timeToNextCar =minTimeToNextCar;
        lines = new float[] { -6f,-1.6f,-2.5f, 1.6f ,2.5f, 6f };
       
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Speed = car.GetComponent<Controller>().currentSpeed;
        timeToNextCar -= Time.deltaTime;
        if (Speed < 100)
        {
            minTimeToNextCar = 4f;
        }
        else if (Speed < 130)
        {
            minTimeToNextCar = 1.4f;
        }
        else if (Speed < 150)
        {
            minTimeToNextCar = 0.7f;
        }
        else if (Speed < 200)
        {
            minTimeToNextCar = 0.5f;
        }
        else if (Speed < 220)
        {
            minTimeToNextCar = 0.3f;
        }


        if (timeToNextCar <= 0.0f)
        {

            {
                
                x = lines[(Random.Range(0, 6))];
                carPos = new Vector3(x, transform.position.y, transform.position.z);
                carNo = Random.Range(0, cars.Length);

                if (maxCars < 15)
                {
                    Instantiate(cars[carNo], carPos, transform.rotation);
                    maxCars = maxCars + 1f;
                }
            }
            timeToNextCar += minTimeToNextCar;
        }
    }
}

