using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonEffects : MonoBehaviour {


	public Button StartGame;
	public Button HowToPlay;
	public Button Options;
	public Button Exit;


	public void startPress ()
	{
		SceneManager.LoadScene (1);
	}

	public void howToPress ()
	{
		SceneManager.LoadScene (3);
	}

	public void OptionPress ()
	{
		SceneManager.LoadScene (4);
	}

	public void ExitPress ()
	{
		SceneManager.LoadScene (5);
	}
}
