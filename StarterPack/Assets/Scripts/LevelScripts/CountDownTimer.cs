using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public Text text;
    public float startingTime = 120f;
    float currentTime;
    int minute;
    int second;
   
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        while(currentTime > 0)
        {

        }
        minute = (int)(currentTime / 60);
        second = (int)(currentTime % 60);
        currentTime -= 1 * Time.deltaTime;
        text.text = "0" + minute + ":" + (second < 10 ? "0" : "") + second; 
        if (currentTime <= 0)
        {
            text.text = "00:00";
            print("time is up");
        }
    }
}
