using System.Collections;
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

    private float jumpWait = 3f;
    public float jumpHeight;

    public GameObject ashPileSprite;

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

        if (player.transform.position.y - 1 > gameObject.transform.position.y && jumpWait <= 0 && player.transform.position.x - gameObject.transform.position.x >= -5 && player.transform.position.x - gameObject.transform.position.x <= 5)
        {
            jumpWait = 3f;
            jumpWait -= Random.Range(0, 3);
        }

        if (jumpWait > 0)
        {
            jumpWait -= Time.deltaTime;
            if (jumpWait <= 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
        }

    }

    public void takeDamage(int damage, bool isLeft)
    {
        health -= damage;

        if (health <= 0)
        {
            Instantiate(ashPileSprite, (gameObject.transform.position + new Vector3(0, 0.5f, 0)), Quaternion.identity);
            Destroy(gameObject);
        }
            
        else
        {
            if(player.transform.position.x - gameObject.transform.position.x > 0)
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
