using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuDirections : MonoBehaviour {
	
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject creditsMenu;
	public GameObject quitMenu;
	public void startPush()
	{
		Application.LoadLevel (1);
	}

	public void optionsPush()
	{
			mainMenu.SetActive (false);
			optionsMenu.SetActive (true);
	}

	public void creditsPush()
	{
			mainMenu.SetActive (false);
			creditsMenu.SetActive (true);
	}

	public void quitPush()
	{
			mainMenu.SetActive (false);
			quitMenu.SetActive (true);
	}

	public void noPush()
	{
		mainMenu.SetActive (true);
		quitMenu.SetActive (false);
	}

	public void yesPush()
	{
		Application.Quit ();
	}


	public void backPush()
	{
		mainMenu.SetActive (true);
		optionsMenu.SetActive (false);
		creditsMenu.SetActive (false);
	}
}
