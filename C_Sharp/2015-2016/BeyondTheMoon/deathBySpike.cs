using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class deathBySpike : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "Spike")
		{
			Application.LoadLevel(0);
		}
	}
}
