using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AttackParticle : MonoBehaviour {


	private Animator Anim;

	// Use this for initialization
	void Awake () {
		Anim = GetComponent<Animator> ();
	}	
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
		//	AttackActive = true;
			Anim.SetBool ("Attack", true);
		}
	}

	public void StopAttack(){
		Anim.SetBool ("Attack", false);
	}

	void FixedUpdate(){
//		FrontAttack ();
//		JumpAttack ();	

	}


//	private void FrontAttack(){
//		if (CurrentAttack == AttackType.FrontCone) {
////			if (number != 16)
////				number = 16;
////			if (AttackActive) {
////				ThisTrail.time = 1f;
////				OuterTrail.time = 1f;
////				CurrentNumber++;
////				if (CurrentNumber < number - 1) {
////					ThisTransform.localPosition = new Vector3 (((Mathf.Cos ((Mathf.PI * CurrentNumber) / 12f)) * Mathf.Deg2Rad) * 250f, 0f, (Mathf.Sin ((Mathf.PI * CurrentNumber) / 12f) * Mathf.Deg2Rad) * 250f);
////
////				} else {
////					AttackActive = false;
////					CurrentNumber = 0f;
////				}
////			} else if (!AttackActive) {
////				ThisTransform.localPosition = new Vector3 (((Mathf.Cos ((Mathf.PI * CurrentNumber) / 12f)) * Mathf.Deg2Rad) * 250f, 0f, (Mathf.Sin ((Mathf.PI * CurrentNumber) / 12f) * Mathf.Deg2Rad) * 250f);
////				ThisTrail.time = 0.01f;
////				OuterTrail.time = 0.01f;
////			}
//		}
//	}
//	private void JumpAttack(){
//		if (CurrentAttack == AttackType.AirToGround) {
//			if (number != 32)
//				number = 32;
//
//			if (AttackActive) {
//				ThisTrail.time = 5f;
//				float FasterCircle = CurrentNumber * 5f;
//				if (FasterCircle < number - 1) {
//					ThisTransform.localPosition = new Vector3 (((Mathf.Cos ((Mathf.PI * FasterCircle) / 12f)) * Mathf.Deg2Rad) * 500f, 0f, (Mathf.Sin ((Mathf.PI * FasterCircle) / 12f) * Mathf.Deg2Rad) * 500f);
//					CurrentNumber++;
//
//				} else {
//					AttackActive = false;
//					CurrentNumber = 0f;
//				}
//			} else if (!AttackActive) {
//				ThisTransform.localPosition = new Vector3 (((Mathf.Cos ((Mathf.PI * CurrentNumber) / 12f)) * Mathf.Deg2Rad) * 500f, 0f, (Mathf.Sin ((Mathf.PI * CurrentNumber) / 12f) * Mathf.Deg2Rad) * 500f);
//				ThisTrail.time = 0.1f;
//			}
//		}
//	}
	
}

