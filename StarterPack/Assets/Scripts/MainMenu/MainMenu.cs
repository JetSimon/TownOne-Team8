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
    private string[] levelNames;

    [SerializeField]
    private Text[] buttons;
    private int buttonIndex = 0;

    [SerializeField]
    private Text stageText;

    [SerializeField]
    private bool canPlay = false;

    void Start()
    {
        levelNames = GameHandler.Instance.levelNames;
        currentLevel = levelNames[currentLevelIndex];
        ChangeButton(0);
        UpdateUI();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            canPlay = EnoughPlayersIn();
            UpdateUI();
        }

        if(Input.GetButtonDown("Submit"))
        {
            if(buttonIndex == 0 && canPlay)
            {
                StartGame();
            }
            else if(buttonIndex == 1)
            {
                Debug.Log("Quitting game");
                Application.Quit();
            }
            
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

        //Fix this bs later
        if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
        {
            ChangeButton(1);
        }

        //Fix this bs later
        if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
        {
            ChangeButton(-1);
        }

    }

    void SetActivePlayers()
    {
        bool[] activePlayers = {false,false,false,false};
        foreach(PlayerSelector player in GameObject.FindObjectsOfType<PlayerSelector>())
        {
            activePlayers[player.GetPlayerNumber() - 1] = player.IsJoined();
        }

        GameHandler.Instance.SetActivePlayers(activePlayers);
    }

    void StartGame()
    {
        SetActivePlayers();
        Debug.Log("Starting Game...");
        GameHandler.Instance.lastLevelPlayed = currentLevel;
        SceneManager.LoadScene(currentLevel);
        
    }

    void ChangeButton(int delta)
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
        buttons[buttonIndex].text = buttons[buttonIndex].text.Replace("- ", "");
        buttons[buttonIndex].text = buttons[buttonIndex].text.Replace(" -", "");
        buttons[buttonIndex].fontSize = 109;
        buttonIndex += delta;
        if(buttonIndex < 0) buttonIndex = buttons.Length - 1;
        buttonIndex = buttonIndex % (buttons.Length);
        buttons[buttonIndex].text = $"- {buttons[buttonIndex].text} -";
        buttons[buttonIndex].fontSize = 115;
    }

    void ChangeLevel(int delta)
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
        currentLevelIndex += delta;
        if(currentLevelIndex < 0) currentLevelIndex = levelNames.Length - 1;
        currentLevelIndex = currentLevelIndex % (levelNames.Length);
        currentLevel = levelNames[currentLevelIndex];
        UpdateUI();
        stageText.GetComponent<Animator>().SetTrigger("Pop Up");
    }

    void UpdateUI()
    {
        buttons[0].color = canPlay ? Color.white : Color.grey;
        stageText.text = $"< {currentLevel.ToUpper()} >";
    }

    bool EnoughPlayersIn()
    {
        int amountJoined = 0;

        foreach(PlayerSelector player in GameObject.FindObjectsOfType<PlayerSelector>())
        {
            if(player.IsJoined()) amountJoined++;
        }

        return amountJoined >= 2;
    }

}
