using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	public float speed;
	Rigidbody rb;

	void Start()
		{
		rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (speed, 0, 0);
		}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (transform.position.x < 57)
			rb.velocity = new Vector3 (speed, 0, 0);

		if (transform.position.x > 63.5)
			rb.velocity = new Vector3 (-speed, 0, 0);
			
	}
}
