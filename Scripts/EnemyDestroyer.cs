using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    public Transform player;
    private carSpawner maxcars;
    public GameObject car;



    private float zOffset = -15f;
    void Start()
    {
      
        maxcars = car.GetComponent<carSpawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0f, player.position.z + zOffset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyCar1"  && maxcars.maxCars>0)
        {
            Destroy(collision.gameObject);
            maxcars.maxCars -= 1;          
            
        }


        if (collision.gameObject.tag =="SideContent" )
        {
            Destroy(collision.gameObject);
          

        }
    }

  
}
