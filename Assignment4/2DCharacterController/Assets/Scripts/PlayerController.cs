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

	// Use this for initialization
	void Start ()
    {
        //get rigidbody component
        rb = GetComponent<Rigidbody2D>();

        //reset double jumps
        doubleJumped = false;
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

    }
}
