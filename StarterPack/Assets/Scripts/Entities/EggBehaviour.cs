using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    public Animator animator;
    private ChickenController m_collidingChicken;
    public GameObject boundDepositChute;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if(chicken != null && !chicken.carryingEgg)
        {
            m_collidingChicken = collision.gameObject.GetComponent<ChickenController>();
            animator.SetTrigger("Collected");
        }
    }

    public void OnEggCollected()
    {
        m_collidingChicken.PickupEgg(gameObject);
    }
    public void OnEggDeposited()
    {
        transform.SetParent(boundDepositChute.transform);
        transform.localPosition = Vector3.up * 0.5f;
        m_collidingChicken.eggsSecured++;
        m_collidingChicken.carryingEgg = false;
    }
}
