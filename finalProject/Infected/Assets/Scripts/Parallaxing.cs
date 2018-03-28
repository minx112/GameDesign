using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds;     //array of all background objects
    private float[] parallaxScales;     //proportion of the camera's movement to move backgrounds by
    public float smoothing;        //how smooth the parallax will be

    private Transform cam;              //reference to main camera transform
    private Vector3 previousCamPos;     //camera pos in previous frame

    // Use for references
    private void Awake()
    {
        //set up camera reference
        cam = Camera.main.transform;
    }
    // Use this for initialization
    void Start ()
    {
        previousCamPos = cam.position;

        //assign cooresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //parallax is opposite of the camera movement
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create a target position which is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current position and target position with lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        //set previousCamPos to camera's new position at the end of the frame
        previousCamPos = cam.position;

    }
}
