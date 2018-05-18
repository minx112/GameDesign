using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed;
    public float jumpHeight;
    private double jumpDelay;
    private double jumpDelayTime = 0.1;
    private bool doubleJumped;
    private bool facingRight;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

	public LayerMask whatIsEnemy;

	public float attackDelay;
	public float hitDelay;
	private float attackCounter;
	private float hitCounter;
	bool attack;
	public int health;

    public Transform laserPoint;
    public float raycastMaxDistance = 1f;
    private const int ENEMY_LAYER = 10;
    private float originOffset = 2f;
    public LineRenderer laserLine;
    private double laserLineDelay;
    private double laserLineDelayTime = 0.25;

    SpriteRenderer m_SpriteRenderer;

    // Animation
    private Animator anim;

    // Control Keys
    [HideInInspector] public bool keyA;
    [HideInInspector] public bool keyD;
    [HideInInspector] public bool keyE;
    [HideInInspector] public bool keyHoldE;
    [HideInInspector] public bool keyR;
    [HideInInspector] public bool keyHoldR;
    [HideInInspector] public bool keyQ;
    [HideInInspector] public bool keyHoldQ;
    [HideInInspector] public bool keySpace;
    [HideInInspector] public bool keyMouseClick;

    // Use this for initialization
    void Start ()
    {
        //get animator component
        anim = GetComponent<Animator>();

        //set health to max
        health = 3;

        //get rigidbody component
        rb = GetComponent<Rigidbody2D>();

        //reset double jumps
        doubleJumped = false;

		//Fetch the SpriteRenderer from the GameObject
		m_SpriteRenderer = GetComponent<SpriteRenderer>();

		attack = false;
	}

    private void FixedUpdate()
    {
        //ground check using transform positions
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        RaycastCheckUpdate();
    }

    // Update is called once per frame
    void Update ()
    {
        // get input
        keyA = Input.GetKey(KeyCode.A);
        keyD = Input.GetKey(KeyCode.D);
        keyE = Input.GetKeyDown(KeyCode.E);
        keyHoldE = Input.GetKey(KeyCode.E);
        keyR = Input.GetKeyDown(KeyCode.R);
        keyHoldR = Input.GetKey(KeyCode.R);
        keyQ = Input.GetKeyDown(KeyCode.Q);
        keyHoldQ = Input.GetKey(KeyCode.Q);
        keySpace = Input.GetKeyDown(KeyCode.Space);
        keyMouseClick = Input.GetMouseButtonDown(0);

        // set variables for animator
        anim.SetBool("Grounded", grounded);

        // jumpDelay
        if (jumpDelay > 0)
        {
            jumpDelay -= Time.deltaTime;
            if (jumpDelay <= 0) jumpDelay = 0;
        }
        
        // if grounded
        if (grounded)
        {
            // doubleJumped = false;
            // anim.SetBool("Double Jumped", false);
            if (jumpDelay <= 0) anim.SetBool("Jumped", false);
        }

        // jumping
        if (keySpace && grounded)
        {
            // if (!grounded && !doubleJumped)
            // {
            //     doubleJumped = true;
            //     anim.SetBool("Double Jumped", true);
            // }
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            anim.SetBool("Jumped", true);
            jumpDelay = jumpDelayTime;
        }

        if (rb.velocity.y < -jumpHeight*2) rb.velocity = new Vector2(rb.velocity.x, -jumpHeight*2);

        // move right
        if (keyD)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }

        // move left
        if (keyA)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        if (!keyA && !keyD)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // set float 'Speed' for animator
        anim.SetFloat("Horizontal Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Vertical Speed", rb.velocity.y);

        // animation flipping
        if (keyD || (rb.velocity.x > 0))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            facingRight = true;
        }
        else if (keyA || (rb.velocity.x < 0))
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            facingRight = false;
        }

        // eye laser animation and timer
        if (laserLine.enabled)
        {
            laserLine.SetPosition(0, laserPoint.position);
        }

        if (laserLineDelay > 0)
        {
            laserLineDelay -= Time.deltaTime;

            if (laserLineDelay < 0) laserLineDelay = 0;
        }

        if (laserLineDelay <= 0)
        {
            laserLine.enabled = false;
        }

    }

    public RaycastHit2D CheckRaycast(Vector2 direction)
    {
        float directionOriginOffset = originOffset * (direction.x > 0 ? 1 : -1);

        Vector2 startingPosition = new Vector2(transform.position.x + directionOriginOffset, transform.position.y+1);

        return Physics2D.Raycast(startingPosition, direction, raycastMaxDistance, ~ENEMY_LAYER);
    }

    private bool RaycastCheckUpdate()
    {
        // Raycast button pressed
        if (keyE || keyMouseClick)
        {
            // Launch a raycast in the forward direction from where the player is facing.
            Vector2 direction;

            // If facing right
            if (facingRight)
                direction = new Vector2(1, 0);
            else
                direction = new Vector2(-1, 0);

            // First target hit
            RaycastHit2D hit = CheckRaycast(direction);

            if (hit.collider)
            {
                // transform of the enemy being hit
                Transform targetTransform = hit.transform;
                // hitPosition.position = new Vector3(hit.transform.position.x, hit.transform.position.y - 3, hit.transform.position.z);

                // position of the target's feet
                // used for aiming the laser and placing the resulting ash pile
                Vector3 targetGroundPos = targetTransform.position - laserPoint.position;

                // position to fire laser at
                Vector3 laserTargetPos = targetGroundPos + new Vector3(0, 3, 0);

                // deal damage to target
                hit.collider.gameObject.GetComponent<MushroomAI>().takeDamage(1, (rb.velocity.x < 0));

                // laser effects and timer
                laserLine.enabled = true;
                laserLine.SetPosition(0, laserPoint.position);
                laserLine.SetPosition(1, laserPoint.position + laserTargetPos);
                laserLineDelay = laserLineDelayTime;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player health: " + health);
    }
}
