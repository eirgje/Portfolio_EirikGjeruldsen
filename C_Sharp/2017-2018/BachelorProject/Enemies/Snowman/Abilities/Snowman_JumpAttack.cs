using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_JumpAttack : MonoBehaviour {

    [Header("Height / gravity Calculations")]
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 20f;
    [SerializeField]
    private float minSpeed = 50f;
    [SerializeField]
    private float maxSpeed = 150f;
    [SerializeField]
    private float minDistanc = 5f;
    [SerializeField]
    private float maxDistance = 80f;

    [Header("Damage")]
    [SerializeField]
    private DamageValues jumpAttackDamage = null;

    private float gravity = -100f;
    private bool useGravity = false;

    public void SetGravity(float newGravity)
    {
        gravity = newGravity * -1f;
    }

    private float height = 10f;

    [Header("Landing")]
    [SerializeField]
    private float radiusOfDamageZone = 10f;

    private Snowman_Animations mAnimations = null;

    [Header("Debugging")]
    public Transform playerPos = null; //should be removed (having refrence in main script), using for debugging
    public bool useDebugging = false;
    private Vector3 startPos = Vector3.zero;


    private void Awake()
    {
        mAnimations = transform.GetChild(0).GetComponent<Snowman_Animations>();
    }

    private void Update()
    {
        if (useDebugging)
        {
            startPos = transform.position;
            DrawPath();
        }
            
    }





    private void CalculateSpeedAndHeight(Vector3 landingPos)
    {
        float t = (landingPos - transform.position).magnitude / maxDistance;

        height = minHeight + t * (maxHeight - minHeight);
        print(height + " = height");

        gravity = minSpeed + t * (maxSpeed - minSpeed);
        gravity *= -1;
        print(gravity + " = gravity");
    }
    #region Initialize jump
    [SerializeField]
    private float durationUntilNextJump = 4f;
    private Vector3 targetPosition = Vector3.zero;
    private bool jumping = false;
    private bool nextJumpRdy = true;
    private bool landingParticlePlayed = false;


    public void InitializeAttack(Transform player, Rigidbody rb, ParticleSystem landingParticle)
    {
        if (rb.useGravity)
            rb.useGravity = false;

        if (useGravity)
        {
            rb.velocity += Vector3.up * gravity * Time.deltaTime;

        }


        if (nextJumpRdy && !jumping)
        {
            mAnimations.Animation_longJump();
            useGravity = true;
            rb.AddForce(Vector3.up * gravity);
            rb.velocity = CalculateLaunch(player.position);
            targetPosition = player.position;
            jumping = true;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else if ((targetPosition - transform.position).magnitude < 3f && jumping)
        {
            jumping = false;
            nextJumpRdy = false;
            landingParticlePlayed = false;
           
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        if (!jumping && !nextJumpRdy && !landingParticlePlayed)
        {
            RaycastHit hit;
            Ray distanceToGround = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(distanceToGround, out hit, 1f))
            {
                GetComponent<EnemySnowman>().SetSnowManState(EnemySnowman.States.Reatreating);
                landingParticle.Stop();
                landingParticle.Play();
                landingParticlePlayed = true;
                useGravity = false;
                Debug.Log("Did player get hit: " + CheckIfPlayerGotHit());
                nextJumpRdy = true;
                jumping = false;
            }
        }


    }

    public void Reatreat(Vector3 target, Rigidbody rb, ParticleSystem landingParticle)
    {
        if (rb.useGravity)
            rb.useGravity = false;

        if (useGravity)
        {
            rb.velocity += Vector3.up * gravity * Time.deltaTime;

        }


        if (nextJumpRdy && !jumping)
        {
            useGravity = true;
            rb.AddForce(Vector3.up * gravity);
            rb.velocity = CalculateLaunch(target);
            targetPosition = target;
            mAnimations.Animation_Reatreating();
            jumping = true;
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        }
        else if ((targetPosition - transform.position).magnitude < 3f && jumping)
        {
            jumping = false;
            nextJumpRdy = false;
            landingParticlePlayed = false;

            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        if (!jumping && !nextJumpRdy && !landingParticlePlayed)
        {
            RaycastHit hit;
            Ray distanceToGround = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(distanceToGround, out hit, 1f))
            {
                landingParticle.Stop();
                landingParticle.Play();
                landingParticlePlayed = true;
                useGravity = false;

                Debug.Log("Did player get hit: " + CheckIfPlayerGotHit());
                nextJumpRdy = true;
                jumping = false;

                GetComponent<EnemySnowman>().SetSnowManState(EnemySnowman.States.Exhausted);
            }
        }


    }

    #endregion

    #region Setting direction and velocity


    public bool CheckIfPlayerGotHit()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusOfDamageZone);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Player")
            {
                hitColliders[i].GetComponent<Transform>().GetChild(2).GetComponent<HealthPlayer>().Player_TakingDamage(
                    jumpAttackDamage.damage,
                    jumpAttackDamage.canKnockBack,
                    jumpAttackDamage.knockBackPower * Vector3.ProjectOnPlane((hitColliders[i].transform.position - transform.position).normalized, Vector3.up)
                    );
                return true;
            }
        }

        return false;
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData(playerPos.position);
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

    public Vector3 CalculateLaunch(Vector3 target)
    {

        //Setting height/speed depending on distance.
        CalculateSpeedAndHeight(target);


        float displacementY = target.y - transform.position.y;

        Vector3 displacementXZ = new Vector3(target.x - transform.position.x, 0f, target.z - transform.position.z);

        float time = Mathf.Sqrt(-2f * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);

        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity);
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


    private void OnDrawGizmos()
    {
        if (useDebugging)
        {
            Color red = Color.red;
            red.a *= 0.5f;
            Gizmos.color = red;
            Gizmos.DrawSphere(transform.position, radiusOfDamageZone);
        }
    }
}
