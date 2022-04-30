using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Debug.Log("Test");

    }

    private void LateUpdate()
    {
        TestFunctionality(1);
    }

    private bool TestFunctionality(int input)
    {
        if (input == 1) { 
            return true; 
        }

        return false;
    }
}
