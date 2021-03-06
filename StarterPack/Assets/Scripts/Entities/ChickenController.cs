using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenController : MonoBehaviour
{
    //Publics
    [Header("Player")]
    public int playerNum = 1;
    [SerializeField]
    private float respawnTime = 1;

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

    private AudioSource cluckSound;
    private Color initialColor;

    public GameObject wing;
    private SpriteRenderer wingRenderer;

    [SerializeField]
    private Sprite[] hats;

    [SerializeField]
    private SpriteRenderer hatRenderer;

    private void Start()
    {
        startingPoint = transform.position;
        initialScale = transform.localScale;

        initialColor = spriteRenderer.color;

    }
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cluckSound = GetComponent<AudioSource>();

        wingRenderer = wing.GetComponent<SpriteRenderer>();

        //hatRenderer.color = spriteRenderer.color;
        //hatRenderer.sprite = hats[Random.Range(0, hats.Length)];

        //Get Raws
        HRaw = 0;
        VRaw = 0;
    }

    public Color GetPlayerColor()
    {
        return initialColor;
    }


    private void Update()
    {
        if (GetComponentInChildren<EggBehaviour>() != null)
            carriedEgg.transform.localPosition = spriteRenderer.flipX ? new Vector3(1.00f, 0, 0) : new Vector3(-1.10f, 0, 0);

        wingRenderer.color = spriteRenderer.color;
    }

    private void FixedUpdate()
    {
        if(Input.GetButtonDown($"Fire P{playerNum}"))
        {
            GameHandler.Instance.PlaySoundWithRandomPitch("Cluck0" + Random.Range(1,8));
        }

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


        wing.transform.localPosition = carriedEgg != null ? new Vector3(-0.6f, -0.3f, 0) : new Vector3(0.33f, 0, 0);
        wing.transform.localScale = carriedEgg != null ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

        if (spriteRenderer.flipX)
        {
            if (carriedEgg == null)
                wing.transform.localPosition -= Vector3.right * 0.65f;
            else wing.transform.localPosition += Vector3.right * 0.85f;

            var s = wing.transform.localScale;
            s.x *= -1;
            wing.transform.localScale = s;
        }

        animator.SetBool("Walking", HRaw != 0);

        if(HRaw != 0 || VRaw != 0)
        {
            //GameHandler.Instance.PlaySound("Walk");
        }


        RigidbodyConstraints2D normalConstraints = RigidbodyConstraints2D.FreezeRotation;
        RigidbodyConstraints2D stunnedConstraints = normalConstraints | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        m_rigidbody.constraints = stunned ? stunnedConstraints : normalConstraints;
    }

    public void DepositEgg(GameObject chute)
    {
        carriedEgg.GetComponent<EggBehaviour>().boundDepositChute = chute;
        carriedEgg.GetComponent<EggBehaviour>().animator.SetTrigger("Deposited");

        Debug.Log("Deposited egg");
        GameHandler.Instance.PlaySound("EggDeliver");
    }

    public void Die()
    {
        GameHandler.Instance.PlaySound("Death");

        if(carriedEgg)
        {
            carriedEgg.GetComponent<EggBehaviour>().boundSourceHatch.GetComponent<HatchBehaviour>().spawnedEggExists = false;
            Destroy(carriedEgg);
        }



        StartCoroutine(Respawn());


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
        GameHandler.Instance.PlaySoundWithRandomPitch("Cluck0" + Random.Range(1,8));
        StartCoroutine(StunnedCoroutine());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        transform.position = startingPoint;
        transform.localScale = initialScale;
        enableMove();
    }

    private IEnumerator StunnedCoroutine()
    {
        animator.SetTrigger("Hurt");

        stunned = true;

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
