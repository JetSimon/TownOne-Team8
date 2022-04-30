using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField]
    private string buttonToJoin, buttonToLeave, playerName, instructions, joinInstructions;

    [SerializeField]
    private Color playerColor;

    [SerializeField]
    private Text playerNameText, instructionsText, joinButtonText;

    [SerializeField]
    private Image playerImage;

    private bool joined = false;

    // Start is called before the first frame update
    void Start()
    {
        playerNameText.text = playerName;
        instructionsText.text = instructions;
        joinButtonText.text = joinInstructions;

        UpdateUI();
    }

    void Update()
    {
        if(Input.GetButtonDown(buttonToJoin))
        {
            joined = true;
            UpdateUI();
        }

        if(Input.GetButtonDown(buttonToLeave))
        {
            joined = false;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        playerNameText.gameObject.SetActive(joined);
        joinButtonText.gameObject.SetActive(!joined);
        instructionsText.gameObject.SetActive(joined);
        playerImage.color = joined ? playerColor : Color.black;
    }

    //To use when checking if all players have joined
    public bool IsJoined()
    {
        return joined;
    }
}
