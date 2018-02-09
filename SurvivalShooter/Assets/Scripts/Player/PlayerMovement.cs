using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidBody;
	int floorMask;
	float camRayLength = 100f;

	void Awake() {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidBody = GetComponent<Rigidbody> ();

	}
	// automatically updates
	void FixedUpdate()
	{
		float h = Input.GetAxisRaw ("Horizontal"); // snap to raw speed
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);
		Turning ();
		Animating (h, v);

	}
	void Move(float h, float v) {
		movement.Set (h, 0f, v);
		//move diagnol normalize it
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidBody.MovePosition (transform.position + movement);
	}

	void Turning(){
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition); // sets camera to mouse
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidBody.MoveRotation (newRotation);

		}
	}

	void Animating(float h, float v) {
		bool walking = h != 0f || v != 0f;//did we press hor or vert axis
		anim.SetBool ("IsWalking", walking);
	}

}
