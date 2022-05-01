using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    GameObject dyingPlayer;

    private void KillPlayer(ChickenController controller)
    {
        StartCoroutine(_KillLoop(controller));
    }
    private IEnumerator _KillLoop(ChickenController controller)
    {
        dyingPlayer = controller.gameObject;
        dyingPlayer.GetComponent<ChickenController>().disableMove();

        Vector3 initialScale = dyingPlayer.transform.localScale;
        Vector3 initialPosition = dyingPlayer.transform.position;

        float t = 0;
        while (t <= 1)
        {
            dyingPlayer.transform.localScale = initialScale * Mathf.Lerp(1, 0, Mathf.Pow(t, 0.5f));
            dyingPlayer.transform.position = Vector3.Lerp(initialPosition, transform.position, Mathf.Pow(t, 0.33f));
            t += Time.deltaTime / 0.5f;
            yield return null;
        }
        dyingPlayer.GetComponent<ChickenController>().Die();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if (chicken != null)
            KillPlayer(chicken);
        GameHandler.Instance.PlaySound("Pit");
    }
}
