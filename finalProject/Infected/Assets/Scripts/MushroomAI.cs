﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAI : MonoBehaviour {

	private Rigidbody2D rb;
    private Animator anim;

    public int health;
	public GameObject player;
	public float moveSpeed;
    private bool facingRight;
    private float distanceToPlayer;

    bool canAttack = true;

    public float raycastMaxDistance = 1f;
    private const int PLAYER_LAYER = 9;
    private float originOffset = 1f;


    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D>();
		
	}

    private void FixedUpdate()
    {
        RaycastCheckUpdate();
    }

    // Update is called once per frame
    void Update () {

		if (player.transform.position.x - gameObject.transform.position.x > 0)
        {
			// Debug.Log (player.transform.position.x - gameObject.transform.position.x);
			rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
            facingRight = true;
        }
		else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            facingRight = false;
        }

        distanceToPlayer = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);

        // set variables for animator
        anim.SetFloat("Move Speed", moveSpeed);
        anim.SetFloat("Distance To Player", distanceToPlayer);

        // animation flipping
        if (facingRight)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    public void takeDamage(int damage, bool isLeft)
    {
        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
        else
        {
            if(isLeft)
                rb.velocity = new Vector2(-100, rb.velocity.y);
            else
                rb.velocity = new Vector2(100, rb.velocity.y);
        }
    }

    public RaycastHit2D CheckRaycast(Vector2 direction)
    {
        float directionOriginOffset = originOffset * (direction.x > 0 ? 1 : -1);

        Vector2 startingPosition = new Vector2(transform.position.x + directionOriginOffset, transform.position.y + 0);

        return Physics2D.Raycast(startingPosition, direction, raycastMaxDistance, ~PLAYER_LAYER);
    }

    private bool RaycastCheckUpdate()
    {
        if (canAttack)
        {

            // Raycast trigger
			if (!facingRight && player.transform.position.x - gameObject.transform.position.x > -2)
            {

                // Launch a raycast in the forward direction from where the player is facing.
                Vector2 direction = new Vector2(-1, 0);

                // First target hit
                RaycastHit2D hit = CheckRaycast(direction);

                if (hit.collider)
                {
                    Debug.Log("Enemy hit the collidable object " + hit.collider.name);

                    Debug.DrawRay(transform.position, hit.point, Color.red, 0.5f);

                    hit.collider.gameObject.GetComponent<PlayerController>().takeDamage(1);

                    canAttack = false;
                    Invoke("enableAttack", 3);
                }

                return true;
            }
			else if (facingRight && player.transform.position.x - gameObject.transform.position.x < 2)
            {
                // Launch a raycast in the forward direction from where the player is facing.
                Vector2 direction = new Vector2(1, 0);

                // First target hit
                RaycastHit2D hit = CheckRaycast(direction);

                if (hit.collider)
                {
                    Debug.Log("Enemy hit the collidable object " + hit.collider.name);

                    Debug.DrawRay(transform.position, hit.point, Color.red, 0.5f);

                    hit.collider.gameObject.GetComponent<PlayerController>().takeDamage(1);

                    canAttack = false;
                    Invoke("enableAttack", 3);
                }

                return true;
            }
            else
            {
                return false;
            }

        }
        else return false;
    }

    private void enableAttack()
    {
        canAttack = true;
    }
}
