using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    private Transform audioManagerTransform;

    [SerializeField]
    public string[] levelNames;
    public static GameHandler Instance { get; private set; }
    public float eggSpawnInterval = 10;

    private int[] totalPoints = {0,0};

    public ChickenController[] chickenControllers;
    public HatchBehaviour[] spawnHatches;
    public EggChuteBehaviour[] eggChutes;

    private float eggSpawnElapsed = 0;

    public bool[] activePlayers = {false, false, false, false};

    public string lastLevelPlayed = "";

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
        audioManagerTransform = GameObject.Find("AudioManager").transform;
        if(audioManagerTransform)
        {
            PlaySound("Music0" + Random.Range(0,4));
        }
    }

    public void PlaySound(string s)
    {
        if(audioManagerTransform == null) return;
        audioManagerTransform.Find(s).GetComponent<AudioSource>().Play();
    }

    public void PlaySoundWithRandomPitch(string s)
    {
        if(audioManagerTransform == null) return;
        audioManagerTransform.Find(s).GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
        audioManagerTransform.Find(s).GetComponent<AudioSource>().Play();
    }

    public void StopSound(string s)
    {
        if(audioManagerTransform == null) return;
        audioManagerTransform.Find(s).GetComponent<AudioSource>().Stop();
    }

    public void StopAllSounds()
    {
        foreach(Transform t in audioManagerTransform)
        {
            t.GetComponent<AudioSource>().Stop();
        }
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
        StopAllSounds();
        GameHandler.Instance.PlaySound("LevelComplete");
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

    public void LoadRandomLevel()
    {
        int index = Random.Range(0, levelNames.Length);
        if(levelNames[index] == lastLevelPlayed)
        {
            index = (index + 1) % levelNames.Length;
        }
        lastLevelPlayed = levelNames[index];
        SceneManager.LoadScene(levelNames[index]);
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
                    GameHandler.Instance.PlaySound("EggSpawn");
                    hatches[index].SpawnEgg();
                    break;
                }
            }

            eggSpawnElapsed = 0;
        }
    }
}
