using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {

	private Rigidbody2D rb;
	public GameObject player;
	public float moveSpeed;

	// Use this for initialization
	void Start () {
		
		player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (player.transform.position.x - gameObject.transform.position.x > 0) {
			// Debug.Log (player.transform.position.x - gameObject.transform.position.x);
			rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
		}
		else
			rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
		
	}
}
