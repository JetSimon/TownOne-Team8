using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ChickenPanelBehaviour : MonoBehaviour
{
    public TextMeshProUGUI text_playerName;
    public TextMeshProUGUI text_eggsCount;

    public int targetPlayer;

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.Instance.activePlayers[targetPlayer - 1])
        {
            ChickenController target = GameHandler.Instance.chickenControllers.First(c => c.playerNum == targetPlayer);
            if (target != null)
            {
                text_playerName.text = $"PLAYER {target.playerNum}";
                text_eggsCount.text = $"{target.eggsSecured} EGGS";
            }
        }
        else gameObject.SetActive(false);
    }
}
