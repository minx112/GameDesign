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
	public static Transform playerTransform;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();//Components

	}
	void Update() {//void
		if (player.health >= 0) {
			HeartUI.sprite = HeartsSprites [player.health];
		}
		if (player.health <= 0 || GameObject.FindGameObjectWithTag("Player").transform.position.y < -10) {
			StartCoroutine (Fading ());
			//Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
		}

		if(GameObject.FindGameObjectWithTag("Player").transform.position.x >= 214 && GameObject.FindGameObjectWithTag("Player").transform.position.x < 222 && GameObject.FindGameObjectWithTag("Player").transform.position.y < 10) {//Grant your people need you
            Invoke("Win", 3f);

		}
	}
	IEnumerator Fading() {
		float fadeTime = gameObject.GetComponent<Fading>().BeginFade(1);
		Debug.Log("fade function" +  fadeTime+ " health" + player.health); 
		yield return new WaitForSeconds (0.05f);// wont wait
		//Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
		Invoke("GameOver", 0.05f);
	}

	void GameOver(){
		Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 1);
	}
	void Win() {

		Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex + 2);

	}
}