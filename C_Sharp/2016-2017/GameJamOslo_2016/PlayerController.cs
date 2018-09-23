using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private Rigidbody2D rb;
	private GameManager gameManager;
	float h;
	float v;
	[Header("Player Attributes")]
	public float movementSpeed;
	private bool isHiding;
	private bool isTrapped;
	[Space(20)]
	public GameObject winScreen;
	public Animator anim;
	public bool isEating = false;
	public GameObject hiddenStatus;

	public GameObject screenForGameOver;

	bool roundTwoReady = false;
	bool roundThreeReady = false;


	bool isSafe = false;

	bool failedGame = false;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
			gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

	}
		
	void Update () 
	{
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis ("Vertical");

		if (Input.GetKeyDown (KeyCode.R) && failedGame == true) {
			gameOver (false);
			gameManager.countdownFloat = gameManager.time;
			gameManager.countdownInt = (int)gameManager.time;
			gameManager.score = 0;
			SceneManager.LoadScene (2);
		}

		if (Input.GetKeyDown (KeyCode.T) && roundTwoReady){
			SceneManager.LoadScene (5);
			winScreen.SetActive (false);

			if (Input.GetKeyDown (KeyCode.T) && roundThreeReady){
				SceneManager.LoadScene (8);
				winScreen.SetActive (false);
	}
	}
	}

	void FixedUpdate()
	{
		hiddenCheck ();
		movementBehavior ();
		rotationOfTheAvatar ();
	}

	void hiddenCheck()
	{
		if (isSafe == true) {
		
			hiddenStatus.SetActive (true);
		} 
		else if (!isSafe)
			hiddenStatus.SetActive (false);
	}

	void movementBehavior()
	{

		Vector2 input = new Vector2 (h, v) * movementSpeed * Time.fixedDeltaTime;
		if ( Mathf.Abs(h) > 0.01f && Mathf.Abs(v) > 0.01f)
			input /= Mathf.Sqrt (2);
		
			transform.position += new Vector3 (input.x, input.y, 0f);

		if ((Mathf.Abs (h) > 0.01f || Mathf.Abs (v) > 0.01f) && isEating == false)
			anim.SetBool ("isRunning", true);
		else if (Mathf.Abs (h) < 0.01f || Mathf.Abs (v) < 0.01f)
			anim.SetBool ("isRunning", false);
		}

	void rotationOfTheAvatar()
	{
		float centerPoint = 0f;
		Vector3 theScale = transform.localScale;
		if (h > 0.01f && transform.localScale.x >= -0)
		{
			theScale.x *= -1;
			transform.localScale = theScale;
			transform.position += new Vector3 (2,0,0);
		}
		if (h < -0.01f && transform.localScale.x <= 0) {
			theScale.x *= -1;
			transform.localScale = theScale;
			transform.position += new Vector3 (-2, 0, 0);
		}
	}
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "ChickenHouse") {
			SceneManager.LoadScene (3);
		}
		if (coll.gameObject.tag == "ChickenHousePartTwo")
			SceneManager.LoadScene (6);
			

		if (coll.gameObject.tag == "Outside") {
			SceneManager.LoadScene (4);
		}

		if (coll.gameObject.tag == "OutsidePartTwo")
			SceneManager.LoadScene (7);

		if (coll.gameObject.tag == "EscapeZone") {
			winScreen.SetActive (true);
			roundTwoReady = true;

		}

		if (coll.gameObject.tag == "EscapeZoneTwo") {
			winScreen.SetActive (true);
			roundThreeReady = true;

		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.transform.name == "Light" && isSafe == false) {
			gameManager.isPlaying = false;
			print ("You lose.");
			failedGame = true;
			gameOver (true);
		} else if (coll.transform.tag == "Enemy" && isSafe == false) {
			gameManager.isPlaying = false;
			print ("You lose.");
			failedGame = true;
			gameOver (true);
		}
		if (coll.gameObject.tag == "Enemy" && isSafe == false) {
			gameManager.isPlaying = false;
			print ("You lose.");
			failedGame = true;
			gameOver (true);
		}



	}
	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "SafeZone") {
			isSafe = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "SafeZone") {
			isSafe = false;
		}
	}

	void gameOver(bool notReset)
	{
		if (notReset) {
			screenForGameOver.SetActive (true);
			Time.timeScale = 0.001f;
		} else {
			screenForGameOver.SetActive (false);
			Time.timeScale = 1f;
		}
	}
}
