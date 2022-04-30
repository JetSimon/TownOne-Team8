using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string currentLevel;
    private int currentLevelIndex = 0;

    [SerializeField]
    private string[] levelNames;

    [SerializeField]
    private Text startButton, stageText;

    [SerializeField]
    private bool canPlay = false;

    void Start()
    {
        currentLevel = levelNames[currentLevelIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            canPlay = AllPlayersIn();
            UpdateUI();
        }

        if(canPlay && Input.GetButtonDown("Submit"))
        {
            StartGame();
        }

        //Fix this bs later
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            ChangeLevel(-1);
        }

        if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            ChangeLevel(1);
        }

    }

    void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene(currentLevel);
    }

    void ChangeLevel(int delta)
    {
        currentLevelIndex += delta;
        currentLevelIndex = currentLevelIndex % (levelNames.Length - 1);
        currentLevel = levelNames[currentLevelIndex];
        UpdateUI();
    }

    void UpdateUI()
    {
        startButton.color = canPlay ? Color.white : Color.grey;
        stageText.text = $"< {currentLevel.ToUpper()} >";
    }

    bool AllPlayersIn()
    {
        bool allIn = true;

        foreach(PlayerSelector player in GameObject.FindObjectsOfType<PlayerSelector>())
        {
            if(!player.IsJoined()) allIn = false;
        }

        return allIn;
    }

}
