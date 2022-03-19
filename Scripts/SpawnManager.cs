using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    RoadSpawner roadSpawner;
  
 
    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner>();
       
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnTriggerEnter()
    {
      
        roadSpawner.MoveRoad();
        
    }
}
