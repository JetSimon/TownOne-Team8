using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    private Animation m_collectAnimation;
    private ChickenController m_collidingChicken;

    private void Awake()
    {
        m_collectAnimation = GetComponent<Animation>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<ChickenController>() != null)
        {
            m_collidingChicken = collision.gameObject.GetComponent<ChickenController>();
            m_collectAnimation.Play();
        }
    }

    public void OnEggCollected()
    {
        m_collidingChicken.eggs++;
        Destroy(gameObject);
    }
}
