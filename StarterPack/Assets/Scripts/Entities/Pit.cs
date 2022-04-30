using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    bool shrink;
    GameObject collider;
    Vector3 scalingFactor = new Vector3(-0.1f, -0.1f, 0);
    Vector3 minSize = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (shrink == true && collider.transform.localScale.x > 0 && collider.transform.localScale.y > 0)
        {
           collider.transform.localScale += scalingFactor;
           if (collider.transform.localScale.x < 0 && collider.transform.localScale.y < 0)
            {
                collider.transform.localScale = minSize;
                collider.GetComponent<ChickenController>().Die();
                shrink = false;
                collider.GetComponent<ChickenController>().enableMove();

            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collider = collision.gameObject;
        collider.transform.position = transform.position;
        collider.GetComponent<ChickenController>().disableMove();
        shrink = true;
    }
}
