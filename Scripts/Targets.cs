using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    public Transform player;

    private float zOffset = 500f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, player.position.z + zOffset);
    }
}
