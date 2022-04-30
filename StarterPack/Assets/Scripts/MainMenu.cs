using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Canvas mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void CloseMainMenu()
    {
        if(mainMenu)
        {
            mainMenu.enabled = false;
        }
         
    }
}