using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    private Animator m_animator;
    private ChickenController m_collidingChicken;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<ChickenController>() != null)
        {
            m_collidingChicken = collision.gameObject.GetComponent<ChickenController>();
            m_animator.SetTrigger("Collected");
        }
    }

    public void OnEggCollected()
    {
        m_collidingChicken.eggs++;
        Destroy(gameObject);
    }
}
