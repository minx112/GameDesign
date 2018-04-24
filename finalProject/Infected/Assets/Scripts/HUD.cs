﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	public Sprite[] HeartSprites;

	public Image HeartsUI;

	private PlayerController player;

	void Start() {
		player = GameObject.FindGameObjectsWithTag("Player").GetComponent<PlayerController> ();

	}

	void Update() {
		HeartsUI.sprite = HeartSprites[player.curHealth];//health object on PlayerController script
	}

}
