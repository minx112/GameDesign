using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour {

	public Slider progress;
	Stopwatch watch;
	private long time;
	// Use this for initialization
	//becoming insane 7:20 - 440 secs
	//1.000   440
	void Start () {
		progress.value = 0;
		watch = new Stopwatch ();
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		time = watch.ElapsedMilliseconds;
		if (time % 1000 == 0) {
			progress.value += .07f;
		}
	
	}
}
