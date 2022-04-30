using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{

    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Vector2 appliedForce;

    private List<GameObject> activeOverlaps = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Setup ConveyerBelt");
        boxCollider.isTrigger = true;
    }


    // Update is called once per frame
    void Update()
    {
        if(activeOverlaps.Count > 0)
        {
            ApplyForces();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeOverlaps.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(activeOverlaps.Contains(collision.gameObject))
        {
            activeOverlaps.Remove(collision.gameObject);
        }
    }

    private void ApplyForces()
    {
        foreach (GameObject overlapped in activeOverlaps)
        {
            if (overlapped.GetComponent<Rigidbody2D>())
            {
                overlapped.GetComponent<Rigidbody2D>().AddForce(appliedForce);
            }
        }
    }

}
