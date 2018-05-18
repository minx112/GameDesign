using System.Collections;
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
		if (player.health >= 0) {
			HeartUI.sprite = HeartsSprites [player.health];
		}
		if (player.health <= 0) {
			StartCoroutine (Fading ());
			//Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
		}
	}
	IEnumerator Fading() {
		float fadeTime = gameObject.GetComponent<Fading>().BeginFade(1);
		Debug.Log("fade function" +  fadeTime+ " health" + player.health); 
		yield return new WaitForSeconds (fadeTime);// wont wait
		//Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
		Invoke("GameOver", fadeTime);
	}

	void GameOver(){
		Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
	}
}