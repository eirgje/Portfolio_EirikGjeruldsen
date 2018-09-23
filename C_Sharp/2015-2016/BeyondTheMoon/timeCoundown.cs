using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class timeCoundown : MonoBehaviour {

	public float timeCount = 99f;
	public Text timerText;

	public openDoor oDoor;
	public openDoor oDoor2;

	public timeGathering timerDo;
	public GameObject endText;
	public GameObject quitText;

	public bool levelComplete = false;
	public bool level2Complete = false;

	private float counterTime = 0f;


	private bool completeLevel = false;

	void Start ()
	{
		timerText = GetComponent<Text> ();										//henter opp Text ved start.
	}

	void Update ()
	{																			//Lager en nedtellingsklokke som går på 50 frames per sekund. Teksten blir omgjort til en string, så den kan plukkes opp som et tekst format, men fortsatt mulighet til å endres.
		Debug.Log (timeCount);
			if (timeCount > 0) {
				timeCount -= Time.deltaTime;
				timerText.text = timeCount.ToString ("f0");
			if (timerDo.timeAdd == true && completeLevel == false) {
					int doLoop = 0;
					do {
						timeCount++;
						doLoop++;

					} while(doLoop < 15);
					counterTime++;
					timerDo.timeAdd = false;

				}

			} else {			//Sjekker om tiden er ute, og hvis den er så taper en og får TIME IS OUT som tekst.
				timerDo.timeOut = true;
				timerText.text = ("TIME IS OUT!");
			}

		
	}	

	void FixedUpdate ()
	{
		
		if (counterTime == 9 && levelComplete == true) { 	//Ser om level 1 er ferdig
			endText.SetActive (true);
			quitText.SetActive (false);
			completeLevel = true;



		}
	

		if (level2Complete == true) 			//Ser om level 2 er ferdig
		{
			endText.SetActive (true);
			quitText.SetActive (false);
			completeLevel = true;
		}
			

		if (counterTime == 4)			//Åpner neste del av level 1, ved å fjerne en stein som blokkerer veien.
			oDoor.doorOpen = true;

		if (counterTime == 9)			//Åpner siste del av level 1, ved å fjerne en stein som blokkerer veien.
			oDoor2.doorOpen = true;
		
	}


		
}
