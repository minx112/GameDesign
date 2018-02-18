using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {

	private CharacterController con;
	private Camera cam;
	//public means type into editor
	public float speed;
	public float jumpSpeed;
	public float sensitivity;
	public float cameraHeight;
	public float gravity;
	public int doublej;

	private bool thirdperson = false;
	public float camdistz =0f;//-5
	public float camdisty = 0f; //1.5
	public float camrotx = 0f; // -10

	private Vector3 direction = Vector3.zero; // 3d vector (0,0,0) = new Vector3(0,0,0)

	// Use this for initialization
	void Start () {
		con = GetComponent<CharacterController> ();
		cam = GetComponentInChildren<Camera> ();
		cam.transform.localPosition = new Vector3 (0, cameraHeight, 0);
		cam.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up); // direction; 

	}
	//.forward

	// Update is called once per frame
	void Update () {
		//get input
		Vector3 moveInput = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
		
		//move player
		direction.x = moveInput.x;
		direction.z = moveInput.z;

		doublej = 0;

		if (con.isGrounded) {
			if (Input.GetKey ("space")) {
				direction.y = jumpSpeed;
				doublej++;
				if (Input.GetKey ("space") && doublej >= 1f) {
					direction.y += jumpSpeed;
				}
			} else {
				direction.y = 0;
			}
		}


		con.Move (direction * Time.deltaTime);
		direction.y -= gravity *Time.deltaTime;
		//look
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		transform.Rotate (0, mouseX * sensitivity * Time.deltaTime, 0);
		cam.transform.Rotate (mouseY * sensitivity * Time.deltaTime, 0, 0);

		if (Input.GetKey (KeyCode.Tab)) {
			if (!thirdperson) {
				Debug.Log ("3rd person mode pressed");
				camdistz = -5f;
				camdisty = 1.5f;
				camrotx = -15f;
				//cam.transform.Translate (0, camdisty, camdistz);
				cam.transform.Rotate (camrotx, 0f, 0f);
				cam.transform.localPosition = new Vector3 (0f, 1.5f, -5f);
				cam.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up); // direction;


			} else {
				Debug.Log ("3rd person out");
				camdistz = 0;
				camdisty = 0;
				camrotx = 0;				
				cam.transform.Rotate (0f, 0f, 0f);
				cam.transform.localPosition = new Vector3 (0f, cameraHeight, 0f);
				cam.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up); // direction;
			
			}

		}
		if (Input.GetKey (KeyCode.Tab)) {
			if (thirdperson) {
				thirdperson = false;
			} else {
				thirdperson = true;
			}
		}
	}
}