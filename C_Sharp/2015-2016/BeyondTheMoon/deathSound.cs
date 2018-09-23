using UnityEngine;
using System.Collections;

public class deathSound : MonoBehaviour {

	public bool isDead = false;

	public Component audio;

	void Start()
	{
		AudioSource audio = GetComponent<AudioSource> ();
	}

	void Update () {
	
		if (isDead == true)
			GetComponent<AudioSource>().PlayDelayed(1);

	}
}
