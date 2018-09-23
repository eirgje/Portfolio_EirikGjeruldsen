using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	//public variables
	public float speed = 0f;
	public float maxSpeed = 0f;
	public float jumpSpeed = 0f;
	public float rotationSpeed = 0f;
	public float rotationSensitivity = 0f;
	public float yRotation = 5f;
	public float lockOnSideRange = 5f;



	//public gameObjects/components
	public Transform target;
	public Vector3 cubePosition;
	public Vector3 playerPosition;
	public BoxInformation bInfo;


	//public bool variables 				//used to confirm actions from other objects
	public bool grounded = false;				//Jump-check
	public bool isAiming = false;
	public bool isShooting = false;


	//protected variables
	protected float h = 0f;					//Horizontal values
	protected float v = 0f;					//Vertical values
	protected float j = 0f;					//Jump values

	//private variables
	private Rigidbody rd;


	//TESTING VARIABLES
	public bool dashDo = false;
	public float dashSpeed = 100f;
	public float dashTimer = 0.1f;
	public float dashJumpValue = 0f;
	public bool dashJump = false;
	public bool timeForDash = true;
	public float timerForDash = 750f;


	public GameObject focusCamera;
	public GameObject mainCamera;



	// Use this for initialization
	void Start () {
	
		rd = GetComponent<Rigidbody> ();

	}


	// Update is called once per frame
	void Update () {
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
		j = Input.GetAxisRaw ("Jump");


		if (Input.GetKey (KeyCode.Mouse0) || Input.GetKey (KeyCode.JoystickButton4)) {

			isShooting = true;
			if (Input.GetKey (KeyCode.Mouse1) || Input.GetKey (KeyCode.JoystickButton5)) {
				bInfo.isScanning = true;
				if (bInfo.scanningTimer < 0.0001f || Input.GetKeyUp (KeyCode.Mouse1 )|| Input.GetKeyUp (KeyCode.JoystickButton5))
					bInfo.notScanning = true;
			}
			
			else {
				
				bInfo.notScanning = false;
				bInfo.isScanning = false; 

			}
		}
		else
			isShooting = false;


		if (Input.GetKeyDown (KeyCode.LeftShift) && timeForDash == true || Input.GetKeyDown (KeyCode.JoystickButton2) && timeForDash == true ) {
			timerForDash = 0;
			timeForDash = false;
			dashTimer = 15f;


		}
	}
		
	void FixedUpdate ()
	{
		Move ();
		Jump ();
		PerfectAim ();
		Rotation ();
		Dash ();
		DashReset ();
	}

	void Dash ()
	{
		if(dashTimer > 0.0001f && grounded == true)
			{
			rd.AddRelativeForce (Vector3.forward * dashSpeed);
			dashTimer--;
			if (j > 0.01f) {
				rd.velocity = new Vector3 (rd.velocity.x, dashJumpValue, rd.velocity.z);
				Debug.Log ("should be having massive jump");	
				if (Mathf.Abs (rd.velocity.y) > 0.01f) {
					dashTimer = 0f;
					grounded = false;
				}
			}
		}
	}

	void DashReset()
	{
		if (timerForDash == 150)
			timeForDash = true;
		if (dashTimer < 0.0001f && timeForDash == false)
			timerForDash++;
	}

	void Move ()
	{
		if (isShooting == false)
		{
			if (grounded == true)
				rd.AddRelativeForce ((Vector3.forward * speed) * v);
			//if (grounded == true)
			//	rd.AddRelativeForce ((Vector3.right * speed) * h);
		
			//By using a maxSpeed value, the addforce has less of an impact on maxSpeed, and can be used to create a better float in the physics
			if (rd.velocity.z > maxSpeed && grounded == true && dashDo == false)
				rd.velocity = new Vector3 (rd.velocity.x, rd.velocity.y, maxSpeed);
		
			if (rd.velocity.z < -maxSpeed && grounded == true && dashDo == false)
				rd.velocity = new Vector3 (rd.velocity.x, rd.velocity.y, -maxSpeed);

			if (rd.velocity.x > maxSpeed && grounded == true && dashDo == false)
				rd.velocity = new Vector3 (maxSpeed, rd.velocity.y, rd.velocity.z);

			if (rd.velocity.x < -maxSpeed && grounded == true && dashDo == false)
				rd.velocity = new Vector3 (-maxSpeed, rd.velocity.y, rd.velocity.z);
		
			if (Mathf.Abs (rd.velocity.z) > 0.001f && grounded == true && j == 0 || Mathf.Abs (rd.velocity.x) > 0.001f && grounded == true && j == 0) {
				rd.velocity = new Vector3 (rd.velocity.x / 99, rd.velocity.y, rd.velocity.z / 99);	
			} else if (Mathf.Abs (rd.velocity.z) < 0.001f && Mathf.Abs (rd.velocity.x) < 0.001f && grounded == true)
				rd.velocity = new Vector3 (0f, rd.velocity.y, 0f);
		}			
	}

	void PerfectAim ()
	{
		playerPosition = rd.position;

		if (cubePosition.z - playerPosition.z > 0.1f && isShooting == true) {
			mainCamera.SetActive (false);
			focusCamera.SetActive (true);


			if (cubePosition.x - playerPosition.x < -lockOnSideRange || playerPosition.x - cubePosition.x < lockOnSideRange) {
				Debug.Log ("Time to shoooooot!");
				isShooting = false;
				float yCord = target.position.x - transform.position.x;
				transform.eulerAngles = new Vector3 (0f, yCord, 0f);
				isAiming = true;
			}
		} else
			focusCamera.SetActive (false);
			mainCamera.SetActive (true);
			isAiming = false;
	}

	void Rotation ()
	{
		transform.Rotate ((Vector3.up * rotationSpeed) * h);
	}

	void Jump ()
	{
		if (grounded == true && j > 0 && dashTimer == 0)
		{
			rd.velocity = new Vector3 (rd.velocity.x, jumpSpeed * j, rd.velocity.z);
			Debug.Log ("Should be jumping");
			grounded = false;
		}
	}
}
