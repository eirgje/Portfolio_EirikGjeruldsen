using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public GameObject mainPage;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			{
				Time.timeScale = 0;
			mainPage.SetActive (true);
			}
	
	}


	public void ResumeGame()
	{
		Time.timeScale = 1;
		mainPage.SetActive (false);
	}
}
