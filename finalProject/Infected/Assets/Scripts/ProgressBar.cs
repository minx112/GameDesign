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
	//becoming insane 7:20 - 440 secs bpm 145
	void Start () {
		progress.value = 0;
		InvokeRepeating("Prog", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (counter >= 27 && counter <= 66) {//26
			Fill.color = Color.Lerp (red, blue, Mathf.PingPong (Time.time, 1.2083f));

		} else if (counter >= 67 && counter <= 105) { //66
			Fill.color = Color.Lerp (blue, yellow, Mathf.PingPong (Time.time, 1.2083f));

		}
		else if (counter >= 107 && counter <= 132) { //105
			Fill.color = red;
		}
		else if (counter >= 133 && counter <= 202) { //132
			Fill.color = Color.Lerp (red, cyan, Mathf.PingPong (Time.time, 1.2083f));

		}
		else if (counter >= 202 && counter <= 228) { //202
			Fill.color = red;

		}
		else if (counter >= 229 && counter <= 241) { 
			Fill.color = Color.Lerp (red, yellow, Mathf.PingPong (Time.time, 1.2083f));

		}
		else if (counter >= 241 && counter <= 332) { 
			Fill.color = Color.Lerp (yellow, magenta, Mathf.PingPong (Time.time, 1.2083f));

		}
		else if (counter >= 336 && counter <= 360) { 
				Fill.color = red;

		}
		else if (counter >= 360 && counter <= 414) { 
			Fill.color = Color.Lerp (red, blue, Mathf.PingPong (Time.time, 1.2083f));

		}
		 else if (counter >= 414) { 
				Fill.color = red;

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
