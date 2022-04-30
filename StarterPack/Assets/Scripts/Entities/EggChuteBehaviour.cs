using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggChuteBehaviour : MonoBehaviour
{
    public int requiredPlayerNum;
    public void OnTriggerEnter2D(Collider2D other)
    {
        var chicken = other.gameObject.GetComponent<ChickenController>();
        if (chicken != null && chicken.playerNum == requiredPlayerNum && chicken.carriedEgg != null)
            chicken.DepositEgg(gameObject);
    }
}
