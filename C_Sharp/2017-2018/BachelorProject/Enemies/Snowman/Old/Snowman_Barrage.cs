using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_Barrage : MonoBehaviour {

    [Header("Projectile")]

    [SerializeField]
    private float gravity = -20f;

    public void SetGravity(float newGravity)
    {
        gravity = newGravity * -1f;
    }

    private Snowman_Animations mAnimations = null;

    [SerializeField]
    private float minHeight;
    [SerializeField]
    private float maxHeight;

    [SerializeField]
    private GameObject projectilePrefab = null;

    private bool barrageOnCooldown = false;

    private Particle_LaunchBarrage mLaunchParticles = null;
    private Particle_Indicators mIndicatorParticles = null;


    private void Awake()
    {
        mLaunchParticles = transform.GetChild(1).transform.GetChild(1).GetComponent<Particle_LaunchBarrage>();
        mIndicatorParticles = mLaunchParticles.transform.GetChild(0).GetComponent<Particle_Indicators>();
        CreateShotGoals();

        mAnimations = transform.GetChild(0).GetComponent<Snowman_Animations>();
    }

    #region Creating projectile points at start
    
    private int amountOfDirections = 10;

    [SerializeField]
    private int shotInEachDirection = 16;
    [SerializeField]
    private List<Vector3> listOfBarrageOffsetPositions;
    [SerializeField]
    private float spreadMultiplier = 2f;

    #endregion


    private void OneShot(Vector3 direction)
    {
        mLaunchParticles.FireOneShot(
            CalculateLaunch(direction), -gravity);

        //mIndicatorParticles.SpawnOneIndicator(
        //    direction + (Vector3.up * 0.1f));
    }

    public void ResetBarrage()
    {
        barrageOnCooldown = false;
    }

    #region Blue noise
    public void ShootShots()
    {
        if (!barrageOnCooldown)
        {
            for (int i = 0; i < listOfBarrageOffsetPositions.Count; i++)
            {
                OneShot(listOfBarrageOffsetPositions[i]);
                if (i >= listOfBarrageOffsetPositions.Count)
                    barrageOnCooldown = true;
            }
        }
        
    }

    private void CreateShotGoals()
    {
        Vector3[] tempHolder = new Vector3[amountOfDirections + 1];


        for (var i = 0; i < amountOfDirections/2; i++)
        {
            Vector3 forwardAxis = transform.GetChild(0).forward + transform.GetChild(0).right * (i / 10f);

            if (i == 0)
            {
                tempHolder[i] = forwardAxis;
            }
            else
            {
                tempHolder[i] = forwardAxis;
                print(forwardAxis + " | number: " + (i));
                print(-forwardAxis + " | number: " + (i + (amountOfDirections / 2)));
                tempHolder[i + (amountOfDirections / 2)] = -forwardAxis;
            }
            

            

        }

        for (int i = 0; i < amountOfDirections; i++)
        {
            for (int j = 0; j < shotInEachDirection; j++)
            {
                listOfBarrageOffsetPositions.Add(transform.position + tempHolder[i] * (spreadMultiplier * (j + 1)));
            }
        }
    }

    #endregion

    #region Calculate Projectile curve.

    private Vector3 CalculateLaunch(Vector3 target)
    {
        
        float height = Random.Range(minHeight, maxHeight);
        gravity = Physics.gravity.y;


        float displacementY = target.y - transform.position.y;

        Vector3 displacementXZ = new Vector3(target.x - transform.position.x, 0f, target.z - transform.position.z);

        float time = Mathf.Sqrt(-2f * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);

        Vector3 velocityXZ = displacementXZ / time;


        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }

#endregion
}
