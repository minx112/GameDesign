using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour {
	public Color yellow = Color.yellow;
	public Color magenta = Color.magenta;
	public Color blue = Color.blue;
	public Color cyan = Color.cyan;
	public Color red = Color.red;
	public Color green = Color.green;
	public Image Fill;
	public Slider progress;
	private int counter = 0;
	//becoming insane 7:20 - 440 secs
	void Start () {
		progress.value = 0;
		InvokeRepeating("Prog", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (counter >= 5) {
			Fill.color = Color.Lerp (yellow, blue, Mathf.PingPong (Time.time, 1));
		} else if (counter >= 10) { 

		}
	}
	void Prog() {
		progress.value += .0022727f;
		counter++;
		if (counter >= 440) {
			Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex +1);

		}
	}
}
