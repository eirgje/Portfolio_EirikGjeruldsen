using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_Twomping : MonoBehaviour {

    private enum currentAction
    {
        standing,
        jumping
    }

    private currentAction mAction = currentAction.standing;

    [SerializeField]
    private DamageValues twompingDamage = null;
    [SerializeField]
    private float damageZoneRadius = 5f;

    private Snowman_Animations mAnimations = null;

    [Header("Finding direction")]
    [SerializeField]
    private Vector3 startHalfwayPosition = new Vector3(42, 4, -30);
    [SerializeField]
    private Vector3 endHalfwayPosition = new Vector3(42, 4, 114);

    [Header("Leaps")]
    [SerializeField]
    private float leapSize = 10f;
    [SerializeField]
    private float jumpHeight = 3f;
    [Range(-100f, -1f)]
    [SerializeField]
    private float gravity = -40f;
    private bool usingGravity = false;


    [Header("Debugging")]
    public bool useDebugging = true;

    private void Awake()
    {
        mAnimations = transform.GetChild(0).GetComponent<Snowman_Animations>();
    }

    #region Calculate Jump

    private Vector3 CalculateJump(Vector3 target)
    {

        float savedHeight = jumpHeight;

        if ((target - transform.position).magnitude / 10f < 3f)
        {
            jumpHeight = jumpHeight * ((target - transform.position).magnitude / 20f);
        }


        float displacementY = target.y - transform.position.y;

        Vector3 displacementXZ = new Vector3(target.x - transform.position.x, 0f, target.z - transform.position.z);

        float time = Mathf.Sqrt(-2f * jumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - jumpHeight) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * jumpHeight);

        Vector3 velocityXZ = displacementXZ / time;

        jumpHeight = savedHeight;

        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }

    #endregion

    #region Damage

    private bool CheckIfPlayerGotHit()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageZoneRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Player")
            {
                print("PLAYER GOT HIT! WHAT A BAD PLAYER THIS WAS!");
                hitColliders[i].GetComponent<Transform>().GetChild(2).GetComponent<HealthPlayer>().Player_TakingDamage(
                    twompingDamage.damage,
                    twompingDamage.canKnockBack,
                    twompingDamage.knockBackPower * Vector3.ProjectOnPlane((hitColliders[i].transform.position - transform.position).normalized, Vector3.up)
                    );
                return true;
            }

        }

        return false;
    }

    #endregion

    #region Twomping

    private bool delayingCheck = true;

    private bool StartTwompingSequence = false;

    private int amountOfJumps = 0;
    private int currentJump = 0;

    private Vector3 endGoal = Vector3.zero;

    public bool CheckIfTwompingIsActive() { return StartTwompingSequence; }
    /// <summary>
    /// Starts the twomping by adding data to the correct places.
    /// </summary>
    public void SetTwompingData()
    {
        if (mAction == currentAction.standing)
        {
            amountOfJumps = FindAmountOfJumps(FindPointToGoTowards());
            endGoal = FindPointToGoTowards();
            transform.LookAt(new Vector3(endGoal.x, transform.position.y, endGoal.z));
            currentJump = 0;
            print("Got twomping data");
            StartTwompingSequence = true;
        }
    }


    /// <summary>
    /// This should be in an update function, and run every frame. Will check if it hits the ground, and then do next jump.
    /// </summary>
    /// <param name="bossRigidbody"></param>
    public void TwompingSequence(Rigidbody bossRigidbody, ParticleSystem JumpAttackLanding)
    {
        if (StartTwompingSequence)
        {
            if (usingGravity)
            {
                bossRigidbody.velocity += Vector3.up * gravity * Time.deltaTime;
            }


            Vector3 direction = Vector3.ProjectOnPlane((endGoal - transform.position), Vector3.up);
            direction = direction.normalized * leapSize;
            print(direction + " direction");

            if (mAction == currentAction.standing)
            {
                print("inside standing");
                if (currentJump <= amountOfJumps)
                {
                    if (!usingGravity)
                        usingGravity = true;
                    StartCoroutine(delayCheckForGround());
                    mAction = currentAction.jumping;
                    mAnimations.Animation_shortJump();      
                    currentJump++;
                    bossRigidbody.isKinematic = false;
                    bossRigidbody.velocity = CalculateJump(transform.position + direction);
                    print("CurrentJump: " + currentJump + "  OF  " + amountOfJumps);
                }
            }
            else if (mAction == currentAction.jumping && !delayingCheck)
            {

                RaycastHit groundHit;
                if (Physics.Raycast(transform.position + (Vector3.up * 0.05f), Vector3.down, out groundHit, 0.5f))
                {
                    if (groundHit.collider != null)
                    {
                        mAction = currentAction.standing;
                        usingGravity = false;
                        JumpAttackLanding.Stop();
                        JumpAttackLanding.Play();
                        print(CheckIfPlayerGotHit() + " - got hit");
                        if (currentJump >= amountOfJumps)
                        {
                            mAnimations.Animation_StopTwomping();
                            StartTwompingSequence = false;
                            GetComponent<EnemySnowman>().SetSnowManState(EnemySnowman.States.Reatreating);
                            print("Sequence ended!");
                        }
                            
                    }

                }

            }
        }
        else
        {
            bossRigidbody.velocity = Vector3.zero;
        }
    }

    private IEnumerator delayCheckForGround()
    {
        

        yield return new WaitForSeconds(0.15f);

        delayingCheck = false;

    }

    /// <summary>
    /// Returns the position furthest away from the boss.
    /// </summary>
    /// <returns></returns>
    private Vector3 FindPointToGoTowards()
    {
        if ((startHalfwayPosition - transform.position).magnitude < (endHalfwayPosition - transform.position).magnitude)
        {
            return endHalfwayPosition;
        }
        else
        {
            return startHalfwayPosition;
        }
    }

    /// <summary>
    /// This functions calculates the amount of jumps the boss will do
    /// </summary>
    /// <param name="targetPosition"></param>
    private int FindAmountOfJumps(Vector3 targetPosition)
    {
        float distanceToTarget = (targetPosition - transform.position).magnitude;

        int amountOfJumps = 0;

        while(distanceToTarget > leapSize)
        {
            distanceToTarget -= leapSize;
            amountOfJumps++;
        }

        return amountOfJumps;
    }

#endregion



    #region Gizmos
    private void OnDrawGizmos()
    {
        if (useDebugging)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(startHalfwayPosition, new Vector3(2, 2, 2));
            Gizmos.color = Color.green;
            Gizmos.DrawCube(endHalfwayPosition, new Vector3(2, 2, 2));
        }

    }
#endregion
}
