using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class menuScript : MonoBehaviour {


	public void startPress()
	{
		SceneManager.LoadScene (1);
	}

	public void exitPress()
	{
		Application.Quit ();
	}
}
