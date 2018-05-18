using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

	public GameObject smallMushroom;
    public GameObject bigMushroom;
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

            // 20% chance for big mushroom, 80% chance for small mushroom
            if (Random.Range(0, 100) < 20)
            {
                Instantiate(bigMushroom, whereToSpawn, Quaternion.identity);
                bigMushroom.layer = 10;
            }
            else
            {
                Instantiate(smallMushroom, whereToSpawn, Quaternion.identity);
                smallMushroom.layer = 10;
            }
		}
	}
}
