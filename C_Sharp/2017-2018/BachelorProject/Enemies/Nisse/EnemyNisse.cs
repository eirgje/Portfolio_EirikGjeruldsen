using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNisse : MonoBehaviour {

    #region Stances
    public enum TypeOfStance
    {
        Idle,
        Following,
        RePosition,
        Throwing,
        Retreating,
        Reloading,
        TakingDamage,
        Dying
    }

    public TypeOfStance mCurrentStance = TypeOfStance.Idle;

    #endregion


    #region Static-variables
    [SerializeField]
    private EnemyWithNavigation nisseVariables = null;

    private NavMeshAgent mNavMeshAgent = null;

    [SerializeField]
    private GameObject prefab_snowball = null;

    [SerializeField]
    private Transform mTargetToFollow = null;

    private AnimationsNisse mAnimations = null;

    private Transform throwJointTransform = null;

    #endregion


    #region Dynamic - variables

    private Vector3 mIdleTarget = Vector3.zero;

    private bool isDying = false;

    private Vector3 throwHitLocation = Vector3.zero;

    private Vector3 targetForwardWithInput = Vector3.zero;

    #endregion


    #region Searching for enemies

    private bool TargetInSight()
    {
        RaycastHit lineOfSightCheck;

        Vector3 direction = (mTargetToFollow.position - transform.position).normalized;
        Debug.DrawRay(transform.position + Vector3.up * 0.8f, direction * 100f, Color.yellow);



        if (Physics.Raycast(transform.position + Vector3.up * 0.8f, direction, out lineOfSightCheck, Mathf.Infinity))
        {
            if (lineOfSightCheck.collider.tag == "Player")
                return true;
            if (mTargetToFollow.tag != "Player")
                return true;
        }
        return false;
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

            if (distanceToTarget < nisseVariables.detectionRange && mCurrentStance == TypeOfStance.Idle)
            {
                if (TargetInSight())
                {
                    mCurrentStance = TypeOfStance.Following;
                }
            }
            else if (distanceToTarget < nisseVariables.combatRange && mCurrentStance == TypeOfStance.Following)
            {
                if (TargetInSight())
                    mCurrentStance = TypeOfStance.Throwing;
                else
                    mCurrentStance = TypeOfStance.Idle;

            }
        }
    }


    #endregion


    #region Combat


    private void SetLocationToThrowAt()
    {
        if (mTargetToFollow != null)
        {
            if (mTargetToFollow.tag == "Player")
            {
                if (mTargetToFollow.GetComponent<PlayerMovement>().GetState() == PlayerMovement.State.Locomotion && nisseVariables.useForwardPrediction)
                {
                    targetForwardWithInput = new Vector3(
                        mTargetToFollow.GetComponent<PlayerMovement>().GetVelocityCurrent().x,
                        0f,
                        mTargetToFollow.GetComponent<PlayerMovement>().GetVelocityCurrent().z);
                }
                else
                    targetForwardWithInput = Vector3.zero;
            }
            else
                targetForwardWithInput = Vector3.zero;


            throwHitLocation = mTargetToFollow.position + targetForwardWithInput * nisseVariables.forwardPredrictionSensetivity;

            throwHitLocation = new Vector3(throwHitLocation.x, mTargetToFollow.position.y + 1f, throwHitLocation.z);
        }
    }
    #endregion


    #region --Action initialization--

    private void WhatToDo()
    {
        if (!deathStart && mTargetToFollow != null)
        {
            if (mCurrentStance == TypeOfStance.Idle)
            {
                Actions_Idle();

                CheckRangeOfTarget();
            }
            else if (mCurrentStance == TypeOfStance.Following)
            {
                Actions_Following();

                CheckRangeOfTarget();
            }
            else if (mCurrentStance == TypeOfStance.RePosition)
            {
                Actions_RePosition();
            }
            else if (mCurrentStance == TypeOfStance.Throwing)
            {
                Actions_Throwing();
            }
            else if (mCurrentStance == TypeOfStance.Retreating)
            {
                Actions_Retreating();
            }
            else if (mCurrentStance == TypeOfStance.Reloading)
            {
                Actions_Reloading();
            }
            else if (mCurrentStance == TypeOfStance.TakingDamage)
            {
                Actions_TakingDamage();
            }
        }
        else if (!deathStart && mCurrentStance == TypeOfStance.Idle)
        {
            Actions_Idle();
        }

        DeathScaling();
    }
    #endregion


    #region Actions

    #region Idle
    private void Actions_Idle()
    {
        //Enveiroment behavior here, will become rather complex after a while.

        if (mNavMeshAgent.acceleration != nisseVariables.normalAcceleration)
            mNavMeshAgent.acceleration = nisseVariables.normalAcceleration;

        if (mNavMeshAgent.speed != nisseVariables.normalSpeed)
            mNavMeshAgent.speed = nisseVariables.normalSpeed;

        mNavMeshAgent.SetDestination(mIdleTarget);
    }

    #endregion

    #region Following
    private void Actions_Following()
    {
        mNavMeshAgent.SetDestination(mTargetToFollow.position);

        if (mNavMeshAgent.speed != nisseVariables.normalSpeed)
            mNavMeshAgent.speed = nisseVariables.normalSpeed;
        if (mNavMeshAgent.speed != nisseVariables.normalAcceleration)
            mNavMeshAgent.speed = nisseVariables.normalAcceleration;

        if (!mAnimations.Animations_GetMovementState()) mAnimations.Animations_SetMovementState(true);


        //Debugging if player ever LOS the target.
        if (!TargetInSight())
        {
            mCurrentStance = TypeOfStance.Idle;
        }
    }

    #endregion

    #region Re-Positioning

    private void Actions_RePosition()
    {
        if (!TargetInSight())
            mNavMeshAgent.SetDestination(mTargetToFollow.position);
        else
        {
            SetLocationToThrowAt();
            mCurrentStance = TypeOfStance.Throwing;
        }
    }
    #endregion

    #region Throwing

    private bool needToReload = false;

    private GameObject currentObjectToThrow = null;


    private void Actions_Throwing()
    {
        if (!needToReload)
        {
            SetLocationToThrowAt();
            mNavMeshAgent.SetDestination(transform.position);
            mNavMeshAgent.speed = 0f;
            GameObject Nisse_Snowball = Instantiate(prefab_snowball, transform.position + Vector3.right + Vector3.up, Quaternion.identity, null);
            currentObjectToThrow = Nisse_Snowball;

            currentObjectToThrow.GetComponent<Nisse_SnowballBehavior>().SetThrowJointTransformAndTarget(throwJointTransform, mTargetToFollow);

            mAnimations.Animation_StartAttack(currentObjectToThrow);

            needToReload = true;
        }

        transform.LookAt(new Vector3(mTargetToFollow.position.x, transform.position.y, mTargetToFollow.position.z));
    }

    public void DoneThrowing()
    {
        if (mTargetToFollow.tag == "Player")
            mCurrentStance = TypeOfStance.Retreating;
        else
        {
            needToReload = false;
            mCurrentStance = TypeOfStance.Idle;
        }

    }

    #endregion

    #region Retreating

    [Header("Retreat")]
    [SerializeField]
    private float durationOfRetreat = 3f;
    [SerializeField]
    private float retreatSpeed = 10f;
    [SerializeField]
    private float retreatAcceleration = 300f;

    private Vector3 escapingDirection = Vector3.zero;

    private void Actions_Retreating()
    {
        if (needToReload)
        {
            StartCoroutine(ReatreatDuration());
            mNavMeshAgent.speed = retreatSpeed;
            mNavMeshAgent.acceleration = retreatAcceleration;
            escapingDirection = (mTargetToFollow.position - transform.position).normalized;
            transform.LookAt(transform.position - (mTargetToFollow.position - transform.position).normalized);
            if (!mAnimations.Animations_GetMovementState()) mAnimations.Animations_SetMovementState(true);
            needToReload = false;
        }
        mNavMeshAgent.SetDestination(transform.position - escapingDirection);
    }

    private IEnumerator ReatreatDuration()
    {
        yield return new WaitForSeconds(durationOfRetreat);
        if (!deathStart)
        {
            mCurrentStance = TypeOfStance.Reloading;
        }

    }

    #endregion

    #region Reloading
    [Header("Reloading")]
    [SerializeField]
    private float reloadDuration = 1f;

    private bool reloadingStarted = false;

    private void Actions_Reloading()
    {
        if (!reloadingStarted)
        {
            StartCoroutine(Reloading());
            if (mAnimations.Animations_GetMovementState()) mAnimations.Animations_SetMovementState(false);
            mAnimations.Animations_Giggle();
            reloadingStarted = true;
        }

    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadDuration);
        if (!deathStart)
        {
            mCurrentStance = TypeOfStance.Following;
            reloadingStarted = false;
        }

    }

    #endregion

    #region Taking Damage
    [Header("Taking Damage")]
    [SerializeField]
    private float takingDamageAnimationTime = 1f;

    private bool tookDamage = false;

    private void Actions_TakingDamage()
    {
        if (tookDamage)
        {
            tookDamage = false;
            mAnimations.Animation_TakingDamage();
            mNavMeshAgent.SetDestination(transform.position);
            mNavMeshAgent.speed = 0f;
        }
    }
    public void TookDamage()
    {
        Destroy(currentObjectToThrow);
        mCurrentStance = TypeOfStance.TakingDamage;
        tookDamage = true;
    }

    public void AnimationEvent_DoneTakingDamage()
    {
        mCurrentStance = TypeOfStance.Retreating;
        needToReload = true;
    }

    #endregion

    #region Dying
    private bool deathStart = false;

    [SerializeField]
    private GameObject deathExplosion = null;

    public void Initialize_Death()
    {
        GetComponent<TargetBehavior>().IsTargetDead = true;
        mCurrentStance = TypeOfStance.Dying;
        deathStart = true;
        Destroy(gameObject, 1f);
        GameObject nisseDeathExplosion = Instantiate(deathExplosion, transform.position, Quaternion.identity, null);
        Destroy(nisseDeathExplosion, 2f);
        mAnimations.Animation_Death();
        Destroy(currentObjectToThrow);
        currentScale = transform.localScale.x;
    }

    float minScale = 0.9f;
    float maxScale = 1.1f;
    float currentScale = 0f;
    bool scaleUp = true;

    private void DeathScaling()
    {
        if(deathStart)
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

    #endregion


    #region Update-Functions


    void Awake () {

        mNavMeshAgent = GetComponent<NavMeshAgent>();

        if (mTargetToFollow == null && GameObject.FindGameObjectWithTag("Player") != null)
            mTargetToFollow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        mIdleTarget = transform.position;

        mAnimations = transform.GetChild(0).GetComponent<AnimationsNisse>();

        throwJointTransform = transform.GetChild(0).transform.GetChild(2).transform.GetChild(12).GetComponent<Transform>();
    }
	
	void Update () {
        WhatToDo();
        SetLocationToThrowAt();

	}
    #endregion

    #region Gizmos
    [Header("Gizmos")]
    [SerializeField]
    public bool ShouldGizmosBeDrawn = true;

    void OnDrawGizmos()
    {

        if (ShouldGizmosBeDrawn)
        {
            Gizmos.color = Color.yellow;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.2f);
            Gizmos.DrawSphere(transform.position, nisseVariables.detectionRange);

            Gizmos.color = Color.blue;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.2f);
            Gizmos.DrawSphere(transform.position, nisseVariables.combatRange);

            Gizmos.color = Color.green;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
            Gizmos.DrawWireSphere(transform.position, nisseVariables.attackingRange);
        }
    }

    #endregion
}
