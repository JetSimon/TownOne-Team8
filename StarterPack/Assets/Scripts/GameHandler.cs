using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler gameHandler;
    public float eggSpawnInterval = 10;

    private int[] totalPoints = {0,0};

    private ChickenController[] chickenControllers;
    private HatchBehaviour[] spawnHatches;

    private float eggSpawnElapsed = 0;

    void Awake()
    {
        if(gameHandler != null)
        {
            Destroy(this);
            return;
        }

        gameHandler = this;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadEntities();
    }

    void LoadEntities()
    {
        chickenControllers = FindObjectsOfType<ChickenController>();
        Debug.Log($"There are {chickenControllers.Length} chickens in the match");

        spawnHatches = FindObjectsOfType<HatchBehaviour>();
        Debug.Log($"There are {spawnHatches.Length} egg spawn hatches in the match");

    }

    void AddUpPoints()
    {
        foreach(ChickenController player in chickenControllers)
        {
            totalPoints[player.playerNum-1] += player.eggsSecured;
        }
    }

    public void EndGame()
    {
        Debug.Log("<color=red>GAME OVER SCREEN NOT IN GAME YET, GOING BACK TO MAINMENU</color>");
        SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        LoadEntities();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        eggSpawnElapsed += Time.deltaTime;
        if(eggSpawnElapsed >= eggSpawnInterval)
        {
            //Try to spawn an egg at a hatch
            List<HatchBehaviour> hatches = new List<HatchBehaviour>(spawnHatches);
            while(hatches.Count > 0)
            {
                int index = Random.Range(0, hatches.Count);
                if (hatches[index].containsEgg)
                    hatches.RemoveAt(index);
                else
                {
                    hatches[index].SpawnEgg();
                    break;
                }
            }

            eggSpawnElapsed = 0;
        }
    }
}
