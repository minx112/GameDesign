﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HUD : MonoBehaviour {

	public Sprite[] HeartsSprites;
	public Image HeartUI;
	private PlayerController player;
	public Image black;
	public Animator anim;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();//Components

	}
	void Update() {//void
		HeartUI.sprite = HeartsSprites[player.health];
		if (player.health <= 0) {
			StartCoroutine (Fading ());
			Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex - 1);
		}
	}
	IEnumerator Fading() {
		float fadeTime = GameObject.Find("Player").GetComponent<Fading>().BeginFade(1);
		Debug.Log("fade function" +  fadeTime); 
		yield return new WaitForSeconds (fadeTime);// wont wait
		//Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex - 1);
	}
}