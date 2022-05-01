using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    public Animator animator;
    public ChickenController boundChicken;
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
            boundChicken = collision.gameObject.GetComponent<ChickenController>();
            boundChicken.carriedEgg = gameObject;
            GameHandler.Instance.PlaySound("EggSpawn");
            animator.SetTrigger("Collected");
        }
    }

    public void OnEggCollected()
    {
        boundSourceHatch.GetComponent<HatchBehaviour>().containsEgg = false;
        transform.SetParent(boundChicken.transform);
        transform.localPosition = new Vector3(0.0f, 1.75f, 0);
    }
    public void OnEggDeposited()
    {
        transform.SetParent(boundDepositChute.transform);
        transform.localPosition = Vector3.up * 0.5f;
        boundChicken.eggsSecured++;
        boundChicken.carriedEgg = null;
    }
}
