using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{

    [SerializeField] BoxCollider2D boxCollider;

    //Pivot
    [SerializeField] Component pivot;

    private Vector2 localAppliedForce;

    



    private enum enDirection { 
        Up = 90, 
        Down = -90, 
        Left = 180, 
        Right = 0 };





    [SerializeField] private enDirection desiredDirection;



    [SerializeField] float appliedForce = 25f;

    private List<GameObject> activeOverlaps = new List<GameObject>();

    private ConveyerBelt()
    {
        Debug.Log("ConveyerBelt");
        UpdateDirection(desiredDirection);
    }

    // Start is called before the first frame update
    void Start()
    {
        setDesiredDirectionFromRot();

        //Debug.Log("Setup ConveyerBelt");
        boxCollider.isTrigger = true;

        UpdateDirection(desiredDirection);

        Debug.Log(desiredDirection);
    }

    private void setDesiredDirectionFromRot()
    {
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
                overlapped.GetComponent<Rigidbody2D>().AddForce(localAppliedForce);
            }
        }
    }

    //Call this function to change direction of conveyer belt
    private void UpdateDirection(enDirection direction)
    {
        Debug.Log("Desired Direction Called");
        Debug.Log(desiredDirection);

        if (pivot)
        {
            pivot.transform.localPosition = new Vector3(0, 0, 0);
            pivot.transform.localRotation = Quaternion.Euler(0, 0, ((float)direction));
        }

        if (direction == enDirection.Up)
        {
            localAppliedForce.Set(0, appliedForce);
        }
        
        if (direction == enDirection.Down)
        {
            localAppliedForce.Set(0, (appliedForce * -1f));
        }

        if (direction == enDirection.Left)
        {
            localAppliedForce.Set((appliedForce * -1f), 0);
        }

        if (direction == enDirection.Right)
        {
            localAppliedForce.Set(appliedForce, 0);
        }
    }

}
