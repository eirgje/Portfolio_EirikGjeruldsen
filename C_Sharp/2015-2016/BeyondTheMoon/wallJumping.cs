using UnityEngine;
using System.Collections;

public class wallJumping : MonoBehaviour {

	public CharacterMovement charMove;
	public timeCoundown finishText;

	void OnTriggerEnter2D(Collider2D other)				//leser når du treffer objekter med en type tag, skal informasjon sendes inn til characterMovement scriptet, eller end-text skriptet.
	{

		if (other.gameObject.tag == "Wall") {
			charMove.wallJump = true;
		}
						
		if (other.gameObject.tag == "finish")
			finishText.levelComplete = true;

		if (other.gameObject.tag == "Finish2")
			finishText.level2Complete = true;
	}
		
	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "Ground") {
			charMove.wallStop = true;
		} else
			charMove.wallStop = false;

		if (other.gameObject.tag == "Wall") 
		{
			charMove.wallJumpDraging = true;
		}

	}

}