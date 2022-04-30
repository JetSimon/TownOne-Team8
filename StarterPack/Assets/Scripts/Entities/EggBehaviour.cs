using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    public Animator animator;
    private ChickenController m_collidingChicken;

    public GameObject boundSourceHatch;
    public GameObject boundDepositChute;

    public bool pickedUp = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if(!pickedUp && chicken != null && chicken.carriedEgg == null)
        {
            pickedUp = true;
            m_collidingChicken = collision.gameObject.GetComponent<ChickenController>();
            m_collidingChicken.carriedEgg = gameObject;
            animator.SetTrigger("Collected");
        }
    }

    public void OnEggCollected()
    {
        boundSourceHatch.GetComponent<HatchBehaviour>().containsEgg = false;

        transform.SetParent(m_collidingChicken.transform);
        transform.localPosition = new Vector3(0.0f, 1.75f, 0);
    }
    public void OnEggDeposited()
    {
        transform.SetParent(boundDepositChute.transform);
        transform.localPosition = Vector3.up * 0.5f;
        m_collidingChicken.eggsSecured++;
        m_collidingChicken.carriedEgg = null;
    }
}
