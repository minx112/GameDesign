﻿using System.Collections;
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
	public int health;

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

		if (!attack && keyE)
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

        // set float 'Speed' for animator
        anim.SetFloat("Horizontal Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Vertical Speed", rb.velocity.y);

        // animation flipping
        if (keyD || (rb.velocity.x > 0))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (keyA || (rb.velocity.x < 0))
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

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
