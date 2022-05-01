using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameHud : MonoBehaviour
{
    [SerializeField]
    GameObject resultsPanel;

    [SerializeField]
    Text resultsText;

    [SerializeField]
    Image winnerImage;

    public void ShowWinner(int winnerNumber, Color winnerColor)
    {
        resultsPanel.SetActive(true);

        
        resultsText.text = $"Player {winnerNumber} is the winner!";
        winnerImage.color = winnerColor;

        if(winnerNumber == -1)
        {
            resultsText.text = "There was a tie!";
        }

        Time.timeScale = 0;
    }

    public void Update()
    {
        if(resultsPanel.activeSelf)
        {
            if(Input.GetKeyDown("space"))
            {
                Time.timeScale = 1;
                GameHandler.Instance.LoadRandomLevel();
            }
            else if(Input.anyKeyDown)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");   
            }
        }
    }
}
