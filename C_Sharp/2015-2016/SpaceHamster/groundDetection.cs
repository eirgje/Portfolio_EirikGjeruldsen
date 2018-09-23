using UnityEngine;
using System.Collections;

public class groundDetection : MonoBehaviour {


	//Getting the script-refrence from the playerController-script
	public playerController pCon;						

	void OnTriggerEnter (Collider other)
	{
	//Detecting the ground, and can now send in values to confirm the collision
		if (other.gameObject.tag == "Ground")				
			pCon.grounded = true;
		else if (!(other.gameObject.tag == "Ground"))
			pCon.grounded = false;	
	}
		
}
