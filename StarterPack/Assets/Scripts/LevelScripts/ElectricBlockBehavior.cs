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

        StartCoroutine(Loop());
    }
        
    private IEnumerator Loop()
    {
        while (true)
        {
            spriteRenderer.color = dangerColor;
            isSafe = false;
            yield return new WaitForSeconds(ElectricityTimeInterval);
            spriteRenderer.color = safeColor;
            isSafe = true;
            yield return new WaitForSeconds(ElectricityTimeInterval);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if (chicken != null && !chicken.stunned && !isSafe)
            chicken.Stun();
        
        //print($"ElectricBlockBehavior: {collision.gameObject.name}");
    }
}
