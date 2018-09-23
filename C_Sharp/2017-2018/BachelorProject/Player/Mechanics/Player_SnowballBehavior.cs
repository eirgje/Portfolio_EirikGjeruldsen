using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player_SnowballBehavior : MonoBehaviour
{

    private enum state
    {
        animation,
        flying,
        flyingUpwards
    }

    private state mState = state.animation;

    private Collider mCollider = null;


    [SerializeField]
    private GameObject prefabForSnowballHit = null;


    [SerializeField]
    private GameObject prefabForSnowballCancel = null;

    [SerializeField]
    private DamageValues mDamageValues = null;

    [SerializeField]
    private float height = 5f;
    [SerializeField]
    private float gravity = -56f;
    private bool useGravity = false;

    private Transform handJoint = null;

    private Rigidbody mRigidbody = null;

    private Vector3 targetPoint = Vector3.zero;

    private bool ballThrown = false;

    private void Awake()
    {
        mCollider = GetComponent<SphereCollider>();
        mRigidbody = GetComponent<Rigidbody>();
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (debugPath)
        //    DrawPath();

        if (ballThrown && useGravity)
        {
            mRigidbody.velocity += Vector3.up * gravity * Time.deltaTime;
        }
        if (mState == state.animation)
        {
            transform.position = handJoint.position + (transform.forward * 0.5f);
            transform.rotation = handJoint.rotation;
        }
        else if (mState == state.flying && !ballThrown)
        {
            if (targetPoint.y < transform.position.y)
            {
                SetTargetLocation(targetPoint + Vector3.up * 0.25f);
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                Destroy(this.gameObject, 4f);
                ballThrown = true;
            }
            else
            {
                mState = state.flyingUpwards;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                mRigidbody.velocity = (targetPoint - transform.position).normalized * -gravity * 45f * Time.deltaTime;
                StartCoroutine(ActivateGravity());
                Destroy(this.gameObject, 4f);
                ballThrown = true;
            }
        }
    }

    private IEnumerator ActivateGravity()
    {
        yield return new WaitForSeconds(2f);
        mRigidbody.isKinematic = false;
        mRigidbody.useGravity = true;
    }

    #region Drop Snowball

    public void DropSnowball()
    {
        GameObject snowballhitParticle = Instantiate(prefabForSnowballCancel, transform.position, Quaternion.identity, null);
        Destroy(snowballhitParticle, 1f);
        Destroy(gameObject);
    }
#endregion

    #region During animation throw

    public void SetTargetPositionAndChangeState(Vector3 pos)
    {
        targetPoint = pos;
        mState = state.flying;
    }

    public void SetThrowJoint(Transform throwJoint)
    {
        handJoint = throwJoint;
    }

    #endregion

    #region Setting direction and velocity
    private void SetTargetLocation(Vector3 target)
    {

        mRigidbody = GetComponent<Rigidbody>();
        useGravity = true;

        float savedHeight = height;

        ////DEXTER - make height happen broooo.
        //if ((target - transform.position).magnitude / 10f < 1f)
        //{
        //    height = height * ((target - transform.position).magnitude / 20f);
        //}

        mRigidbody.velocity = CalculateLaunchData(target).initialVelocity;

        height = savedHeight;

    }



    private LaunchData CalculateLaunchData(Vector3 target)
    {

        float displacementY = target.y - transform.position.y;

        Vector3 displacementXZ = new Vector3(target.x - transform.position.x, 0f, target.z - transform.position.z);

        float time = Mathf.Sqrt(-2f * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);



        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);

        Vector3 velocityXZ = displacementXZ / time;


        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    //If we want to add debugging later, need some calibration with targetHit input.
    //-------------------------------------------------------------------------------
    //private void DrawPath()
    //{
    //    LaunchData launchData = CalculateLaunchData(targetHit.position);
    //    Vector3 previousDrawPoint = transform.position;

    //    int resolution = 30;
    //    for (int i = 1; i <= 30; i++)
    //    {
    //        float simulationTime = i / (float)resolution * launchData.timeToTarget;

    //        Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;

    //        Vector3 drawPoint = transform.position + displacement;
    //        Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);

    //        previousDrawPoint = drawPoint;
    //    }
    //}
    //-------------------------------------------------------------------------------
    #endregion //Dexter edits here plox.

    #region TriggerCollision
    private void HitWithTrigger(Collider collision)
    {
        if (collision.transform.tag == "PuzzleTree")
        {
            collision.transform.parent.GetComponent<FallingTreeBehavior>().Fall();
        }

        if ((mState == state.flying || mState == state.flyingUpwards)
            && collision.gameObject.tag != "DamageZone" 
            && collision.gameObject.tag != "Player" 
            && collision.gameObject.tag != "Projectile" 
            && collision.gameObject.tag != "Collectible")
        {
            if (collision.tag == "Enemy")
            {
                if (collision.gameObject.GetComponent<HealthHedgehog>() != null)
                {
                   
                    if (!collision.gameObject.GetComponent<InvincibilityFrames>().GetInvincibleState())
                    {
                        collision.gameObject.GetComponent<HealthHedgehog>().Hedgehog_Takingdamage(mDamageValues.damage);
                        collision.gameObject.GetComponent<InvincibilityFrames>().StartInvincibility();
                    }

                }
                else if (collision.gameObject.GetComponent<HealthNisse>() != null)
                {
                    if (!collision.gameObject.GetComponent<InvincibilityFrames>().GetInvincibleState())
                    {
                        collision.gameObject.GetComponent<HealthNisse>().Nisse_TakeDamage(mDamageValues.damage);
                        collision.gameObject.GetComponent<InvincibilityFrames>().StartInvincibility();
                    }

                }
                else if (collision.transform.parent.GetComponent<Snowman_WeakPoint>())
                {
                    if (!collision.gameObject.GetComponent<InvincibilityFrames>().GetInvincibleState())
                    {
                        collision.transform.parent.GetComponent<Snowman_WeakPoint>().TakeDamage();
                        collision.GetComponent<InvincibilityFrames>().StartInvincibility();
                    }
                    else
                    {
                        print("Snowman got hit, but in invincibility frame");
                    }
                }
            }
            GameObject snowballhitParticle = Instantiate(prefabForSnowballHit, transform.position - transform.forward + Vector3.up, Quaternion.identity, null);
            Destroy(snowballhitParticle, 1f);
            Destroy(gameObject);

        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        HitWithTrigger(collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        HitWithTrigger(collision);
    }


    private void OnTriggerExit(Collider collision)
    {
        HitWithTrigger(collision);
    }

    #endregion


    #region ColliderCollision

    private void HitWithCollider(Collision collision)
    {
        print(collision.collider.name);
        print(collision.collider.tag);

        if (mState == state.flying && collision.gameObject.tag != "DamageZone" && collision.gameObject.tag != "Player" && collision.gameObject.tag != "Projectile")
        {
            print("Poof");

            GameObject snowballhitParticle = Instantiate(prefabForSnowballHit, transform.position, Quaternion.identity, null);
            Destroy(snowballhitParticle, 1f);
            Destroy(gameObject);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        HitWithCollider(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        HitWithCollider(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        HitWithCollider(collision);
    }
#endregion
}