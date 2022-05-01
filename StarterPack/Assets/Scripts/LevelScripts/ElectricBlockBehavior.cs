using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ElectricBlockBehavior : MonoBehaviour
{
    private GameObject sprite;
    private SpriteRenderer spriteRenderer;

    public float timeInBetweenElectricity = 4;
    public float ElectricityTimeInterval = 0.25f;
    public float timeOffset = 0;
    private bool isSafe = true;
    private float currentTime;

    private Color dangerColor;
    private Color safeColor;

    private List<ChickenController> chickenOverlaps = new List<ChickenController>();

    void Start()
    {
        // get the sprite and current time
        sprite = gameObject.transform.GetChild(0).gameObject;
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        currentTime = timeInBetweenElectricity - timeOffset;

        // get the safe and dangerous color, set the sprite to be safecolor
        dangerColor = Color.red;
        safeColor = Color.green;
        spriteRenderer.color = safeColor;
    }
        
    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            if (isSafe)
            {
                spriteRenderer.color = dangerColor;
                currentTime = ElectricityTimeInterval;
            }
            else
            {
                spriteRenderer.color = safeColor;
                currentTime = timeInBetweenElectricity;
            }
            isSafe = !isSafe;
        }
        if (!isSafe)
        {
            if (chickenOverlaps.Count > 0)
            {
                Stun();
            }
        }
    }

    private void Stun()
    {
        foreach (ChickenController chicken in chickenOverlaps)
        {
            if (!chicken.stunned)
            {
                chicken.Stun();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if (chicken != null)
        {
            chickenOverlaps.Add(chicken);
        }
        
        //print($"ElectricBlockBehavior: {collision.gameObject.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (chickenOverlaps.Contains(collision.gameObject.GetComponent<ChickenController>()))
        {
            chickenOverlaps.Remove(collision.gameObject.GetComponent<ChickenController>());
        }
    }
}
