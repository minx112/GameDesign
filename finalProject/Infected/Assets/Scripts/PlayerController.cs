using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed;
    public float jumpHeight;
    private bool doubleJumped;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

	public GameObject hitCheck;
	public float hitCheckRadius;
	public LayerMask whatIsEnemy;

	public float attackDelay;
	public float hitDelay;
	private float attackCounter;
	private float hitCounter;
	bool attack;

	SpriteRenderer m_SpriteRenderer;


	// Use this for initialization
	void Start ()
    {
        //get rigidbody component
        rb = GetComponent<Rigidbody2D>();

        //reset double jumps
        doubleJumped = false;

		//Fetch the SpriteRenderer from the GameObject
		m_SpriteRenderer = GetComponent<SpriteRenderer>();

		attack = false;
		hitCheck.SetActive (false);
	}

    private void FixedUpdate()
    {
        //ground check using transform positions
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update ()
    {
        // jumping
        if (grounded) doubleJumped = false;

		if (Input.GetKeyDown(KeyCode.Space) && (grounded || !doubleJumped))
        {
            if (!grounded && !doubleJumped) doubleJumped = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if (rb.velocity.y < -jumpHeight*2) rb.velocity = new Vector2(rb.velocity.x, -jumpHeight*2);

        //move right
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }

        //move left
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

		if (!attack && Input.GetKey (KeyCode.E)) 
		{
			attackFunction();
		}
/*
		if (attack && attackCounter > 0) 
		{
			attackCounter -= Time.deltaTime;
		}

		if (attack && attackCounter < 0)
		{
			attack = false;
			m_SpriteRenderer.color = new Color (51, 152, 0);
		}
		*/

    }

	private IEnumerator attackFunction()
	{
		attack = true;
		hitCheck.SetActive (true);
		//Set the SpriteRenderer to the Color defined by the Sliders
		m_SpriteRenderer.color = new Color (0, 0, 100);
		attackCounter = attackDelay;
		hitCounter = hitDelay;

		yield return new WaitForSeconds (hitDelay);
		m_SpriteRenderer.color = new Color (51, 152, 0);
		hitCheck.SetActive (false);
	}
}
