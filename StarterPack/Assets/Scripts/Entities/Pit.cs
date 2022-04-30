using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    bool isLerping;
    GameObject collider;
    Vector2 startScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isLerping)
        {
            collider.transform.localScale = Vector2.Lerp(startScale, new Vector2(0,0), 20 * Time.deltaTime);
            print("Object touched");
            if(collider.transform.localScale == new Vector3(0,0,0))
            {
                isLerping = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collider = collision.gameObject;
        collider.transform.position = this.transform.position;
        startScale = collider.transform.localScale;
        isLerping = true;
        
    }
}
