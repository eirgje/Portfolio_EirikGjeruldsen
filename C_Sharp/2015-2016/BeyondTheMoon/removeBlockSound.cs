using UnityEngine;
using System.Collections;

public class removeBlockSound : MonoBehaviour {
	
	public bool doorGone = false;

	private AudioSource audio;

	void Start ()
	{
		audio = GetComponent<AudioSource> ();
	}

	void Update () {

		if (doorGone == true) 
		{
			audio.Play ();

			doorGone = false;
		}
	}
}
		