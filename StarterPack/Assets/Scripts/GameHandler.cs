using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    public float eggSpawnInterval = 10;

    private int[] totalPoints = {0,0};

    public ChickenController[] chickenControllers;
    public HatchBehaviour[] spawnHatches;
    public EggChuteBehaviour[] eggChutes;

    private float eggSpawnElapsed = 0;

    public bool[] activePlayers = {false, false, false, false};

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadEntities();
    }

    void LoadEntities()
    {
        chickenControllers = FindObjectsOfType<ChickenController>();
        spawnHatches = FindObjectsOfType<HatchBehaviour>();
        eggChutes = FindObjectsOfType<EggChuteBehaviour>();

        foreach (var controller in chickenControllers)
            controller.gameObject.SetActive(activePlayers[controller.playerNum - 1]);

        foreach (var chute in eggChutes)
            chute.gameObject.SetActive(activePlayers[chute.requiredPlayerNum - 1]);
    }

    void AddUpPoints()
    {
        foreach(ChickenController player in chickenControllers)
        {
            totalPoints[player.playerNum-1] += player.eggsSecured;
        }
    }

    public void SetActivePlayers(bool[] arr)
    {
        activePlayers = arr;
    }

    public void EndGame()
    {
        int winnerNumber = 0;
        int winnerPoints = 0;
        Color winnerColor = Color.white;
        bool tied = false;

        foreach(ChickenController player in chickenControllers)
        {
            if(player.eggsSecured > winnerPoints)
            {
                tied = false;
                winnerNumber = player.playerNum;
                winnerPoints = player.eggsSecured;
                winnerColor = player.GetPlayerColor();
            }
            else if(player.eggsSecured == winnerPoints)
            {
                tied = true;
            }
        }

        if(tied)
        {
            winnerColor = Color.white;
            winnerNumber = -1;
        }

        GameObject.Find("GameHUD").GetComponent<GameHud>().ShowWinner(winnerNumber, winnerColor);
        
        
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
