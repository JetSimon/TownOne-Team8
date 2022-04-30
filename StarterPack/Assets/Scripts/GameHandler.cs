using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler gameHandler;

    private int[] totalPoints = {0,0};

    private ChickenController[] chickenControllers;

    void Awake()
    {
        if(gameHandler != null)
        {
            Destroy(this);
            return;
        }

        gameHandler = this;
    }

    void OnSceneLoaded()
    {
        chickenControllers = GameObject.FindObjectsOfType<ChickenController>();
        Debug.Log($"There are {chickenControllers.Length} chickens on screen rn man");
    }

    void AddUpPoints()
    {
        foreach(ChickenController player in chickenControllers)
        {
            totalPoints[player.playerNum-1] += player.eggs;
        }
    }

    void Start()
    {
        OnSceneLoaded();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}
