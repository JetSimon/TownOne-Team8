using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenController : MonoBehaviour
{
    public int eggs;
    public float moveSpeed = 4.0f;
    public int playerNum = 1;

    private Rigidbody2D m_rigidbody;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw($"Horizontal P{playerNum}"), Input.GetAxisRaw($"Vertical P{playerNum}")).normalized * Time.fixedDeltaTime * moveSpeed;
        m_rigidbody.MovePosition(m_rigidbody.position + move);
    }

    public void OnPlayerJoined()
    {
        Debug.Log("Hello?");
    }
}
