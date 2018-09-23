using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponSwing : MonoBehaviour {

    private Rigidbody ThisRigidbody;

    [SerializeField]
    private MeleeNpcScript npcScript;
    [SerializeField]
    private AudioClip ImpactSound;
    private AudioSource ThisAudioSource;
    [SerializeField]
    private Transform ParentEnemyPosition;

    private bool hasDealtDamageOnThisSwing = false;

	// Use this for initialization
	void Awake () {
        ThisRigidbody = GetComponent<Rigidbody>();
        ThisAudioSource = GetComponent<AudioSource>();
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && npcScript.GetAttackState() && !hasDealtDamageOnThisSwing)
        {
            other.GetComponent<PlayerStatScript>().DealDamageToPlayer(npcScript.GetDamage(), ParentEnemyPosition);
            hasDealtDamageOnThisSwing = true;
            ThisAudioSource.PlayOneShot(ImpactSound, 0.5f);
        }
        else if (!npcScript.GetAttackState()){ 
            hasDealtDamageOnThisSwing = false;
        }
    }
}
