using UnityEngine;
using System.Collections;

public class normalJump : MonoBehaviour {

	public CharacterMovement charMove;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Ground") {
			charMove.jumpRdy = true;
		} else {
			charMove.jumpRdy = false;
		}			

	}
}
