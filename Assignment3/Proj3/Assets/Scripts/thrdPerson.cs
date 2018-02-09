using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrdPerson : MonoBehaviour {

	private bool thirdperson = false;
	private Camera cam;
	public float camdistz =0f;//-5
	public float camdisty = 0f; //1.5
	public float camrotx = 0f; // -10

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (thirdperson = false) {
			if (Input.GetKey ("shift")) {
				camdistz = -5f;
				camdisty = 1.5f;
				camrotx = -15;
				thirdperson = true;
				cam.transform.Translate (0, camdisty, camdistz);
			} else {
				
			}
		}
		if (thirdperson = true) {
			if (Input.GetKey ("shift")) {
				camdistz = 0f;
				camdisty = 0f;
				camrotx = 0f;				
				thirdperson = false;
				cam.transform.Rotate (camrotx, 0, 0);
			} else {

			}
		}

	}
}
