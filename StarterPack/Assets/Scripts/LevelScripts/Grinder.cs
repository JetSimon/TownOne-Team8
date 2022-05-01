using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    bool grinding;
    [SerializeField] float shrinkRate = 0.95f;
    [SerializeField] Component activeSprite;
    [SerializeField] ParticleSystem chickenDeathSystem;

    bool pendingDeath = false;

    private GameObject dyingPlayer; //= collider.GetComponent<ChickenController>().gameObject;
    [SerializeField] private bool disableCollision;

    public List<GameObject> overlaps;

    private void Awake()
    {
        if(activeSprite)
        {
            activeSprite.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        if(chickenDeathSystem)
        {
            chickenDeathSystem.Stop();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (overlaps.Count > 0)
        {
            CheckOverlaps();
        }

        if (grinding == true && dyingPlayer)
        {

            dyingPlayer.GetComponent<ChickenController>().disableMove();


            if (activeSprite)
            {
                activeSprite.transform.localScale = new Vector3(1, 1, 1);
            } else
            {
                Debug.Log("Please add activeSprite Reference");
            }

            if (dyingPlayer.transform.localScale.x > 0.1f)
            {
                dyingPlayer.transform.localScale = dyingPlayer.transform.localScale * (Time.deltaTime * shrinkRate);
            } else
            {
                dyingPlayer.transform.localScale = dyingPlayer.transform.localScale * 0;

                if(!pendingDeath) 
                {
                    if(chickenDeathSystem && dyingPlayer)
                    {

                        ParticleSystem.MainModule settings = chickenDeathSystem.GetComponent<ParticleSystem>().main;
                        settings.startColor = new ParticleSystem.MinMaxGradient(dyingPlayer.GetComponent<ChickenController>().GetPlayerColor());

                        chickenDeathSystem.Play();
                    }

                    pendingDeath = true;
                    overlaps.Remove(dyingPlayer);
                    GameHandler.Instance.PlaySound("Grinder");
                    Invoke("killPlayer", 2f);
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
                if(overlap.GetComponent<ChickenController>() != null)
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
        if(dyingPlayer)
        {
            
            dyingPlayer.GetComponent<ChickenController>().Die();
            dyingPlayer.GetComponent<ChickenController>().enableMove();
            grinding = false;
            pendingDeath = false;

            dyingPlayer = null;
        } else
        {
            return;
        }

        if(activeSprite)
        {
            activeSprite.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        if(chickenDeathSystem)
        {
            chickenDeathSystem.Stop();
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
