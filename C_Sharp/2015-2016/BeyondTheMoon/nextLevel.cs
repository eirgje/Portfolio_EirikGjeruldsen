using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class backToStart : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.B))
			SceneManager.LoadScene (0);
	}
}
