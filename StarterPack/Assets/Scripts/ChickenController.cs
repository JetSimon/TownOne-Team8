using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenController : MonoBehaviour
{
    public float moveSpeed = 4.0f;

    public bool usesWASD = true;
    public bool usesArrows = false;

    private Rigidbody2D m_rigidbody;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 move = Vector3.zero;
        if ((Input.GetKey(KeyCode.LeftArrow) && usesArrows) || (Input.GetKey(KeyCode.A) && usesWASD)) move += Vector2.left;
        if ((Input.GetKey(KeyCode.RightArrow) && usesArrows) || (Input.GetKey(KeyCode.D) && usesWASD)) move += Vector2.right;
        if ((Input.GetKey(KeyCode.UpArrow) && usesArrows) || (Input.GetKey(KeyCode.W) && usesWASD)) move += Vector2.up;
        if ((Input.GetKey(KeyCode.DownArrow) && usesArrows) || (Input.GetKey(KeyCode.S) && usesWASD)) move += Vector2.down;
        move = move.normalized * Time.fixedDeltaTime;

        m_rigidbody.MovePosition(m_rigidbody.position + move);
    }
}
