using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
  
    public Text scoreText;
    public Text overtakeText;
    public Text scoreTextOver;
    public Text Speed;
    private Controller scoreAdd;
    public GameObject Add;
    
    float score;
    float overtake;
    
    void Start()
    {
       
        score = 0;
        InvokeRepeating("scoreUpdate", 1f,0.2f);

       
       
         Add = GameObject.FindGameObjectWithTag("Player").gameObject;        
         scoreAdd = Add.GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        scoreTextOver.text = " " + score;
        overtakeText.text = "Overtake: " + overtake;
        Speed.text = "KM/h:" + Mathf.Round(scoreAdd.currentSpeed);



    }
    void scoreUpdate()
    {
        if (scoreAdd.currentSpeed >= 100)
        {

            score += 1;
            if (scoreAdd.rightCheck == true)
            {
                score += 100;
                overtake += 1;
                FindObjectOfType<AudioManager>().Play("overtake");

            }

            if (scoreAdd.leftCheck == true)
            {
                score += 100;
                overtake += 1;
                FindObjectOfType<AudioManager>().Play("overtake");
            }
        }
    }

   public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
