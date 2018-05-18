using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 1.5f;
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;
	// Use this for initialization
	void OnGui() {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), fadeOutTexture);
		Debug.Log ("alpha: " + alpha);
	}
	public float BeginFade(int direction) {
		fadeDir = direction;
		return (fadeSpeed);
	}
	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		Debug.Log ("OnEnable called");
	}
	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		//do stuff
		BeginFade (-1);
	}
	/*
	void OnLevelWasLoaded() {
		BeginFade (-1);

	}
	*/
}

/*
 * 
 * using UnityEngine.SceneManagement;
 
   void OnEnable() {
      SceneManager.sceneLoaded += OnSceneLoaded;
  }
 
  void OnDisable() {
      SceneManager.sceneLoaded -= OnSceneLoaded;
  }
 
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
   //do stuff
  }
 * */