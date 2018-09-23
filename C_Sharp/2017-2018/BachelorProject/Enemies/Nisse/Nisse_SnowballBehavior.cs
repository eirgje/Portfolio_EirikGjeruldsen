using UnityEngine;
using System.Collections;

public class Nisse_SnowballBehavior : MonoBehaviour
{

    private enum state
    {
        animation,
        flying
    }

    private state mState = state.animation;

    private ParticleSystem mFlyingParticle = null;


    private Rigidbody rb;

    [SerializeField]
    private GameObject prefabForSnowballHit = null;

    [SerializeField]
    private DamageValues mDamageValues = null;

    [SerializeField]
    private float height = 5f;
    [SerializeField]
    private float gravity = -56f;
    private bool useGravity = false;

    private Vector3 currentGravity = Vector3.zero;

    [SerializeField]
    private Transform targetHit = null;

    private Vector3 startPos = Vector3.zero;

    [SerializeField]
    private float dmgRadius = 5f;


    [SerializeField]
    private bool debugPath = true;

    private Transform throwingJoint = null;
    private bool throwBall = false;

    #region UpdateFunctions
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        useGravity = false;
    }


    private void Update()
    {

        if (debugPath)
            DrawPath();

        if (throwBall && useGravity)
        {
            currentGravity = Vector3.up * gravity * Time.deltaTime;

            rb.velocity += currentGravity;
        }
        if (mState == state.animation)
        {
            if (!rb.isKinematic)
                rb.isKinematic = true;
            transform.position = throwingJoint.position;
            transform.rotation = throwingJoint.rotation;
        }
        else if (mState == state.flying && !throwBall)
        {
            if (rb.isKinematic) {
                rb.isKinematic = false;
            }
            SetTargetLocation(targetHit.position);

            throwBall = true;
        }

        
    }
    #endregion

    #region During animation throw

    public void SetThrowJointTransformAndTarget(Transform throwJoint, Transform target)
    {
        throwingJoint = throwJoint;
        targetHit = target;
    }

    public void ChangeState()
    {
        mState = state.flying;
    }

#endregion

    #region Setting direction and velocity
    public void SetTargetLocation(Vector3 target)
    {

        rb = GetComponent<Rigidbody>();
        useGravity = true;
 
        startPos = transform.position;
        if ((target - transform.position).magnitude / 10f < 1f)
        {
            height = height * ((target - transform.position).magnitude / 20f);
        }

        
        rb.velocity = CalculateLaunchData(target).initialVelocity;

    }

    private void DrawPath()
    {
        LaunchData launchData;

        if (targetHit == null)
            launchData = CalculateLaunchData(transform.position + transform.forward * 10f);
        else
            launchData = CalculateLaunchData(targetHit.position);
        Vector3 previousDrawPoint = transform.position;

        int resolution = 30;
        for (int i = 1; i <= 30; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;

            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;

            Vector3 drawPoint = transform.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);

            previousDrawPoint = drawPoint;
        }
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
    #endregion

    #region Collision

    #region TriggerCollision

    private void HitWithTrigger(Collider other)
    {
        if(mState == state.flying && other.gameObject.tag != "DamageZone" && other.gameObject.tag != "Enemy")
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, dmgRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Player")
                {

                    if (hitColliders[i].transform.GetChild(2).GetComponent<HealthPlayer>() != null)
                    {
                        if (!hitColliders[i].transform.GetChild(2).GetComponent<InvincibilityFrames>().GetInvincibleState())
                        {

                            hitColliders[i].transform.GetChild(2).GetComponent<InvincibilityFrames>().StartInvincibility();

                            hitColliders[i].transform.GetChild(2).GetComponent<HealthPlayer>().Player_TakingDamage(
                                mDamageValues.damage,
                                mDamageValues.canKnockBack,
                                mDamageValues.knockBackPower * (other.transform.position - startPos).normalized +
                                mDamageValues.knockBackPower / 2 * other.transform.forward
                                );
                        }
                    }
                }

                i++;
            }
            GameObject snowballhitParticle = Instantiate(prefabForSnowballHit, transform.position, Quaternion.identity, null);
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
        GameObject snowballhitParticle = Instantiate(prefabForSnowballHit, transform.position, Quaternion.identity, null);
        Destroy(snowballhitParticle, 1f);
        Destroy(gameObject);
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

    #endregion

    private void OnDrawGizmos()
    {
        if (debugPath)
        {
            Gizmos.color = Color.red;
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.75f);

            Gizmos.DrawSphere(transform.position, dmgRadius);
        }

    }

}