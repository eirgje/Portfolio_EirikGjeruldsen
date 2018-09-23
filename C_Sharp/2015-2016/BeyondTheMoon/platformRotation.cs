using UnityEngine;
using System.Collections;

public class platformRotation : MonoBehaviour {

	public float rotation = 0f;


	// Update is called once per frame
	void FixedUpdate () {

		transform.Rotate (transform.forward * rotation * Time.deltaTime);
	}
}
