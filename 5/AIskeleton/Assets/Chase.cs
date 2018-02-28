using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {

	public Transform player;
	public Transform head;
	static Animator anim;
	bool pursue = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = player.position - this.transform.position;
		direction.y = 0;

		//float angle = Vector3.Angle (direction, this.transform.forward);//line of sight
		float angle = Vector3.Angle (direction, head.up);//line of sight


		if (Vector3.Distance (player.position, this.transform.position) < 10 && (angle < 30 || pursue)) {

			pursue = true;//head

			this.transform.rotation = Quaternion.Slerp (this.transform.rotation,
				Quaternion.LookRotation (direction), 0.1f);//slerp rotates 
			
			anim.SetBool ("isIdle", false);
			if (direction.magnitude > 3) {
				this.transform.Translate (0, 0, 0.05f);
				anim.SetBool ("isWalking", true);
				anim.SetBool ("isAttacking", false);
			} else {
				anim.SetBool ("isAttacking", true);
				anim.SetBool ("isWalking", false);
			}
		}
		else {
			anim.SetBool ("isIdle", true);
			anim.SetBool ("isWalking", false);
			anim.SetBool ("isAttacking", false);
			pursue = false; // head
		}
	}
}
