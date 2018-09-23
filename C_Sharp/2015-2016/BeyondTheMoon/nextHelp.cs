using UnityEngine;
using System.Collections;

public class nextHelp : MonoBehaviour {



	public GameObject firstSide;
	public GameObject secondSide;
	public GameObject thirdSide;

	public GameObject move;
	public GameObject jump;
	public GameObject next;

	public GameObject wallJ;
	public GameObject pickUps;
	public GameObject previous;
	public GameObject next2;

	public GameObject previous2;
	public GameObject enemies;


	public void nextHelpPress()
	{

		move.SetActive (false);
		jump.SetActive (false);
		next.SetActive (false);
		firstSide.SetActive (false);

		wallJ.SetActive (true);
		pickUps.SetActive (true);
		previous.SetActive (true);
		next2.SetActive (true);
		secondSide.SetActive (true);


	}

	public void previousHelpPress()
	{
		move.SetActive (true);
		jump.SetActive (true);
		next.SetActive (true);
		firstSide.SetActive (true);

		wallJ.SetActive (false);
		pickUps.SetActive (false);
		previous.SetActive (false);
		next2.SetActive (false);
		secondSide.SetActive (false);
	}


	public void nextHelpPress2()
	{

		previous.SetActive (false);
		pickUps.SetActive (false);
		wallJ.SetActive (false);
		next2.SetActive (false);
		secondSide.SetActive (false);

		thirdSide.SetActive (true);
		previous2.SetActive (true);
		enemies.SetActive (true);


	}


	public void previousHelpPress2()
	{
		previous.SetActive (true);
		pickUps.SetActive (true);
		wallJ.SetActive (true);
		next2.SetActive (true);
		secondSide.SetActive (true);

		thirdSide.SetActive (false);
		previous2.SetActive (false);
		enemies.SetActive (false);
	}

}
