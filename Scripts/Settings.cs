using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Settings : MonoBehaviour
{
    public GameObject[] Cars;
    public GameObject SpawnPoint;
    public GameObject gameOver;
    public GameObject[] lanes;
    public GameObject Car;






    void Awake()
    {
         Car = Instantiate(Cars[PlayerPrefs.GetInt("SelectedCar")], SpawnPoint.transform.position, SpawnPoint.transform.rotation) as GameObject;
        


        GameObject.Find("carSpawnPosition").GetComponent<SpawnerController>().player = Car.transform;
       
        GameObject.Find("carSpawnPosition").GetComponent<carSpawner>().car = Car.gameObject;
        GameObject.Find("Destroyer").GetComponent<EnemyDestroyer>().player = Car.transform;
        GameObject.FindWithTag("MainCamera").GetComponent<TestCamController>().car = Car.gameObject;
        GameObject.Find("uiManager").GetComponent<uiManager>().Add = Car.gameObject;
        GameObject.FindWithTag("Player").GetComponent<Controller>().GameOver = gameOver.gameObject;        
        GameObject.Find("Targets").GetComponent<Targets>().player = Car.transform;
        GameObject.FindWithTag("Player").GetComponent<Controller>().Target[0] = lanes[0];
        GameObject.FindWithTag("Player").GetComponent<Controller>().Target[1] = lanes[1];
        GameObject.FindWithTag("Player").GetComponent<Controller>().Target[2] = lanes[2];
        GameObject.FindWithTag("Player").GetComponent<Controller>().Target[3] = lanes[3];

    }

}
