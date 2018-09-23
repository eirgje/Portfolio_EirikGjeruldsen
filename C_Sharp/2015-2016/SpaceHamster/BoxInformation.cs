using UnityEngine;
using System.Collections;

public class BoxInformation : MonoBehaviour {


	public playerController pCont;
	public float boxMovementSpeed = 0f;
	Rigidbody rd;
	public Vector3 position;
	public Vector3 scale;

	public bool isScanning = false;
	public bool notScanning = false;
	public float scaleScale = 0f;
	public float scanningTimer = 30f;


	private Vector3 theScale;


	void Start ()
	{
		rd = GetComponent<Rigidbody> ();
		theScale = transform.localScale;
	}
	void FixedUpdate()
	{
		if (rd.position.x <= 70)
		rd.velocity = new Vector3 (boxMovementSpeed, 0f, 0f);
		if (rd.position.x >= 100)
			rd.velocity = new Vector3 (-boxMovementSpeed, 0f, 0f);

		position = rd.position;
		pCont.cubePosition = position;

		if (isScanning == true && scanningTimer > 0.0001f) {
			transform.localScale += new Vector3 (scaleScale, scaleScale, scaleScale);
			scanningTimer--;
		}

		if (notScanning == true) {
			transform.localScale = theScale;
			scanningTimer = 100f;
		}
		




	}
}
