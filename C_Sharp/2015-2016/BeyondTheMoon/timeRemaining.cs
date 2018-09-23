using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timeRemaining : MonoBehaviour {

	public timeCoundown timeC;

	private float endTime = 0f;

	private Text remainingTimeText;
	
	// Update is called once per frame
	void Update () {

		if (timeC.levelComplete == true)
			gameObject.SetActive (true);

		endTime = timeC.timeCount;
		//remainingTimeText = endTime.ToString ("f0");
	
	}
}
