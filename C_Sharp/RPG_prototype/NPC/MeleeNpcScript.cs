using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeNpcScript : GameManager {

    //Local-components
    private NavMeshAgent ThisNavMesh;
    private Animator ThisAnimator;
    private Transform ThisTransform;
    private AudioSource ThisAudioSource;
    private Rigidbody rb;

    [Header("---------------------")]
    [Header("Movement")]
    [Header("---------------------")]

    [Range(1, 20)]
    [SerializeField]
    private float MovementSpeed = 5f;

    private bool stopMovement = false; //for later use

    [Range(1, 20)]
    [SerializeField]
    private float RunSpeed = 5f;

    [Header("---------------------")]
    [Header("Tracking")]
    [Header("---------------------")]
    [SerializeField]
    private float DetectionRange = 75f;
    [SerializeField]
    private Transform PlayerTransform;
    private bool RoarActive = false;
    private PlayerStatScript playerStatsScript;
    [SerializeField]
    private Light FollowingYouLight;
    

    [Header("---------------------")]
    [Header("Attack")]
    [Header("---------------------")]
    [SerializeField]
    private float AttackRange = 1f;
    [Range(1, 100)]
    [SerializeField]
    private int StartDamage = 14;
    [Range(1, 100)]
    [SerializeField]
    private int StartHealth = 30;
    [SerializeField]
    private LayerMask PlayerLayer;
    private bool attackStarted = false;
    private bool canDealDamage = false;

    [Header("---------------------")]
    [Header("Sound")]
    [Header("---------------------")]
    [SerializeField]
    private AudioClip RoarSound;
    [SerializeField]
    private AudioClip IdleSound;
    [SerializeField]
    private AudioClip AttackSound;
    [SerializeField]
    private AudioClip RunSound;
    [SerializeField]
    private AudioClip StartingRunTowardsYouSound;
    [SerializeField]
    private AudioClip ImpactSound_Melee;
    private bool ImpactSoundStarted = false;
    private bool FollowYouSoundUsed = false;

    [Header("---------------------")]
    [Header("DamageTaken")]
    [Header("---------------------")]
    [SerializeField]
    private float DamageReduction = 0f;
    private bool impactKnockback = false;
    private Vector3 fromImpactDirection = Vector3.zero;
    private bool isDead = false;
    private bool deathStarted = false;
    [SerializeField]
    private GameObject DeathParticlePrefab;
    private bool destroySequenceActive = false;
    [SerializeField]
    private Transform particlePosition;
    private int attackType = 0;
    private bool restartNavMesh = false;
    private bool finalVictoryRoar = false;

    //Animation - states
    private bool roarStarted = false;
    private bool roarUsed = false;


    

	// Use this for initialization
	void Awake () {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ThisTransform = GetComponent<Transform>();

        ThisNavMesh = GetComponent<NavMeshAgent>();
        ThisAnimator = GetComponent<Animator>();
        
        ThisAudioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();

        Health = StartHealth;
        AttackDamage = StartDamage;

        playerStatsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Health <= 0)
            isDead = true;

        if (!isDead){
        DetectPlayer();
        Movement();
        Sounds();
        ImpactPushBack(fromImpactDirection, attackType );
        } else if (isDead) {
            Destroy(rb);
            ThisNavMesh.enabled = false;
            if (!deathStarted)
            {
                StartCoroutine(Death(3f));
            }
        }
	}

    private IEnumerator Death(float time)
    {
        ThisAnimator.SetTrigger("Died");
        deathStarted = true;
        yield return new WaitForSeconds(time);
        if (!destroySequenceActive)
        StartCoroutine(DeathParticle(0.5f));
    }
    private IEnumerator DeathParticle(float time) {
        GameObject deathParticle = Instantiate(DeathParticlePrefab, particlePosition.position, Quaternion.identity, null);
        destroySequenceActive = true;
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    private void Sounds() { 
    
    }

    private void AttackRotation() {
        if (attackStarted) {
            Quaternion newRot = Quaternion.LookRotation(PlayerTransform.position - ThisTransform.position);
            transform.rotation = newRot;
        }
    }


    private void Movement() {
        
        
            float currentDistance = (PlayerTransform.position - ThisTransform.position).magnitude;

            if (currentDistance > DetectionRange)
            {
                FollowingYouLight.intensity = 0f;
                if (FollowYouSoundUsed)
                FollowYouSoundUsed = false;
            }
            else if (roarUsed){
                if (!FollowYouSoundUsed) {
                    FollowingYouLight.intensity = 5f;
                    ThisAudioSource.PlayOneShot(StartingRunTowardsYouSound, 1f);
                    FollowYouSoundUsed = true;
                }
            }

            if (playerStatsScript.GameOver() && !finalVictoryRoar && currentDistance > AttackRange)
            {
                ThisNavMesh.destination = PlayerTransform.position;
                ThisAnimator.SetBool("gotTarget", true);
            }
            else if (playerStatsScript.GameOver() && !finalVictoryRoar && currentDistance < AttackRange*2)
            {
                ThisNavMesh.destination = ThisTransform.position;
                ThisAnimator.SetBool("gotTarget", false);
                if (!finalVictoryRoar)
                    StartCoroutine(VictoryCelebration(Random.Range(2, 5)));
                if (!RoarActive && !roarUsed)
                {
                    StartCoroutine(RoarFinish(2.15f));

                }

            }
            else if (currentDistance < AttackRange && !attackStarted && !playerStatsScript.GameOver() && !stopMovement)
            {
                if (!ThisNavMesh.isActiveAndEnabled)
                    ThisNavMesh.enabled = true;
                ThisNavMesh.speed = 0.01f;
                StartCoroutine(Attack(1.6125f));

            }
            else if (!stopMovement && currentDistance < DetectionRange && !playerStatsScript.GameOver())
            {
                if (!ThisNavMesh.isActiveAndEnabled)
                    ThisNavMesh.enabled = true;
                ThisNavMesh.destination = PlayerTransform.position;
                if (!attackStarted)
                    ThisNavMesh.speed = RunSpeed;
                ThisAnimator.SetBool("gotTarget", true);
            }
            else if (stopMovement && currentDistance < DetectionRange && currentDistance > DetectionRange && !playerStatsScript.GameOver() )
            {
                ThisAnimator.SetBool("gotTarget", false);
            }
            else if (!playerStatsScript.GameOver())
            {
                stopMovement = true;
                ThisAnimator.SetBool("gotTarget", false);
                ThisNavMesh.destination = ThisTransform.position;
            }

    }

    private void DetectPlayer()
    {
        Debug.DrawRay(ThisTransform.position, PlayerTransform.position - ThisTransform.position, Color.red);
        if ((PlayerTransform.position - ThisTransform.position).magnitude < DetectionRange)
        {
            if (!RoarActive && !roarUsed)
                StartCoroutine(RoarFinish(2.15f));
        }
        else if (roarUsed)
        {
            roarUsed = false;
        }

    }

    private IEnumerator Attack(float time)
    {
        attackStarted = true;
        ThisAnimator.SetTrigger("Attack");
        ThisAudioSource.PlayOneShot(AttackSound, 0.5f);        
        if (!canDealDamage)
        StartCoroutine(DealingDamage(0.6f));
        yield return new WaitForSeconds(time);
        attackStarted = false;
        stopMovement = false;
    }

    private IEnumerator DealingDamage(float time)
    {
        canDealDamage = true;
        yield return new WaitForSeconds(time);
        canDealDamage = false;
    }

    private IEnumerator VictoryCelebration(float time) {
        finalVictoryRoar = true;
        yield return new WaitForSeconds(time);
        roarUsed = false;
        finalVictoryRoar = false;
    }

    private IEnumerator RoarFinish(float timer) {
        RoarActive = true;
        stopMovement = true;
        ThisAnimator.SetTrigger("Roar");
        ThisAudioSource.Stop();

        ThisAudioSource.PlayOneShot(RoarSound, 0.75f);

        roarStarted = true;
        yield return new WaitForSeconds(timer);
        RoarActive = false;
        roarStarted = false;
        roarUsed = true;
        stopMovement = false;

    }

    private IEnumerator KnockBackAnimation() {
        stopMovement = true;
        if (Health > 0)
        ThisAnimator.SetTrigger("GetHit");
        yield return new WaitForSeconds(0.5f);
        stopMovement = false;
    }
    public int GetDamage() { return AttackDamage; }
    public bool GetAttackState() { return canDealDamage; }
    public int GetNpcHealth() { return Health;  }
    public void DealDamageToMeleeNPC(int damage) {
        Health -= damage;
    }
    public bool GetImpactFromDamage() { return impactKnockback; }
    public void SetImpactFromDamage(bool start, Vector3 fromObject, int stance)
    {
        attackType = stance;
        impactKnockback = start;
        fromImpactDirection = fromObject;
    }
    private void ImpactPushBack(Vector3 fromObject, int stance) {

        if (impactKnockback)
        {
            StartCoroutine(ImpactRoutine(0.1f, fromObject, stance));
        }
     
    }
    private IEnumerator ImpactRoutine(float time, Vector3 pos, int stance) {

        float force = 0f;
        if (stance == 1) { force = 100f; StartCoroutine(KnockBackAnimation()); }
        else if (stance == 2) { 
            force = 250f;
            if (!ImpactSoundStarted) {
                StartCoroutine(PlayImpactSound(0.5f));
                StartCoroutine(KnockBackAnimation());
            }
        }
        transform.LookAt(pos);
        rb.AddForce(-transform.forward * force * Time.deltaTime, ForceMode.Impulse);
        restartNavMesh = true;
        

        yield return new WaitForSeconds(time);
        rb.velocity = Vector3.zero;
        impactKnockback = false;
    }
    private IEnumerator PlayImpactSound(float time) {
        ImpactSoundStarted = true;
        ThisAudioSource.PlayOneShot(ImpactSound_Melee, 1f);
        yield return new WaitForSeconds(time);
        ImpactSoundStarted = false;
    }
}
