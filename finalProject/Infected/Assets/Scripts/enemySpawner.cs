using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

	public GameObject enemy;
	Vector2 whereToSpawn;
	public float spawnRate = 2f;
	float nextSpawn = 0.0f;
	public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if (Time.time > nextSpawn) {
			nextSpawn = Time.time + spawnRate;

			whereToSpawn = new Vector2 (25f+player.position.x, transform.position.y);
			Instantiate (enemy, whereToSpawn, Quaternion.identity);
			enemy.layer = 10;
		}
	}
}
