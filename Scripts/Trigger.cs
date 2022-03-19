using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public SpawnManager spawnManager;

    private Transform player;
    private float zOffset = -10f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0.3f, player.position.z + zOffset);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "SpawnTrigger")
        {
            spawnManager.SpawnTriggerEnter();

        }
        
    }
}
