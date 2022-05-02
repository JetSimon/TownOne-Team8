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
            foreach (var chicken in queuedChickens)
                KillPlayer(chicken);
            queuedChickens.Clear();
            yield return new WaitForSeconds(interval);
        }
    }

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


    private List<ChickenController> queuedChickens = new List<ChickenController>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if (chicken != null)
        {
            if (!disableCollision)
                KillPlayer(chicken);
            else queuedChickens.Add(chicken);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var chicken = collision.gameObject.GetComponent<ChickenController>();
        if (chicken != null)
            queuedChickens.Remove(chicken);
    }
}
