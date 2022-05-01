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

    public void ShowWinner(int winnerNumber)
    {
        resultsPanel.SetActive(true);

        
        resultsText.text = $"Player {winnerNumber} is the winner!";

        if(winnerNumber == -1)
        {
            resultsText.text = "There was a tie!";
        }
        
        Time.timeScale = 0;
    }

    public void Update()
    {
        if(Input.GetKeyDown("space") && resultsPanel.activeSelf)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}