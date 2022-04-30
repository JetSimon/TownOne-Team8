using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Timer : MonoBehaviour
{
    [SerializeField]
    private float timeInSeconds;

    float timeLeft;

    [SerializeField]
    TMP_Text mText;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeInSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            mText.text = timeLeft.ToString("0.00");
            timeLeft -= Time.deltaTime;
        }
        else
        {
            GameHandler.gameHandler.EndGame();
            Destroy(this);
        }
    }
}
