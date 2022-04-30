using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenController : MonoBehaviour
{
    public int eggsSecured;
    public float speed;
    public float finalSpeed;
    public float acceleration = 4f;
    public float baseSpeed = 4.0f;
    public float maxSpeed = 6.0f;

    public float HRaw;
    public float VRaw;

    public int playerNum = 1;

    public bool carryingEgg = false;

    private Rigidbody2D m_rigidbody;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        //Get Raws
        HRaw = 0;
        VRaw = 0;
    }




    private void FixedUpdate()
    {

        if(HRaw != Input.GetAxisRaw($"Horizontal P{playerNum}") || VRaw != Input.GetAxisRaw($"Vertical P{playerNum}"))
        {
            //Set Last Raws
            HRaw = Input.GetAxisRaw($"Horizontal P{playerNum}");
            VRaw = Input.GetAxisRaw($"Vertical P{playerNum}");

            speed = 0;
        }


        //Calc Speed
        speed += (acceleration * Time.deltaTime);
    
        //Clamp Speed
        Mathf.Clamp(speed, 0, (maxSpeed - baseSpeed));

        //Calc finalSpeed
        finalSpeed = speed + baseSpeed;

        //Clamp
        finalSpeed = Mathf.Clamp(finalSpeed, 0, maxSpeed);

        Vector2 move = new Vector2(Input.GetAxisRaw($"Horizontal P{playerNum}"), Input.GetAxisRaw($"Vertical P{playerNum}")).normalized * Time.fixedDeltaTime * finalSpeed;
        m_rigidbody.MovePosition(m_rigidbody.position + move);
    }

    public void PickupEgg()
    {
        carryingEgg = true;
        Debug.Log("Picked up egg");
    }
}
