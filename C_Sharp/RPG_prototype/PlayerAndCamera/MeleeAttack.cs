using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    private PlayerControllerTwo playerController;
    private PlayerStatScript playerStats;
    [SerializeField]
    private Transform PlayerTransform = null;

    private bool hasDealtDamageOnce = false;

    [SerializeField]
    private AudioClip impactHit;
    private AudioSource ThisAudioSource;

    private void Awake() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerTwo>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatScript>();
        ThisAudioSource = GetComponent<AudioSource>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other) {
        if ((other.tag == "Enemy") && playerController.GetAttackState() && !hasDealtDamageOnce)
        {
            ThisAudioSource.PlayOneShot(impactHit, 1f);
            other.GetComponent<MeleeNpcScript>().DealDamageToMeleeNPC(playerStats.GetAttackDamage());
            other.GetComponent<MeleeNpcScript>().SetImpactFromDamage(true, PlayerTransform.position, 2);
            hasDealtDamageOnce = true;
        }
        else if (other.tag == "Dummy" && playerController.GetAttackState() && !hasDealtDamageOnce) {
            ThisAudioSource.PlayOneShot(impactHit, 1f);
            other.GetComponent<DummyScript>().DealDamageToDummy(playerStats.GetAttackDamage());
        }
        else if (!playerController.GetAttackState())
        {
            hasDealtDamageOnce = false;
        }
    }
	
}
