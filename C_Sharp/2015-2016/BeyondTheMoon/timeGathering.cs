using UnityEngine;
using System.Collections;

public class timeGathering : MonoBehaviour {

	public CharacterMovement player;

	public GameObject endText;

	public deathSound dSound;

	public bool timeAdd = false;
	public bool timeOut = false;


	void FixedUpdate ()
	{
		if (timeOut == true) {
			Destroy (gameObject);
		}
			
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "killZone") 
		{
			
			Debug.Log ("Should be stopping");
			Destroy(gameObject);

			float oneTurn = 1f;
			if (oneTurn == 1f) {
				dSound.isDead = true;
				oneTurn++;
			}
		}

		if (other.gameObject.tag == "timeCap")
		{
			Destroy(other.gameObject);
			timeAdd = true;
		}



	}
		
}
