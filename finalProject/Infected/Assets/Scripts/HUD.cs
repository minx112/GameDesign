using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HUD : MonoBehaviour {

	public Sprite[] HeartsSprites;
	public Image HeartUI;
	private PlayerController player;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();//Components

	}
	void Update() {
		HeartUI.sprite = HeartsSprites[player.health];
		if (player.health <= 0) {
			Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
		}
		//Debug.Log("Player Health HUD" +  player.health); // player.health not pointing to it
	}
}