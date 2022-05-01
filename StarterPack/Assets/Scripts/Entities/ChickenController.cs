using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenController : MonoBehaviour
{
    //Publics
    [Header("Player")]
    public int playerNum = 1;

    [Header("Speed Tuneables")]
    public int eggsSecured;
    public float speed;
    public float finalSpeed;
    public float acceleration = 4f;
    public float baseSpeed = 4.0f;
    public float maxSpeed = 6.0f;

    public float eggHoldSpeedModifier = 0.9f;
    public float stunDuration = 2.00f;
    
    [Header("Raws")]
    public float HRaw;
    public float VRaw;

    //Privates
    private Vector3 startingPoint;
    private Vector3 initialScale;
    private bool canMove = true;

    [Header("Inspects")]
    public GameObject carriedEgg;
    public bool stunned;

    private Rigidbody2D m_rigidbody;
    private SpriteRenderer spriteRenderer;

    private Animator animator;

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
        speed += (acceleration * Time.deltaTime);
    
        //Clamp Speed
        Mathf.Clamp(speed, 0, (maxSpeed - baseSpeed));

        //Calc finalSpeed
        finalSpeed = speed + baseSpeed;
        finalSpeed = Mathf.Clamp(finalSpeed, 0, maxSpeed);
        finalSpeed *= carriedEgg != null ? eggHoldSpeedModifier : 1.0f;

        if (canMove && !stunned)
        {
            Vector2 move = new Vector2(Input.GetAxisRaw($"Horizontal P{playerNum}"), Input.GetAxisRaw($"Vertical P{playerNum}")).normalized * Time.fixedDeltaTime * finalSpeed;
            m_rigidbody.MovePosition(m_rigidbody.position + move);

        }

        if(HRaw > 0) spriteRenderer.flipX = true;
        if(HRaw < 0) spriteRenderer.flipX = false;

        animator.SetBool("Walking", HRaw != 0);


        RigidbodyConstraints2D normalConstraints = RigidbodyConstraints2D.FreezeRotation;
        RigidbodyConstraints2D stunnedConstraints = normalConstraints | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        m_rigidbody.constraints = stunned ? stunnedConstraints : normalConstraints;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var otherChicken = collision.gameObject.GetComponent<ChickenController>();
        if(otherChicken != null)
        {
            if(carriedEgg == null && otherChicken.carriedEgg != null && !stunned)
            {
                carriedEgg = otherChicken.carriedEgg;
                carriedEgg.GetComponent<EggBehaviour>().boundChicken = this;
                carriedEgg.transform.SetParent(transform);
                carriedEgg.transform.localPosition = new Vector3(0.0f, 1.75f, 0);

                                otherChicken.carriedEgg = null;
                otherChicken.Stun();
            }
        }
    }
    public void Stun()
    {
        StartCoroutine(StunnedCoroutine());
    }

    private IEnumerator StunnedCoroutine()
    {
        animator.SetTrigger("Hurt");

        stunned = true;
        Color initialColor = spriteRenderer.color;

        float t = 0;
        while(t <= 1)
        {
            spriteRenderer.color = Color.Lerp(initialColor, Color.grey, Mathf.Round(Mathf.Abs(Mathf.Sin(t * 20))));
            t += Time.deltaTime / stunDuration;
            yield return null;
        }
        spriteRenderer.color = initialColor;
        stunned = false;

    }
}
