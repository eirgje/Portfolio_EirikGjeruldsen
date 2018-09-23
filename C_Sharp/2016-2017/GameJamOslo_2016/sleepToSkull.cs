using UnityEngine;
using System.Collections;

public class sleepToSkull : MonoBehaviour {


	public bool chickenIsDead = false;
	private SpriteRenderer sleepSprite;
	
	public Sprite deathSpriteSHITMANFUCK; 	

	// Use this for initialization
	void Start () {
		sleepSprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (chickenIsDead == true) {
			sleepSprite.sprite = deathSpriteSHITMANFUCK;
			Vector3 scaleIncrease = new Vector3 (5, 5, 0);
			if (transform.localScale.x < scaleIncrease.x)
			transform.localScale += scaleIncrease;
		}
	}
}
