using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChickenPanelBehaviour : MonoBehaviour
{
    public TextMeshProUGUI text_playerName;
    public TextMeshProUGUI text_eggsCount;

    public ChickenController target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            text_playerName.text = $"PLAYER {target.playerNum}";
            text_eggsCount.text = $"{target.eggsSecured} EGGS";
        }            
    }
}
