using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemySnowman : MonoBehaviour {

    [SerializeField]
    private TimelineClip Start;

    #region Update functions

    private void Awake()
    {
        jumpAttack = GetComponent<Snowman_JumpAttack>();
        twomping = GetComponent<Snowman_Twomping>();
        giantSnowball = GetComponent<Snowman_FinalAbility>();

        mRigidbody = GetComponent<Rigidbody>();
        if(GameObject.FindGameObjectWithTag("Player") != null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


        //Particles:
        JumpAttackLanding = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();
        JumpAttackLanding.Stop();

        mAnimations = transform.GetChild(0).GetComponent<Snowman_Animations>();

        mHealth.Health_RestoreHealthToMax();
    }



    private void FixedUpdate()
    {
        if(checkStateAndPhase)
            print(mState + " | " + mPhase + " | " + Time.realtimeSinceStartup);

        //Running the state machine of the snowman, this should only contain action functions. (can contain debugging functions)
        switch (mState)
        {
            case States.Idle:

                Actions_Idle();

                break;

            case States.Defeated:

                Actions_Defeated();

                break;

            case States.Reatreating:

                Actions_Reatreating();

                break;

            case States.JumpAttacking:

                Actions_JumpAttack();

                break;

            case States.Twomping:

                Actions_Twomping();

                break;

            case States.GiantSnowballs:

                Actions_GiantSnowballs();

                break;

            case States.Exhausted:

                Actions_Exhausted();

                break;

            default:

                break;
        }
    }

    #endregion

    #region States

    public enum States
    {
        Idle,
        Defeated,
        Exhausted,
        Reatreating,
        JumpAttacking,
        Twomping,
        GiantSnowballs
    }

    private States mState = States.Idle;

    public States GetSnowmanState() { return mState; }
    public void SetSnowManState(States newState) { mState = newState; }

    public void SetNewInstanceOfPhaseThree(float waitDuration)
    {
        StartCoroutine(waitToReleaseAbilities(waitDuration));
    }

    private IEnumerator waitToReleaseAbilities(float duration)
    {
        transform.LookAt(transform.position - Vector3.forward);
        mAnimations.PushBackPlayer();   
        mState = States.Idle;
        yield return new WaitForSeconds(duration);
        canStartSendingStuff = true;
        mAnimations.PlayerCanContinue(true);
        CheckPhase();
        SetPhase();
        mState = States.GiantSnowballs;
    }

    public enum Phase
    {
        one,
        two,
        three,
        dead
    }

    private Phase mPhase = Phase.one;

    public Phase GetPhase()
    {
        return mPhase;
    }


    #endregion

    #region Generic Information

    private Rigidbody mRigidbody = null;

    private Transform playerTransform = null;

    private GameObject particleParent = null;

    private Vector3 startLocation = Vector3.zero;


    #endregion

    #region Connected scripts

    private Snowman_JumpAttack jumpAttack = null;
    private Snowman_FinalAbility giantSnowball = null;
    private Snowman_Twomping twomping = null;
    private Snowman_Animations mAnimations = null;

    #endregion

    #region Actions

    #region Idle
    private float countdown = 2f;


    private bool startFight = false;

    private void Actions_Idle()
    {
        if (mPhase != Phase.three)
        {
            if (countdown <= 0f && !startFight)
            {
                startLocation = transform.position;
                startFight = true;
                CheckPhase();
            }
            else
                countdown -= Time.deltaTime;
        }   
    }

    #endregion

    #region Defeated
    private bool deathStarted = false;

    private void Actions_Defeated()
    {
        if (!deathStarted)
        {
            mAnimations.DeathScene();
            deathStarted = true;
        }
    }

    #endregion

    #region Exhausted

    [SerializeField]
    private float exhaustedDuration = 4f;

    private float timeBeingExhausted = 0f;

    private bool isInExhaustAnimation = false;

    private void Actions_Exhausted()
    {
        if (mPhase == Phase.one)
        {
            BeingExhausted();
        }
        else if (mPhase == Phase.two)
        {
            if (currentJump == amountOfJumps)
            {
                BeingExhausted();
                if (mAnimations.Get_JumpContinouslyCondition())
                    mAnimations.Animation_SetJumpContinuesly(false);
            }
            else
            {
                currentJump += 1;
                currentTimeUntilJump = 0f;
                currentTimeUntilReatreat = 0f;
                if (!mAnimations.Get_JumpContinouslyCondition())
                    mAnimations.Animation_SetJumpContinuesly(true);
                mState = States.JumpAttacking;
            }
        }
        else if (mPhase == Phase.three)
        {
            BeingExhausted();
        }
        
    }

    private void BeingExhausted()
    {
        if (timeBeingExhausted == 0f)
        {
            mAnimations.Animation_StartExhaust();
            isInExhaustAnimation = true;
            currentTimeUntilReatreat = 0f;
            tookMaxDamage = false;
            Vector3 lookRotation = playerTransform.position;
            lookRotation = new Vector3(lookRotation.x, transform.position.y, lookRotation.z);
            transform.LookAt(lookRotation);
        }

        if (timeBeingExhausted < exhaustedDuration && isInExhaustAnimation)
        {
            if (isInExhaustAnimation)
                timeBeingExhausted += Time.deltaTime;
            // Debug.Log("I am exhausted, remaining time: " + (exhaustedDuration - timeBeingExhausted) + " of " + exhaustedDuration);
        }
        else if (isInExhaustAnimation)
        {
            print("Got out of exhausted state");
            currentJump = 0;
            timeBeingExhausted = 0f;
            CheckPhase();
            mAnimations.Animation_StopExhaust();
            isInExhaustAnimation = false;

        }
    }

    #endregion

    #region Jump Attack

    private ParticleSystem JumpAttackLanding = null;

    private int amountOfJumps = 2;
    private int currentJump = 0;

    private float firstTimeWaitingJumpAttack = 1.5f;

    private float currentTimeUntilJump = 0f;

    private bool firstTimeJumpAttack = true;

    private void Actions_JumpAttack()
    {

        if (firstTimeJumpAttack)
        {
            if (currentTimeUntilJump >= firstTimeWaitingJumpAttack)
            {
                jumpAttack.InitializeAttack(playerTransform, mRigidbody, JumpAttackLanding);
                firstTimeJumpAttack = false;
            }
            else
            {
                currentTimeUntilJump += Time.fixedDeltaTime;
            }
        }
        else
        {

            if (currentTimeUntilJump >= waitForReatreat)
            {
                jumpAttack.InitializeAttack(playerTransform, mRigidbody, JumpAttackLanding);
            }
            else
            {
                currentTimeUntilJump += Time.fixedDeltaTime;
            }
        }

    }

    #endregion

    #region Twomping


    private void Actions_Twomping()
    {
        if (!twomping.CheckIfTwompingIsActive())
        {
            twomping.SetTwompingData();
        }
        twomping.TwompingSequence(mRigidbody, JumpAttackLanding);
    }

    #endregion

    #region Giant Snowball

    private float timeBetweenSnowballs = 1.25f;
    private float firstTimeWaitTime = 2f;
    private float currentTimeBetweenSnowballs = 0f;
    private int amountOfSnowballs = 5;
    private int currentAmountOfThrows = 0;
    private bool firstTimeSpikeSpawning = true;
    private bool canStartSendingStuff = false;

    private void Actions_GiantSnowballs()
    {
        print("Should throw snowballs");
        if (canStartSendingStuff)
        {
                if (currentTimeBetweenSnowballs >= timeBetweenSnowballs && currentAmountOfThrows < amountOfSnowballs)
                {
                    giantSnowball.SpawnNewSnowball();
                    mAnimations.Animation_SnowballSpawning();
                    currentTimeBetweenSnowballs = 0f;
                    currentAmountOfThrows += 1;
                }
                else if (currentAmountOfThrows >= amountOfSnowballs)
                {
                    currentAmountOfThrows = 0;
                    mState = States.Exhausted;
                }
                else
                {
                    currentTimeBetweenSnowballs += Time.fixedDeltaTime;
                }
        }
        
        
    }

    #endregion

    #region Retreating

    private float waitForReatreat = 0.5f;
    private float currentTimeUntilReatreat = 0f;

    private void Actions_Reatreating()
    {
        if (currentTimeUntilReatreat >= waitForReatreat)
        {
            jumpAttack.Reatreat(startLocation, mRigidbody, JumpAttackLanding);
        }
        else
        {
            currentTimeUntilReatreat += Time.fixedDeltaTime;
        }
    }

    #endregion

    #endregion

    #region Phases


    public void SetPhase()
    {
        int answer = 4 - mHealth.Health_GetHealth();
        print("Phase: " + answer);
        switch (mHealth.Health_GetHealth())
        {
            case 3:
                mAnimations.SetPhaseInAnimator(1);
                mPhase = Phase.one;
                break;

            case 2:
                mAnimations.SetPhaseInAnimator(2);
                mPhase = Phase.two;
                break;

            case 1:
                mAnimations.SetPhaseInAnimator(3);
                mPhase = Phase.three;
                break;

            case 0:
                mAnimations.SetPhaseInAnimator(0);
                mPhase = Phase.dead;
                break;

            default:
                break;

        }
    }

    public void CheckPhase()
    {
        if (startFight)
        {
            switch (mPhase)
            {
                case Phase.one:

                    mState = States.Twomping; 
                    break;

                case Phase.two:

                    mState = States.JumpAttacking;
                    break;

                case Phase.three:

                    mState = States.GiantSnowballs;
                    break;

                case Phase.dead:

                    mState = States.Defeated;
                    break;


                default:
                    Debug.Log("Couldn't find correct phase, setting to phase one.");
                    break;
            }
        }
    }

    #endregion

    #region Health

    [SerializeField]
    private HealthType mHealth = null;

    private int phaseHealth = 3;

    private bool tookMaxDamage = false;

    public void TakeDamage()
    {
        phaseHealth -= 1;
        print(mPhase + " - phase | " + phaseHealth + " - phaseHealth");
        mAnimations.Set_CurrentHealth(phaseHealth);
        mAnimations.Animation_TakeDamage();


        if (phaseHealth <= 0)
        {

            if (mPhase == Phase.one)
                mAnimations.StartPhaseChangeCutscene();
            else if (mPhase == Phase.two)
                mAnimations.StartLastPhaseCutscene();


            mHealth.TakeDamage(1);
            SetPhase();
            if (mPhase == Phase.dead)
                CheckPhase();
            else if (mPhase != Phase.three)
                mState = States.Idle;
            else
                mState = States.Idle;

            if (timeBeingExhausted != 0f)
                timeBeingExhausted = 0f;
            mAnimations.Set_CurrentHealth(phaseHealth);


            phaseHealth = 3;
        }
    }

#endregion


    [Header("Debugging")]
    private bool checkStateAndPhase = true;
}