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
    private Vector3 startingPoint;
    private Vector3 initialScale;
    private bool canMove = true;

    public GameObject carriedEgg;

    private Rigidbody2D m_rigidbody;
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    [SerializeField, Range(0,1)]
    private float slowerBy;

    private void Start()
    {
        startingPoint = transform.position;
        initialScale = transform.localScale;
    }
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

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
        float speedMod = slowerBy;

        speed += (acceleration * Time.deltaTime);
    
        //Clamp Speed
        Mathf.Clamp(speed, 0, (maxSpeed - baseSpeed));

        //Calc finalSpeed
        finalSpeed = speed + baseSpeed;

        //Clamp
        finalSpeed = Mathf.Clamp(finalSpeed, 0, maxSpeed);

        if (canMove)
        {
            if (carriedEgg == null)
            {
                speedMod = 1;
            }
            finalSpeed *= speedMod;


            Vector2 move = new Vector2(Input.GetAxisRaw($"Horizontal P{playerNum}"), Input.GetAxisRaw($"Vertical P{playerNum}")).normalized * Time.fixedDeltaTime * finalSpeed;
            m_rigidbody.MovePosition(m_rigidbody.position + move);

        }

        if(HRaw > 0) spriteRenderer.flipX = true;
        if(HRaw < 0) spriteRenderer.flipX = false;

        animator.SetBool("Walking", HRaw != 0);
    }

    public void DepositEgg(GameObject chute)
    {
        carriedEgg.GetComponent<EggBehaviour>().boundDepositChute = chute;
        carriedEgg.GetComponent<EggBehaviour>().animator.SetTrigger("Deposited");

        Debug.Log("Deposited egg");
    }

    public void Die()
    {
        if(eggsSecured > 0)
        {
            eggsSecured = 0;
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
