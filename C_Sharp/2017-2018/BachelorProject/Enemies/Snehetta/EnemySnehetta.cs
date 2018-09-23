using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnehetta : MonoBehaviour {

    #region
    private enum TypeOfStance
    {
        Idle,
        Spawning,
        Breathing,
        TakingDamage
    }

    private TypeOfStance mTypeOfStance = TypeOfStance.Idle;

    private enum TypeOfPhase
    {
        phase1,
        phase2,
        phase3
    }
    private TypeOfPhase mTypeOfPhase = TypeOfPhase.phase1;


#endregion


    #region Spawning of Hedgehogs

    /// <summary>
    /// The prefab used to spawn a snowballer during the encounter.
    /// </summary>
    [Header("Spawning of Snowballers")]
    [SerializeField]
    private GameObject Spawning_effectPrefab = null;

    [SerializeField]
    private Transform player = null;

    /// <summary>
    /// The spawnPoints used to set the new snowballers out during the encounter.
    /// </summary>
    private Transform[] mSpawnPoints = new Transform[3];

    /// <summary>
    /// The cooldown before the mother can spawn a new snowballer.
    /// </summary>
    [SerializeField]
    private float spawnCooldown = 5f;


    /// <summary>
    /// This int defines amount of spawn each time the mother does an spawn iteration, it allows some visual delay to give it a nicer look.
    /// </summary>
    private int numberSpawned = 0;

    /// <summary>
    /// The mother spawns a snowballer, then creates a cooldown before spawning a new one.
    /// </summary>
    /// <returns></returns>
    private void Spawn_Hedgehog()
    {
        numberSpawned = 0;
        motherAnimator.Spawn();
        StartCoroutine(WaitForRoar(1f));
    }

    /// <summary>
    /// Spawns the particles after the roar has been initialized.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForRoar(float timeBeforeNextSpawn)
    {
        yield return new WaitForSeconds(timeBeforeNextSpawn);
  
        GameObject spawnParticle = Instantiate(Spawning_effectPrefab, mSpawnPoints[numberSpawned].position, Quaternion.identity, null);
        spawnParticle.GetComponent<SpawningHedgehogParticle>().SetTarget(player);
        spawnParticle.GetComponent<SpawningHedgehogParticle>().SetPositionAtStart(mSpawnPoints[numberSpawned].GetChild(0).transform.position);
        print("New target: " + mSpawnPoints[numberSpawned].GetChild(0).transform.position);
        Destroy(spawnParticle, 4f);
        numberSpawned++;

        if (numberSpawned < 3)
            StartCoroutine(WaitForRoar(2f));
    }

    #endregion



    #region Frost-breath
    private Transform BreathImpactLocation = null;


    /// <summary>
    /// Get a refrence to the transform of the where the breath is spawned.
    /// </summary>
    private Transform mHeadTransform = null;

    /// <summary>
    /// Get a refrence to the animator of the mother.
    /// </summary>
    private AnimationsSnehetta motherAnimator = null;

    /// <summary>
    /// The particle effect spawned once the breath hit an object.
    /// <para>  Will be spawned at the raycast hit.</para>
    /// </summary>
    [SerializeField]
    private GameObject prefabForBreathHit = null;


    /// <summary>
    /// This bool is being used to check if the breath is active, once TRUE this will allow for raycast to check distance to nearest object.
    /// </summary>
    private bool breathIsBeingUsed = false;

    /// <summary>
    /// This is the VFX of the breath. This is used to fix the ground particles, and
    /// </summary>
    [SerializeField]
    private GameObject Breath_visualsPLH = null;

    [SerializeField]
    private Transform eyeJointTransform = null;

    private void Breath_Recharge()
    {
        motherAnimator.StartBreath();

    }

    public void StartSnehettaLoop()
    {
        StartCoroutine(MotherActionsLoop(1f));
    }

    /// <summary>
    /// This function will enable the breath visuals.
    /// </summary>
    /// <returns></returns>
    public void Breath_EnableVisuals()
    {
        Breath_visualsPLH.GetComponent<ParticleSystem>().Play();
        Breath_visualsPLH.transform.parent.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        breathIsBeingUsed = true;
    }

    /// <summary>
    /// This public function will be called on a event in the animation to disable the breath.
    /// </summary>
    public void Breath_DisableVisuals()
    {
        Breath_visualsPLH.GetComponent<ParticleSystem>().Stop();
        Breath_visualsPLH.transform.parent.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        breathIsBeingUsed = false;
    }



    #endregion

    #region Phases

    public void NextPhase()
    {
        print("Took damage!");

        if (mTypeOfPhase == TypeOfPhase.phase1)
        {
            mTypeOfPhase = TypeOfPhase.phase2;
            motherAnimator.Health_GetHit();
            
        }
        else if (mTypeOfPhase == TypeOfPhase.phase2)
        {
            mTypeOfPhase = TypeOfPhase.phase3;
            motherAnimator.Health_GetHit();
        }
        else if (mTypeOfPhase == TypeOfPhase.phase3)
        {
            print("###SNEHETTA### just died!");
            motherAnimator.Health_Dying();
            Destroy(this.gameObject, 5f);
        }
    }

#endregion

    #region Mother logic / AI

    private int currentStep = 0;

    private float durationUntilNextMove = 10f;

    [Header("Logic / AI")]
    /// <summary>
    /// Just for checking out stuff when press.
    /// </summary>
    [SerializeField]
    private bool useLoop = false;

    private IEnumerator MotherActionsLoop(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (currentStep < 2)
        {
            if (mTypeOfPhase == TypeOfPhase.phase1)
            {
                durationUntilNextMove = spawnCooldown;
            }
            else if (mTypeOfPhase == TypeOfPhase.phase2)
            {
                durationUntilNextMove = spawnCooldown * 0.75f;
            }
            else if (mTypeOfPhase == TypeOfPhase.phase3)
            {
                durationUntilNextMove = spawnCooldown * 0.5f;
            }
            
            Spawn_Hedgehog();
            currentStep++;
        }
        else
        {
            if (mTypeOfPhase == TypeOfPhase.phase1)
            {
                durationUntilNextMove = spawnCooldown * 2f;
            }
            else if (mTypeOfPhase == TypeOfPhase.phase2)
            {
                durationUntilNextMove = spawnCooldown * 2f * 0.75f;
            }
            else if (mTypeOfPhase == TypeOfPhase.phase3)
            {
                durationUntilNextMove = spawnCooldown * 2 * 0.5f;
            }
            Breath_Recharge();
            currentStep = 0;
        }
        StartCoroutine(MotherActionsLoop(durationUntilNextMove));
    }

#endregion

    #region Update functions

    private void Start()
    {
        motherAnimator = transform.GetChild(0).GetComponent<AnimationsSnehetta>();
    }

    private void Awake()
    {
        mHeadTransform = transform.GetChild(2).transform.GetChild(0).GetComponent<Transform>();

        for (int i = 0; i < 2; i++)
        {
            mSpawnPoints[i] = transform.GetChild(3).transform.GetChild(i).GetComponent<Transform>();
        }

        if (useLoop)
        {
            StartCoroutine(MotherActionsLoop(4f));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Breath_Recharge();
            print("breath");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Spawn_Hedgehog();
        }


        if (Breath_visualsPLH.activeSelf)
        {
            mHeadTransform.position = new Vector3(eyeJointTransform.position.x, mHeadTransform.position.y, eyeJointTransform.position.z);

            mHeadTransform.rotation = new Quaternion(mHeadTransform.rotation.x, eyeJointTransform.rotation.y, mHeadTransform.rotation.z, mHeadTransform.rotation.w);
            
        }
    }


    #endregion

}
