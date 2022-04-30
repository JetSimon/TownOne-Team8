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

    [SerializeField]
    private bool joined = false;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerNameText.text = playerName;
        instructionsText.text = instructions;
        joinButtonText.text = joinInstructions;
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if(Input.GetButtonDown(buttonToJoin))
        {
            if(!joined)
            {
                animator.SetTrigger("Player Joined");
                joined = true;
                UpdateUI();
            }
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
