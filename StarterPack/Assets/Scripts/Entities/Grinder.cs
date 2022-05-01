using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    bool grinding;
    [SerializeField] SpriteRenderer activeSprite;
    [SerializeField] ParticleSystem chickenDeathSystem;
    Color playerColor;
    [SerializeField] Material refMaterial;

    private GameObject dyingPlayer; //= collider.GetComponent<ChickenController>().gameObject;
    [SerializeField] private bool disableCollision;

    private void KillPlayer(ChickenController controller)
    {
        StartCoroutine(_KillLoop(controller));
    }
    private IEnumerator _KillLoop(ChickenController controller)
    {
        dyingPlayer = controller.gameObject;
        dyingPlayer.GetComponent<ChickenController>().disableMove();
        grinding = true;

        ParticleSystem.MainModule settings = chickenDeathSystem.GetComponent<ParticleSystem>().main;
        ParticleSystem particleSystemLocal = chickenDeathSystem.GetComponent<ParticleSystem>();
        settings.startColor = new ParticleSystem.MinMaxGradient(dyingPlayer.GetComponent<ChickenController>().GetPlayerColor());
        particleSystemLocal.GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1.0f);
        chickenDeathSystem.Play();

        Vector3 initialScale = dyingPlayer.transform.localScale;
        Vector3 initialPosition = dyingPlayer.transform.position;

        float t = 0;
        while(t <= 1)
        {
            dyingPlayer.transform.localScale = initialScale * Mathf.Lerp(1, 0, Mathf.Pow(t, 0.5f));
            dyingPlayer.transform.position = Vector3.Lerp(initialPosition, transform.position, Mathf.Pow(t, 0.33f));
            t += Time.deltaTime / 0.5f;
            yield return null;
        }
        dyingPlayer.GetComponent<ChickenController>().Die();
        grinding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if (!grinding && chicken != null) KillPlayer(chicken);
    }
}
