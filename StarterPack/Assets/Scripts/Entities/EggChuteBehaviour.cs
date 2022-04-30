using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggChuteBehaviour : MonoBehaviour
{
    public int requiredPlayerNum;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger enter wtih " + other.gameObject.name);

        var chicken = other.gameObject.GetComponent<ChickenController>();
        if (chicken != null && chicken.playerNum == requiredPlayerNum && chicken.carryingEgg)
            chicken.DepositEgg(gameObject);
    }
}
