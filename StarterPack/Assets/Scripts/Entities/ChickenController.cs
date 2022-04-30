using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenController : MonoBehaviour
{
    public int eggs;
    public float moveSpeed = 4.0f;
    public int playerNum = 1;
    private Vector3 startingPoint;
    private Vector3 initialScale;
    private bool canMove = true;

    private Rigidbody2D m_rigidbody;

    private void Start()
    {
        startingPoint = transform.position;
        initialScale = transform.localScale;
    }
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 move = new Vector2(Input.GetAxisRaw($"Horizontal P{playerNum}"), Input.GetAxisRaw($"Vertical P{playerNum}")).normalized * Time.fixedDeltaTime * moveSpeed;
            m_rigidbody.MovePosition(m_rigidbody.position + move);

        }

    }

    public void OnPlayerJoined()
    {
        Debug.Log("Hello?");
    }

    public void Die()
    {
        if(eggs > 0)
        {
            eggs = 0;
        }
        transform.position = startingPoint;
        transform.localScale = initialScale;
    }

    public void enableMove()
    {
        canMove = true;
    }

    public void disableMove()
    {
        canMove = false;
    }
}
