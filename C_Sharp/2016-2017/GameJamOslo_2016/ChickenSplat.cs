using UnityEngine;
using System.Collections;

public class ChickenSplat : MonoBehaviour 
{
	public float timer;

	public Sprite sprite1;
	public Sprite sprite2;

	public bool attackEnabled = false;

	public GameObject sirkelTING;

	private SpriteRenderer spriteRenderer;
	private Animator anim;

	public Animator animOfPlayer;
	AudioSource audio;
	private GameManager gameManager;

	public PlayerController playerController;

	public GameObject featherExplosion;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		if (spriteRenderer.sprite == null)
			spriteRenderer.sprite = sprite1;

		anim = GetComponent<Animator> ();	
		audio = GameObject.Find ("ChickenDEAD").GetComponent<AudioSource> ();
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			anim.enabled = false;
			sirkelTING.SetActive (false);
			spriteRenderer.sprite = sprite2;
			featherExplosion.SetActive (true);
			playerController.isEating = true;
			animOfPlayer.SetBool ("eating", true);
			audio.Play ();


		}

		if (gameManager.countdownInt > 0)
		// Score based on how much time you have left. Hurry up to get a higher score!
			gameManager.score += gameManager.baseScorePerChicken * gameManager.countdownInt;
		else
			gameManager.score += gameManager.baseScorePerChicken;
		if (gameManager.score > PlayerPrefs.GetInt("hiScore", 0))
		{
			PlayerPrefs.SetInt("hiScore", gameManager.score);
			PlayerPrefs.Save();
		}

	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			animOfPlayer.SetBool ("eating", false);
			playerController.isEating = false;
		}
	}
}
