using UnityEngine;
using System.Collections;

public class camerTest : MonoBehaviour {


	public Transform target;
	public float rotationSpeed = 0f;
	protected float h = 0;
	// Update is called once per frame
	void Update () {
	
		h = Input.GetAxisRaw ("Horizontal");
	}

	void FixedUpdate()
	{
		Rotation ();
	}

	void Rotation ()
	{
		


		Vector3 relativePos = (target.position + new Vector3 (0f, rotationSpeed * h, 0f) - transform.position);
		Quaternion rotation = Quaternion.LookRotation (relativePos);

		Quaternion current = transform.localRotation;

		transform.localRotation = Quaternion.Slerp (current, rotation, Time.deltaTime);
		transform.Translate (0, 0, 0 * Time.deltaTime);
	}
}