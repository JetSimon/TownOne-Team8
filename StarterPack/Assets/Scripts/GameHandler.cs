using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{
    public PlayerInputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager.JoinPlayer();
        inputManager.JoinPlayer();
        Debug.Log("Joined players");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
