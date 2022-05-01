using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    bool grinding;
    [SerializeField] float shrinkRate = 0.95f;
    [SerializeField] Component activeSprite;

    bool pendingDeath = false;

    private GameObject dyingPlayer; //= collider.GetComponent<ChickenController>().gameObject;
    [SerializeField] private bool disableCollision;

    public List<GameObject> overlaps;

    private void Awake()
    {
    
    }

    private void Start()
    {
        Invoke("StartCycle", 2f);
    }

    private void StartCycle()
    {
        if(activeSprite)
        {
            activeSprite.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            disableCollision = false;
        }
        Invoke("CycleStep", 2f);
    }

    private void CycleStep()
    {
        if (activeSprite)
        {
            activeSprite.transform.localScale = new Vector3(1f, 1f, 1f);
            disableCollision = true;
        }
        Invoke("StartCycle", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (overlaps.Count > 0)
        {
            CheckOverlaps();
        }

        foreach (GameObject overlap in overlaps)
        {
            if(!disableCollision)
            {
                killPlayer();
            }
        }

        {
            dyingPlayer.GetComponent<ChickenController>().disableMove();

            if (dyingPlayer.transform.localScale.x > 0.1f)
            {
                dyingPlayer.transform.localScale = dyingPlayer.transform.localScale * (Time.deltaTime * shrinkRate);
            }
            else
            {
                dyingPlayer.transform.localScale = dyingPlayer.transform.localScale * 0;

                if (!pendingDeath)
                {
                    pendingDeath = true;
                    overlaps.Remove(dyingPlayer);
                }

            }
        }

    }

    private void CheckOverlaps()
    {
        if (!disableCollision)
        {
            foreach (GameObject overlap in overlaps)
            {
                if (overlap.GetComponent<ChickenController>() != null)
                {
                    if (grinding == false)
                    {
                        dyingPlayer = overlap.gameObject;

                        dyingPlayer.transform.position = this.transform.position;
                        dyingPlayer.GetComponent<ChickenController>().disableMove();

                        grinding = true;
                    }
                    else
                    {
                        Debug.Log("Already Grinding");
                    }
                }
            }
        }
        else
        {

        }
    }

    private void killPlayer()
    {
        if (dyingPlayer)
        {
            dyingPlayer.GetComponent<ChickenController>().Die();
            dyingPlayer.GetComponent<ChickenController>().enableMove();
            grinding = false;
            pendingDeath = false;

            dyingPlayer = null;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        overlaps.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        overlaps.Remove(collision.gameObject);
    }
}
