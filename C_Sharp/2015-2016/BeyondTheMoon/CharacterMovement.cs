using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

		//public variables
		public float jumpSpeed = 0f;
		public float movingSpeed = 0f;
		public float maxSpeed = 3f;
		public float wallJumpDrag = 0f;

		public float wallVelocity = 0f;


		protected float velY; 
		
		private Rigidbody2D rd;

		//animation
		private Animator anim;


		//Jump
		public bool jumpRdy = false;
		public bool jumpPressed = false;

		//movement
		private float h = 0f;
		
		//wallJump
		public bool wallJumpDraging = false;
		public bool wallJump = false;
		public bool jumpFromWall = false;
		public float wallJumpSpeed = 0f;

		//Flip
		protected bool facingRight = true;
		private bool flipFix = false;
		public float flipSpeed = 0f;


		//bug fixes
		public bool wallStop = false;
		public float dragSpeedForWallBug = 0f;



		// Use this for initialization
		void Start () {
			rd =GetComponent<Rigidbody2D>();
			anim = GetComponent<Animator> ();
		}
		



		

		// Update is called once per frame
	void Update()                               //her hentes alle inputsene 
	{
		velY = rd.velocity.y;

		if (Input.GetKeyDown (KeyCode.Space) && wallJump == true) 
			jumpFromWall = true;

		if (Input.GetKeyDown (KeyCode.Space) && jumpRdy == true)
			jumpPressed = true;


		h = Input.GetAxis ("Horizontal");

	}
		
	void FixedUpdate()
	{
		Animator ();
		whenWallStop ();
		Jump ();
		Movement ();
		jumpWall ();



			
	}

	void Animator ()
	{

		if ((Mathf.Abs(h)) > 0.001) 
		{
			anim.SetFloat ("movingSide", Mathf.Abs(h));
		}
		else
		{
			anim.SetFloat ("movingSide", Mathf.Abs(h));
		}



		anim.SetBool ("grounded", jumpRdy);


		anim.SetFloat ("vDown", velY);

		anim.SetBool ("grounded", jumpRdy);
	}

	void Jump()
	{
		if (jumpRdy == true && jumpPressed == true) {
			
			rd.velocity = new Vector2 (rd.velocity.x, jumpSpeed);



			jumpRdy = false;
			jumpPressed = false;
		}

	}
		
	void Movement()
	{
		rd.AddForce (Vector2.right * h * movingSpeed);

		if (facingRight == true && h < -0.1)
		{
			Flip ();
		} else if (facingRight == false && h > 0.1)
		{
			Flip ();
		}
			
		if (rd.velocity.x > maxSpeed && jumpRdy == true)
		{
			rd.velocity = new Vector2 (maxSpeed, rd.velocity.y);

		}

		if (rd.velocity.x < -maxSpeed && jumpRdy == true) 
		{
			rd.velocity = new Vector2 (-maxSpeed, rd.velocity.y);
		}

		if (rd.velocity.x > maxSpeed && jumpRdy == false)
		{
			rd.velocity = new Vector2 (maxSpeed / 2, rd.velocity.y);

		}

		if (rd.velocity.x < -maxSpeed && jumpRdy == false) 
		{
			rd.velocity = new Vector2 (-maxSpeed / 2, rd.velocity.y);
		}
			
	}


		

		void jumpWall()
		{
			if (jumpFromWall == true && wallJump == true) {

				Debug.Log (rd.velocity.x);
				wallJump = false;
				jumpFromWall = false;


				if (rd.velocity.x < 0) {
				rd.velocity = new Vector2 (rd.velocity.x, wallVelocity);
					Debug.Log ("skal walljumpe riktig");
				}

				if (rd.velocity.x > 0){
				rd.velocity = new Vector2 (rd.velocity.x, wallVelocity);
					Debug.Log ("skal walljumpe korrekt");


				}
					
			}
		}

		void Flip()
		{
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

	void whenWallStop()
	{
		if (wallStop == true) {
			rd.velocity = new Vector2 (0f, dragSpeedForWallBug);
			Debug.Log ("Should be dragging");
			wallStop = false;
		}

	}
		
				

	}