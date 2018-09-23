using UnityEngine;
using System.Collections;

public class hideHelp : MonoBehaviour {

	public GameObject helpText;

	private bool isHelpActive = true;
	
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.H) && isHelpActive == true) {
			helpText.SetActive (false);
			isHelpActive = false;
		} else if (Input.GetKeyDown (KeyCode.H) && isHelpActive == false) {
			helpText.SetActive (true);
			isHelpActive = true;
		}
	}
}
