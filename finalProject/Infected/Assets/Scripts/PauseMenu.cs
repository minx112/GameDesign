using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour {

	public static bool gameIsPaused = false;
	public GameObject pauseMenuUI;
	// Use this for initialization
	void Start () {
		pauseMenuUI.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (gameIsPaused) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}
	public void Resume() {
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		gameIsPaused = false;
		//sound
	}

	void Pause() {
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		gameIsPaused = true;
		//sound
	}
	public void LoadMenu() {
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}

	public void QuitGame() {
		Debug.Log ("Quit");
		Application.Quit ();

	}

}
