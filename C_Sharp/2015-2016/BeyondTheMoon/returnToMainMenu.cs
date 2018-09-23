using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class returnToMainMenu : MonoBehaviour {
	

	public void returnToMainPress ()
	{
		SceneManager.LoadScene (0);
	}

	public void exitGamePress()
	{
		Application.Quit();
	}
}
