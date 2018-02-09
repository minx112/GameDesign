using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    private CharacterController con;
    private Camera cam;

    public float speed;
    public float jumpSpeed;
    public float sensitivity;
    public float cameraHeight;
    public float gravity;

    private Vector3 direction = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        con = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        cam.transform.localPosition = new Vector3(0, cameraHeight, 0);
        cam.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //get input
        Vector3 moveInput = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        //move player
        direction.x = moveInput.x;
        direction.z = moveInput.z;

        if(con.isGrounded)
        {
            if (Input.GetKey("space"))
            {
                direction.y = jumpSpeed;
            }
            else
            {
                direction.y = 0;
            }
        }

        con.Move(direction * Time.deltaTime);
        direction.y -= gravity * Time.deltaTime;

        //look
        transform.Rotate(0, mouseX * sensitivity * Time.deltaTime, 0);
        cam.transform.Rotate(mouseY * sensitivity * Time.deltaTime, 0, 0);
    }
}
