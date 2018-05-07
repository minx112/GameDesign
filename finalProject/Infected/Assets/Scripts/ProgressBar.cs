using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour {

	public Slider progress;
	Stopwatch watch;
	private long time;
	private int counter = 0;
	// Use this for initialization
	//becoming insane 7:20 - 440 secs
	//1.000   440
	void Start () {
		progress.value = 0;
		watch = new Stopwatch ();
		time = 0;
		InvokeRepeating("Prog", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
		/*time = watch.ElapsedMilliseconds;
		if (watch.ElapsedMilliseconds % 1000 == 0) {
			//Debug.Log (time);
			progress.value += .07f;
		}*/
	
	}
	void Prog() {
		progress.value += .0022727f;
		counter++;
		if (counter >= 440) {
			Application.LoadLevel(SceneManager.GetActiveScene ().buildIndex +1);

		}
	}
}
