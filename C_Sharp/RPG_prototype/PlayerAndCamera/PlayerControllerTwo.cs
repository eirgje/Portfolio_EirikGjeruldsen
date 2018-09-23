using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerTwo : MonoBehaviour {
	private float Horz;
	private float Vert;
	
	private CharacterController ThisController = null;
    
	private Vector3 Velocity = Vector3.zero;

	private bool JumpAttack = false;

    private Animator ThisAnimator;
    private Transform ThisTransform = null;

    [Header("Values")]
    [Header("---------------------")]
    [SerializeField]
    private float CameraDirectionSnapSpeed = 20f;
    [SerializeField]
    private float MaxSpeed = 10f;
    [SerializeField]
    private float SideWalkSpeed = 5f;
    [SerializeField]
    private float RotSpeed = 5f;
    [SerializeField]
    private float JumpForce = 50f;
    [SerializeField]
    private float GroundDist = 1.1f;
    [SerializeField]
    private float DoubleJumpGroundDist = 6.1f;
    [SerializeField]
    private bool IsGrounded = false;
    [SerializeField]
    private bool CanDoubleJump = false;
    [SerializeField]
    private bool DoubleJumpUsed = false;
    [Header("---------------------")]


    [Header("Sound-clips")]
    [Header("---------------------")]
    [SerializeField]
    private AudioClip WalkingSound;
    [SerializeField]
    private AudioClip RunningSound;
    [SerializeField]
    private AudioClip JumpingSound;
    [SerializeField]
    private AudioClip CastingSound;
    [SerializeField]
    private AudioClip SlashingSound;
    [SerializeField]
    private AudioClip DeadSound;
    [Header("---------------------")]

    
    [Header("All transforms")]
    [Header("---------------------")]
    [SerializeField]
    private Transform CameraPointTransform;
    [SerializeField]
    private Transform MeleeEffectPosition_right = null;
    [SerializeField]
    private Transform MeleeEffectPosition_left = null;
    [Header("---------------------")]

    [Header("Prefabs & Active-objects")]
    [Header("---------------------")]
    [SerializeField]
    private GameObject ProjectilePrefab = null;
    [SerializeField]
    private GameObject FireHandsPrefab = null;
    [Header("---------------------")]

    [Header("Other components")]
    [Header("---------------------")]
    [SerializeField]
    private Transform ProjectileParent;
    [SerializeField]
    private GameObject AimLine = null;
    [SerializeField]
    private LayerMask DefaultLayer;
    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField]
    private ButtonActivationScript Actionbar;

    [Header("---------------------")]


    //sound
    private bool CastingSoundActive = false;

    private AudioSource ThisAudioSource;


    //cast spell
    private GameObject CurrentSpell = null;
    private bool casting = false;
    
    //Running
    private PlayerStatScript playerStats;


    //Melee atttack
    private bool slashing = false;
    private bool slashingSoundActive = false;
    private bool canDealDamage = false;


    private bool stopCamera = false;
    private bool SprintReset = false;
    private bool hasDied = false;
    private bool DialogIsActive = false;

    [SerializeField]
    private GameObject mainCamera;

    //------------------------------------------------------------------------------------------------
    // Update-Functions
    //------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Awake () {
		ThisTransform = GetComponent<Transform> ();
		ThisController = GetComponent<CharacterController> ();
        ThisAnimator = GetComponent<Animator>();
        ThisAudioSource = GetComponent<AudioSource>();
        ThisAudioSource.clip = null;
        ThisAudioSource.volume = 0.1f;
        playerStats = GetComponent<PlayerStatScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!GetComponent<PlayerStatScript>().GameOver() && !DialogIsActive)
        {
		    Horz = CrossPlatformInputManager.GetAxis ("Horizontal");
		    Vert = CrossPlatformInputManager.GetAxis ("Vertical");

            CastSpell();
            AttackInstantiation();
            SoundEffects();
        }
	}
    void FixedUpdate()
    {
        if (!GetComponent<PlayerStatScript>().GameOver())
        {
            Movement();
            AnimationExecution();
        }
    }

    void LateUpdate() {
        if (playerStats.GameOver() && !hasDied)
        {
            print("dude");
            ThisAnimator.SetTrigger("Died");

            ThisAudioSource.volume = 1f;
            ThisAudioSource.loop = false;
            ThisAudioSource.PlayOneShot(DeadSound, 1f);

            hasDied = true;

        }
        else if (!playerStats.GameOver() && hasDied)
        {
            ThisAnimator.SetTrigger("Respawn");
            hasDied = false;
        }
    }

    //------------------------------------------------------------------------------------------------
    // Animations
    //------------------------------------------------------------------------------------------------

    void AnimationExecution() {
        if (!casting && !DialogIsActive)
        ThisAnimator.SetFloat("runSpeed", (Velocity.z));
        if (IsGrounded && CrossPlatformInputManager.GetButtonDown("Jump") && !casting) 
            ThisAnimator.SetTrigger("Jump");

        if (Mathf.Abs(Vert) < Mathf.Abs(Horz) && !casting)
        {
            ThisAnimator.SetFloat("strafeSpeed", Horz);
        }
        else {
            ThisAnimator.SetFloat("strafeSpeed", 0f);
        }
        
    }


    //------------------------------------------------------------------------------------------------
    // Soundeffects
    //------------------------------------------------------------------------------------------------

    void SoundEffects() {
        if (Mathf.Abs(Vert) > 0.1f && IsGrounded && ThisAudioSource.clip != WalkingSound && ThisAudioSource.clip != JumpingSound && !Input.GetKey(KeyCode.LeftShift))
        {
            ThisAudioSource.clip = WalkingSound;
            ThisAudioSource.loop = true;
            ThisAudioSource.Play();
            
        }
        else if (Mathf.Abs(Vert) > 0.1f && IsGrounded && ThisAudioSource.clip != RunningSound && ThisAudioSource.clip != JumpingSound && Input.GetKey(KeyCode.LeftShift))
        {
            ThisAudioSource.clip = RunningSound;
            ThisAudioSource.loop = true;
            ThisAudioSource.Play();

        }
        else if (Mathf.Abs(Vert) < 0.1f && ThisAudioSource.isPlaying && ThisAudioSource.clip != JumpingSound && ThisAudioSource.clip != CastingSound && ThisAudioSource.clip != SlashingSound 
            )
        {
            ThisAudioSource.clip = null;
            ThisAudioSource.loop = false;
            ThisAudioSource.Stop();

        }
        else if (CrossPlatformInputManager.GetButtonDown("Jump") && IsGrounded && !casting)
        {
            ThisAudioSource.volume = 0.2f;
            ThisAudioSource.clip = JumpingSound;
            ThisAudioSource.loop = false;
            ThisAudioSource.PlayOneShot(JumpingSound, 1f);
        }
        else if (casting && !CastingSoundActive){
            ThisAudioSource.volume = 0.4f;
            ThisAudioSource.clip = CastingSound;
            ThisAudioSource.loop = false;
            ThisAudioSource.PlayOneShot(CastingSound, 1f);
            CastingSoundActive = true;
        }
        else if (slashing && !slashingSoundActive)
        {
            ThisAudioSource.volume = 0.7f;
            ThisAudioSource.clip = SlashingSound;
            ThisAudioSource.PlayOneShot(SlashingSound, 1f);
            ThisAudioSource.loop = false;
            
            slashingSoundActive = true;
        }
        else if (ThisAudioSource.isPlaying == false)
        {
            ThisAudioSource.volume = 0.09f;
            ThisAudioSource.clip = null;
        }
    }


    //------------------------------------------------------------------------------------------------
    // Movement - jumping - distance to ground
    //------------------------------------------------------------------------------------------------
		
	void Movement(){
        if (!DialogIsActive)
        {
            //		print (DistanceToGround ());
            //		ThisTransform.rotation *= Quaternion.Euler (0f, RotSpeed * Horz * Time.deltaTime, 0f);
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                SprintReset = true;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && playerStats.CurrentEnergy() > 0 && Vert > 0.01f && SprintReset)
            {
                ThisAnimator.SetBool("isRunning", true);
                MaxSpeed = 10f;
                playerStats.DepleteEnergy();
            }
            else if (!(Input.GetKey(KeyCode.LeftShift)) || playerStats.CurrentEnergy() <= 0)
            {
                ThisAnimator.SetBool("isRunning", false);
                SprintReset = false;
                MaxSpeed = 6f;
                playerStats.RegainEnergy();
            }


            Velocity.z = Vert * MaxSpeed;
            Velocity.x = Horz * SideWalkSpeed;




            IsGrounded = (DistanceToGround() < GroundDist) ? true : false;
            CanDoubleJump = (DistanceToGround() > DoubleJumpGroundDist) ? true : false;

            if (CrossPlatformInputManager.GetButtonDown("Jump") && IsGrounded && !casting)
            {
                Velocity.y = JumpForce;
                DoubleJumpUsed = false;
            }

            if (CrossPlatformInputManager.GetButtonDown("Jump") && CanDoubleJump && !DoubleJumpUsed && !IsGrounded)
            {
                print("Done");
                Velocity.y = JumpForce;
                DoubleJumpUsed = true;
            }

            if(!IsGrounded)
                Velocity.y += Physics.gravity.y * Time.deltaTime * 3f;
            else{
                Velocity.y += Physics.gravity.y * Time.deltaTime;
            }


            //ThisTransform.position += ThisTransform.forward * MaxSpeed * Vert * Time.deltaTime;
            ThisController.Move(ThisTransform.TransformDirection(Velocity) * Time.deltaTime);

            //if (Velocity.z > 0.01f) {
            Quaternion ThisRotation = ThisTransform.rotation;
            Quaternion LookDirectionOfCamera = Quaternion.LookRotation(CameraPointTransform.position - ThisTransform.position, Vector3.up);
            if (Mathf.Abs(Velocity.z) > 0)
            ThisTransform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(ThisRotation.x, LookDirectionOfCamera.y, ThisRotation.z, LookDirectionOfCamera.w), CameraDirectionSnapSpeed * Time.deltaTime);
            //	}

            //		else if ()
        }
        else {
            ThisAnimator.SetBool("isRunning", false);
            ThisAnimator.SetFloat("runSpeed", 0f);
        }
	}


	public float DistanceToGround(){
		RaycastHit hit;
		float distanceToGround = 0;
        if (Physics.Raycast(ThisTransform.position, -Vector3.up, out hit, Mathf.Infinity, GroundLayer))
        {
            distanceToGround = hit.distance;
            //print(hit.distance);
        }
		return distanceToGround;
	}
    //------------------------------------------------------------------------------------------------
    // Spell - casting
    //------------------------------------------------------------------------------------------------
    private void CastSpell()
    {
        if (Input.GetButtonDown("Fire2") && !casting)
        {
            
            ThisAnimator.SetTrigger("Cast");
            StartCoroutine(SpellCooldown(1f));
        }
    }

    private IEnumerator SpellCooldown(float time)
    {
        AimLine.GetComponent<AimLineScript>().AreYouAiming(true);
        casting = true;
        Actionbar.ButtonTwoActive(true);
        GameObject CastBall = Instantiate(ProjectilePrefab, ProjectileParent.position, Quaternion.identity, ProjectileParent);

        CurrentSpell = CastBall;
        yield return new WaitForSeconds(time);
        casting = false;
        CastingSoundActive = false;
        Actionbar.ButtonTwoActive(false);
        AimLine.GetComponent<AimLineScript>().AreYouAiming(false);
    }

    public void SpellComplete()
    {
        CurrentSpell.GetComponent<ProjectileScript>().SetCastingBool(true);
        
        
    }

    //------------------------------------------------------------------------------------------------
    // Melee - attack
    //------------------------------------------------------------------------------------------------
    private void AttackInstantiation() {
        if (Input.GetButtonDown("Fire1") && !slashing && !casting) {
            ThisAnimator.SetTrigger("Attack");
            GameObject leftFlame = Instantiate(FireHandsPrefab, MeleeEffectPosition_left.position, Quaternion.identity, MeleeEffectPosition_left);
            GameObject rightFlame = Instantiate(FireHandsPrefab, MeleeEffectPosition_right.position, Quaternion.identity, MeleeEffectPosition_right);
            StartCoroutine(MeleeCooldown(0.75f));
        }
    }

    private IEnumerator MeleeCooldown(float time)
    {
        slashing = true;
        Actionbar.ButtonOneActive(true);
        canDealDamage = true;
        yield return new WaitForSeconds(time);
        slashing = false;
        canDealDamage = false;
        Actionbar.ButtonOneActive(false);
        slashingSoundActive = false;
    }


    public bool GetAttackState() { return canDealDamage; }

    public bool ReadSlashingState() {
        return slashing;
    }

    //------------------------------------------------------------------------------------------------
    // Spawning - block
    //------------------------------------------------------------------------------------------------

    public bool CameraStop() { return stopCamera; }
    public bool GetCastingStance() { return casting; }
    public void isDialogActive(bool isIt) { DialogIsActive = isIt; }

    //------------------------------------------------------------------------------------------------
    // 
    //------------------------------------------------------------------------------------------------
}
