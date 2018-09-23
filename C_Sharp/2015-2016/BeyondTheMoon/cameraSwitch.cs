using UnityEngine;
using System.Collections;

public class cameraSwitch : MonoBehaviour {

	public Camera cam;
	public Camera cam2;
	public Camera cam3;

	public openDoor door1;
	public openDoor door2;
	
	// Update is called once per frame
	void FixedUpdate () {


		if (door1.camToDoor == true) {
			cam.enabled = false;
			cam2.enabled = true;
		} else if (door1.camToDoor == false) {
			cam.enabled = true;
			cam2.enabled = false;
		}


		if (door2.camToDoor == true) {
			cam.enabled = false;
			cam3.enabled = true;
		} else if (door2.camToDoor == false) {
			cam.enabled = true;
			cam3.enabled = false;
		}


	
	}
}
