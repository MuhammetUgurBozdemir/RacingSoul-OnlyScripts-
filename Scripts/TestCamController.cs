using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamController : MonoBehaviour
{
    private Transform player;
    public GameObject car;
    public float Speed;
    private float zOffset = -5.0f;
    private float yOffset = 3.5f;
  
    private float yyOffset;
    private float rotation;
    private float rotationOffset;
    private float shake;
    private float Max;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
       
    }

    // Update is called once per frame
    void Update()
    {
        Speed = car.GetComponent<Controller>().currentSpeed;
        Max = car.GetComponent<Controller>().maxSpeed;

        yyOffset = Speed / 100;
        yyOffset = Mathf.Clamp(yyOffset, 0, 1.2f);
        rotationOffset = Speed / 36;

        rotation = Mathf.Lerp(30,30-rotationOffset, 1f);

        transform.position = new Vector3(player.position.x + shake, player.position.y + yOffset - yyOffset, player.position.z + zOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation, 0, 0), 2f * Time.deltaTime);
        if (Speed > 100)
        {
            shake = Random.Range(0f, Speed / Max / 100);
        }
        else
            shake = 0f;
        
           
       }
  
     
        
          
         
        


    
}
