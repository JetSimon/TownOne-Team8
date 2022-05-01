using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;

    [SerializeField]
    private string buttonToJoin, buttonToLeave, instructions, joinInstructions;

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
        playerNameText.text = $"P{playerNumber}";
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
                GetComponent<AudioSource>().pitch = Random.Range(0.9f,1.1f);
                GetComponent<AudioSource>().Play();
                animator.SetTrigger("Player Joined");
                joined = true;
                UpdateUI();
            }
        }

        if(Input.GetButtonDown(buttonToLeave) && joined)
        {
            //THIS IS VERY BAD ITS JUST FOR NOW OK
            GameObject.Find("Main Menu Manager").GetComponent<AudioSource>().Play();
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

    public int GetPlayerNumber()
    {
        return playerNumber;
    }
}
