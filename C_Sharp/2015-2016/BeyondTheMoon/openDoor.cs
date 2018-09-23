using UnityEngine;
using System.Collections;

public class openDoor : MonoBehaviour {

	public removeBlockSound rSound;
	public bool doorOpen = false;
	public bool camToDoor = false;

	public GameObject door;
	void FixedUpdate(){

		if (doorOpen == true) {
			Debug.Log ("door is open");
				rSound.doorGone = true;
				Destroy(this.gameObject);
			}
		}
	}
