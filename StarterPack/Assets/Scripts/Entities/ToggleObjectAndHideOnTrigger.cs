using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectAndHideOnTrigger : MonoBehaviour
{

    [SerializeField]
    private GameObject toToggle, twin;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            toToggle.SetActive(!toToggle.activeSelf);
            gameObject.SetActive(false);
            twin.SetActive(true);
        }
    }
}
