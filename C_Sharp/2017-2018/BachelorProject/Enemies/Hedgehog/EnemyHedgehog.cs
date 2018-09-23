using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHedgehog : MonoBehaviour {

    #region Stances

    public enum TypeOfStances
    {
        Idle,
        Scared,
        Searching,
        Following,
        Screaming,
        Sprinting,
        Recharging,
        ChargingUp
    };

    /// <summary>
    /// Current active stance, this will tell you if you're idle, following a target or attacking.
    /// </summary>
    private TypeOfStances mCurrentStance = TypeOfStances.Idle;

    public TypeOfStances GetCurrentStance() { return mCurrentStance; }

    public void SetCurrentStance(TypeOfStances newStance) { mCurrentStance = newStance; }

    #endregion

    #region Data - constants
    [SerializeField]
    private EnemyWithNavigation hedgehogVariables = null;

    private NavMeshAgent mNavMeshAgent = null;

    private GameObject damagingAura = null;

    private GameObject rechargingParticleEffect = null;

    private HedgehogAnimations mAnimations = null;

    [Header("Data - constants")]

    [Range(1f, 2f)]
    [SerializeField]
    private float sprintingTargetMultiplier = 1.5f;

    #endregion

    #region Data - dynamic

    [Header("Data - dynamic")]
    [SerializeField]
    private Transform mTargetToFollow = null;

    [SerializeField]
    private Vector3 mIdleTarget = Vector3.zero;

    [Header("DEBUGING")]
    [SerializeField]
    private bool useDebuggingInConsole = false;
    [SerializeField]
    private Vector3 sprintLocationGoal = Vector3.zero;

    private Vector3 targetForwardWithInput = Vector3.zero;

    private bool isDying = false;

    #endregion

    #region Searching for enemies

    private Vector3 playerPosLastSpotted = Vector3.zero;

    private bool TargetInSight()
    {
        RaycastHit lineOfSightCheck;

        Vector3 direction = (mTargetToFollow.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * 100f, Color.yellow);



        if (Physics.Raycast(transform.position + Vector3.up * 2f, direction, out lineOfSightCheck, Mathf.Infinity))
        {
            if (lineOfSightCheck.collider.tag == "Player")
                return true;
        }
        return false;
    }

    private void SaveLatestPositionPlayerSpotted(Vector3 lastSpotted)
    {
        playerPosLastSpotted = lastSpotted;
    }

    public void SetTargetToFollow(Transform newTarget)
    {
        mTargetToFollow = newTarget;
    }

    public void SetIdleTarget(Vector3 position)
    {
        mIdleTarget = position;
        mNavMeshAgent.SetDestination(mIdleTarget);
    }


    private void CheckRangeOfTarget()
    {
        if (!isDying)
        {
            float distanceToTarget = (mTargetToFollow.position - transform.position).magnitude;

            if (distanceToTarget < hedgehogVariables.detectionRange && mCurrentStance == TypeOfStances.Idle)
            {
                if (TargetInSight())
                {
                    mCurrentStance = TypeOfStances.Following;
                }

            }
            else if (distanceToTarget < hedgehogVariables.combatRange && mCurrentStance == TypeOfStances.Following)
            {
                if (TargetInSight())
                    mCurrentStance = TypeOfStances.Screaming;
                else
                {
                    mCurrentStance = TypeOfStances.Idle;
                    mNavMeshAgent.SetDestination(playerPosLastSpotted);
                }


            }
            else if ((sprintLocationGoal - transform.position).magnitude < 3f && mCurrentStance == TypeOfStances.Sprinting)
            {
                mCurrentStance = TypeOfStances.Recharging;
            }

            if (mCurrentStance == TypeOfStances.Following && TargetInSight())
            {
                SaveLatestPositionPlayerSpotted(mTargetToFollow.position);
            }
        }


    }

    #endregion

    #region Movement - combat

    private void SetTargetToMoveTowards()
    {
        if (mTargetToFollow.GetComponent<PlayerMovement>().GetState() == PlayerMovement.State.Locomotion && hedgehogVariables.useForwardPrediction)
        {
            targetForwardWithInput = new Vector3(
                mTargetToFollow.GetComponent<PlayerMovement>().GetVelocityCurrent().x,
                0f,
                mTargetToFollow.GetComponent<PlayerMovement>().GetVelocityCurrent().z)
                * hedgehogVariables.forwardPredrictionSensetivity;
        }
        else
            targetForwardWithInput = Vector3.zero;

        sprintLocationGoal =
            (mTargetToFollow.position - transform.position).normalized * 10f
            + mTargetToFollow.position
            + targetForwardWithInput;

        sprintLocationGoal = new Vector3(sprintLocationGoal.x, mTargetToFollow.position.y, sprintLocationGoal.z);

        mNavMeshAgent.SetDestination(sprintLocationGoal);

    }
    #endregion

    #region ---Actions---

    private void WhatToDo()
    {

        //Checks the range of the target, then sets the correct stance accordingly.
        CheckRangeOfTarget();

        if (mCurrentStance == TypeOfStances.Idle)
        {
            Actions_Idle();
        }
        else if (mCurrentStance == TypeOfStances.Searching)
        {
            Actions_Searching();
        }
        else if (mCurrentStance == TypeOfStances.Scared)
        {
            Actions_Scared();
        }
        else if (mCurrentStance == TypeOfStances.Following)
        {
            Actions_Following();
        }
        else if (mCurrentStance == TypeOfStances.Screaming)
        {
            Actions_Screaming();
        }
        else if (mCurrentStance == TypeOfStances.Sprinting)
        {
            Actions_Sprinting();
        }
        else if (mCurrentStance == TypeOfStances.Recharging)
        {
            Actions_Recharging();
        }
        else if (mCurrentStance == TypeOfStances.ChargingUp)
        {
            Actions_ChargingUp();
        }

        if (isDying)
        {
            Dying();
        }


    }


    #endregion

    #region Idle

    private bool reachedIdleTarget = false;

    /// <summary>
    /// What happens during the idle state.
    /// </summary>
    private void Actions_Idle()
    {
        //Enveiroment behavior here, will become rather complex after a while.
        if (mNavMeshAgent.acceleration != hedgehogVariables.normalAcceleration)
            mNavMeshAgent.acceleration = hedgehogVariables.normalAcceleration;
        if (mNavMeshAgent.speed != hedgehogVariables.normalSpeed)
            mNavMeshAgent.speed = hedgehogVariables.normalSpeed;

        if (reachedIdleTarget)
        {
            RandomizedIdleMovement();
            reachedIdleTarget = false;
        }
        else if (mNavMeshAgent.remainingDistance < 0.5f)
            reachedIdleTarget = true;
    }

    private Vector3[] directions = new Vector3[4];
    int currentDirection = 0;
    private void RandomizedIdleMovement()
    {
        if (directions[3] != Vector3.right * -15f)
        {
            directions[0] = Vector3.forward * 15f;
            directions[1] = Vector3.forward * -15f;
            directions[2] = Vector3.right * 15f;
            directions[3] = Vector3.right * -15f;
        }


        mIdleTarget = directions[currentDirection];
        mIdleTarget += transform.position;
        currentDirection++;
        mNavMeshAgent.SetDestination(mIdleTarget);

        if (currentDirection == directions.Length)
            currentDirection = 0;
    }

    #endregion

    #region Scared

    bool gotScared = false;
    float timeUntilRunScared = 0.25f;
    private void Actions_Scared()
    {
        if (!gotScared)
        {
            mAnimations.Animation_GotHit();
            mNavMeshAgent.SetDestination(transform.position + (transform.forward * 30f));
            mNavMeshAgent.speed = 0f;
            timeUntilRunScared = 0.25f;
            gotScared = true;
        }
        else if (timeUntilRunScared > 0f)
        {
            timeUntilRunScared -= 1 * Time.deltaTime;
        }
        else
        {
            mNavMeshAgent.speed = hedgehogVariables.combatSpeed;
        }
    }

    public void AnimationEvent_SetStateBackToIdle()
    {
        mCurrentStance = TypeOfStances.Idle;
        gotScared = false;
        mNavMeshAgent.speed = hedgehogVariables.normalSpeed;   
    }
    #endregion

    #region Following

    /// <summary>
    /// What happens during the follow-state.
    /// </summary>
    private void Actions_Following()
    {
        mNavMeshAgent.SetDestination(mTargetToFollow.position);

        if(mNavMeshAgent.speed != hedgehogVariables.normalSpeed)
            mNavMeshAgent.speed = hedgehogVariables.normalSpeed;

        //Debugging if player ever LOS the target.
        if (!TargetInSight())
        {
            mCurrentStance = TypeOfStances.Idle;
        }
    }

    #endregion

    #region Searching

    private void Actions_Searching()
    {
        if(mNavMeshAgent.destination != playerPosLastSpotted)
            mNavMeshAgent.SetDestination(playerPosLastSpotted);
        if (mNavMeshAgent.remainingDistance < 0.5f)
        {
            if (TargetInSight())
            {
                mCurrentStance = TypeOfStances.Following;
            }
            else
            {
                mCurrentStance = TypeOfStances.Idle;
            }
        }
    }

    #endregion

    #region Screaming

    [Header("Scream")]
    [SerializeField]
    private float durationOfScream = 4f;

    private float savedDurationOfScream = 0f;

    private bool screamStarted = false;
    /// <summary>
    /// What happens during the screaming state.
    /// </summary>
    private void Actions_Screaming()
    {
        transform.LookAt(new Vector3(mTargetToFollow.position.x, transform.position.y, mTargetToFollow.position.z));
        if (!screamStarted && TargetInSight())
        {
            mAnimations.Animation_StartScream();
            mNavMeshAgent.SetDestination(transform.position);
            mNavMeshAgent.acceleration = 100f;
            mNavMeshAgent.speed = 0f;
            screamStarted = true;
        }        

        if (durationOfScream >= 0f)
        {
            durationOfScream -= Time.deltaTime;
            SaveLatestPositionPlayerSpotted(mTargetToFollow.position);
        }
        else
        {
            if (TargetInSight())
            {
                mAnimations.Animation_SetSprintState(true);
                screamStarted = false;
                damagingAura.SetActive(true);
                durationOfScream = savedDurationOfScream;
                SetTargetToMoveTowards(); //Setting target
                mCurrentStance = TypeOfStances.Sprinting;
            }
               
            else
            {
                mAnimations.Animation_SetSprintState(true);
                screamStarted = false;
                damagingAura.SetActive(true);
                durationOfScream = savedDurationOfScream;
                mNavMeshAgent.SetDestination(playerPosLastSpotted);
                mCurrentStance = TypeOfStances.Sprinting;
            }
        }


    }


    #endregion

    #region Sprinting

    private float durationOfSprint = 3.5f;

    /// <summary>
    /// What happens during the sprinting-state.
    /// </summary>
    private void Actions_Sprinting()
    {
        if (mNavMeshAgent.speed != hedgehogVariables.combatSpeed)
        {
            durationOfSprint = 3.5f;
            if (mNavMeshAgent.acceleration != hedgehogVariables.combatAcceleration)
                mNavMeshAgent.acceleration = hedgehogVariables.combatAcceleration;

            if (mNavMeshAgent.speed != hedgehogVariables.combatSpeed)
                mNavMeshAgent.speed = hedgehogVariables.combatSpeed;
        }

        float distance = (new Vector3(sprintLocationGoal.x, transform.position.y, sprintLocationGoal.z) - new Vector3(transform.position.x, transform.position.y, transform.position.z)).magnitude;

        if (distance < 0.1f)
        {
            mCurrentStance = TypeOfStances.Recharging;
        }
        else if (durationOfSprint > 0f)
        {
            durationOfSprint -= 1 * Time.deltaTime;
        }
        else if (durationOfSprint < 0f)
        {
            mAnimations.Animation_SetSprintState(false);
            mCurrentStance = TypeOfStances.Recharging;
        }


    }
    #endregion

    #region Recharging

    [Header("Recharging")]
    [SerializeField]
    private float durationOfRecharge = 2f;
    private float savedDurationOfRecharge = 2f;

    private void Actions_Recharging()
    {
        if (mNavMeshAgent.speed != 0f)
        {
            damagingAura.SetActive(false);
            rechargingParticleEffect.SetActive(true);
            mNavMeshAgent.acceleration = 100f;
            mNavMeshAgent.speed = 0f;
            mAnimations.Animation_Recharging();
        }


        if (durationOfRecharge >= 0f)
        {
            //transform.LookAt(new Vector3(mTargetToFollow.position.x, transform.position.y, mTargetToFollow.position.z));
            durationOfRecharge -= Time.deltaTime;
        }
        else
        {
            SetTargetToMoveTowards();
            rechargingParticleEffect.SetActive(false);
            durationOfRecharge = savedDurationOfRecharge;
            mCurrentStance = TypeOfStances.Screaming;
        }
    }


#endregion

    #region ChargingUp


    [Header("Charging Up")]
    [SerializeField]
    private float durationOfChargingUp = 1.3f;

    [SerializeField]
    private GameObject deathExplosion = null;

    /// <summary>
    /// What happens during the chargingUp-state.
    /// </summary>
    private void Actions_ChargingUp()
    {
        damagingAura.SetActive(false);
        rechargingParticleEffect.SetActive(false);
        if (!isDying) {
            mNavMeshAgent.speed = 0f;
            mAnimations.Animation_StartExplosion();
            GetComponent<TargetBehavior>().IsTargetDead = true;
            GameObject hedgehogDeathExplosion = Instantiate(deathExplosion, transform.position, Quaternion.identity, null);
            Destroy(gameObject, 1f);
            Destroy(hedgehogDeathExplosion, 2f);
            currentScale = transform.localScale.x;
            print("Charging Up");
            isDying = true;
        }
    }

    float minScale = 0.9f;
    float maxScale = 1.1f;
    float currentScale = 0f;
    bool scaleUp = true;

    private void Dying()
    {
        if (isDying)
        {

            if (scaleUp)
            {
                if (currentScale < maxScale)
                {
                    currentScale += Time.deltaTime;
                }
                else
                {
                    scaleUp = !scaleUp;
                }
            }
            else
            {
                if (currentScale > minScale)
                {
                    currentScale -= Time.deltaTime;
                }
                else
                {
                    scaleUp = !scaleUp;
                }
            }
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        }
    }



    #endregion


    #region Gizmos
    [Header("Gizmos")]
    [SerializeField]
    public bool ShouldGizmosBeDrawn = true;

    private void OnDrawGizmosSelected()
    {
        Ray forwardCrashingRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(forwardCrashingRay.origin, forwardCrashingRay.direction * 4f, Color.red);
    }
    void OnDrawGizmos()
    {

        if (ShouldGizmosBeDrawn)
        {
            Gizmos.color = Color.yellow;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.2f);
            Gizmos.DrawSphere(transform.position, hedgehogVariables.detectionRange);

            Gizmos.color = Color.blue;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.2f);
            Gizmos.DrawSphere(transform.position, hedgehogVariables.combatRange);

            Gizmos.color = Color.green;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawWireSphere(transform.position, hedgehogVariables.attackingRange);    
        }
    }

    #endregion

    #region MoreDebugging
    [Header("Debugging for direction of hedgehog")]
    [SerializeField]
    private LineRenderer debugingLine = null;
    [SerializeField]
    private bool useDebugingLinesForDirection = false;
    private void Debuging_drawLineForDirection()
    {
        debugingLine.SetPosition(0, transform.position);

        sprintLocationGoal = new Vector3(sprintLocationGoal.x, mTargetToFollow.position.y, sprintLocationGoal.z);

        Vector3 newPos =
            (mTargetToFollow.position - transform.position).normalized * 1.5f
            + mTargetToFollow.position +
            new Vector3(mTargetToFollow.GetComponent<PlayerMovement>().GetMovement().x,
            0f,
            mTargetToFollow.GetComponent<PlayerMovement>().GetMovement().z
            *hedgehogVariables.forwardPredrictionSensetivity);

        newPos = new Vector3(newPos.x, mTargetToFollow.position.y, newPos.z);

        debugingLine.SetPosition(1, newPos);
    }

#endregion

    #region Update functions

    private void Awake()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mAnimations = transform.GetChild(0).GetComponent<HedgehogAnimations>();
        damagingAura = transform.GetChild(1).gameObject;
        rechargingParticleEffect = transform.GetChild(2).gameObject;
        rechargingParticleEffect.SetActive(false);
        damagingAura.SetActive(false);
        savedDurationOfScream = durationOfScream;
        savedDurationOfRecharge = durationOfRecharge;

        if (mTargetToFollow == null)
            mTargetToFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        WhatToDo();

        if(useDebugingLinesForDirection)
            Debuging_drawLineForDirection();

        
    }

#endregion

}
