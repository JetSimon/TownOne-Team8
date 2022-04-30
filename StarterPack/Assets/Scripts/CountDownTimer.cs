using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public TMP_Text tmpText;
    public float startingTime = 120f;
    private float currentTime;
    private int minute;
    private int second;
    private bool timerFinished = false;

    private void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        minute = Mathf.Max((int)(currentTime / 60), 0);
        second = Mathf.Max((int)(currentTime % 60), 0);
        currentTime -= Time.deltaTime;
        tmpText.text = "0" + minute + ":" + (second < 10 ? "0" : "") + second;

        if (!timerFinished && currentTime <= 0)
        {
            timerOnFinish();
            timerFinished = true;
        }
    }

    void timerOnFinish()
    {
        print("Timer finished");
    }
}
