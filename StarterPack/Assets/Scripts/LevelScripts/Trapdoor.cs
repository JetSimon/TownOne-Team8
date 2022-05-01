using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    bool grinding;
    [SerializeField] float shrinkRate = 0.95f;

    [SerializeField] float deathWait = 2f;

    bool pendingDeath = false;

    public float interval = 2.0f;

    private GameObject dyingPlayer; //= collider.GetComponent<ChickenController>().gameObject;
    [SerializeField] private bool disableCollision;

    public List<GameObject> overlaps;
    public List<GameObject> pendingDead;

    public Animator animator;

    private void Start()
    {
        StartCoroutine(Loop());
    }
    private IEnumerator Loop()
    {
        while (true)
        {
            animator.SetBool("Open", false);
            yield return new WaitForSeconds(0.2f);
            disableCollision = true;
            yield return new WaitForSeconds(interval);
            animator.SetBool("Open", true);
            yield return new WaitForSeconds(0.2f);
            disableCollision = false;
            yield return new WaitForSeconds(interval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dirty Force
        //this.transform.position = (new Vector3(this.transform.position.x, this.transform.position.y, -0.5f));
        //this.transform.SetPositionAndRotation(new Vector3(this.transform.position.x, this.transform.position.y, -0.5f), Quaternion.Euler(0,0,0));

        if (overlaps.Count > 0)
        {
            CheckOverlaps();
        }

        foreach (GameObject overlap in overlaps)
        {
            if(!disableCollision)
            {
                if(overlap.GetComponent<ChickenController>())
                {
                    overlap.GetComponent<ChickenController>().disableMove();
                    overlap.transform.position = this.transform.position;

                    pendingDead.Add(overlap);

                    overlaps.Remove(overlap);

                    break;
                }
            }
        }

        foreach (GameObject dying in pendingDead)
        {

            if(dying.transform.localScale.x > 0.1f) 
            {
                dying.transform.localScale = dying.transform.localScale * 0.95f;
            } else
            {
                dying.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(killSpecificPlayer(dying, deathWait));
                pendingDead.Remove(dying);

                break;
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

    IEnumerator killSpecificPlayer(GameObject player, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // Now do your thing here

        if (player)
        {
            player.GetComponent<SpriteRenderer>().enabled = true;

            player.GetComponent<ChickenController>().Die();
            
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
