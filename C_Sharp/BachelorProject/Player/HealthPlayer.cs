using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class HealthPlayer : MonoBehaviour
{

    /* 
    To do:
    */

    /// <summary>
    /// How long before replenishing 1 health. Must be uninterrupted. 
    /// </summary>
    [SerializeField]
    private float mRegenerationDuration = 10f;
    private float mRegenerationTimer = 0f;
    public void StartRegenerationTimer()
    {
        mRegenerationTimer += Time.deltaTime;
    }
    [SerializeField]
    private bool mDebugInvincibility = false;
    private int currentHealth = 10;

    [SerializeField]
    private HealthType mHealthScript = null;
    [SerializeField]
    private DamageImageEffect mDamageImageEffect = null;
    private PlayerMovement mPlayerMovement = null;

    [SerializeField]
    private PlayableDirector mDeathScreen = null;


    void Awake()
    {
        mPlayerMovement = transform.parent.GetComponent<PlayerMovement>();
        mHealthScript.Health_RestoreHealthToMax();
        // print(mHealthScript.Health_GetHealth() + " starting health");
        if (Camera.main.gameObject.GetComponent<DamageImageEffect>() != null)
            mDamageImageEffect = Camera.main.gameObject.GetComponent<DamageImageEffect>();

        currentHealth = mHealthScript.Health_GetHealth();
        mDamageImageEffect.SetHealth(mHealthScript.Health_GetHealth());

        if (mDebugInvincibility)
            print("Debug invincibility is active. Player will not take damage from enemies. You can change this setting in \"Player Health\"");

        mDeathScreen = transform.parent.GetChild(6).GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (mRegenerationTimer > 0f)
        {
            mRegenerationTimer += Time.deltaTime;
            if (mRegenerationTimer >= mRegenerationDuration)
            {
                mDamageImageEffect.IncreaseHealth();
                mHealthScript.TakeDamage(-1);

                // Stops regenerating if at max health
                if (mHealthScript.Health_GetHealth() == mHealthScript.maxHealth)
                    mRegenerationTimer = 0f;
                else
                    mRegenerationTimer = Time.deltaTime;
            }
        }
    }

    public void Player_TakingDamage(int damage, bool includeKnockback, Vector3 knockDirection)
    {
        if (!mDebugInvincibility)
        {
            if (!includeKnockback)
                mHealthScript.TakeDamage(damage);
            else if (includeKnockback)
            {
                mHealthScript.TakeDamage(damage);

                mPlayerMovement.Push(knockDirection, 0.5f, true, false);
            }

            // Starting regeneration timer
            mRegenerationTimer = Time.deltaTime;

            // Signalling damage image effect
            if (damage > 0)
            {
                mDamageImageEffect.DecreaseHealth();
            }
            else if (damage < 0)
            {
                mDamageImageEffect.IncreaseHealth();
            }
        }

        if (mHealthScript.Health_CheckIfDead())
        {
            Camera.main.transform.GetComponent<DamageImageEffect>().IncreaseHealth();
            Camera.main.transform.GetComponent<DamageImageEffect>().IncreaseHealth();
            Camera.main.transform.GetComponent<DamageImageEffect>().IncreaseHealth();

            StartCoroutine(WaitForDeathScreen(8f));
        }

    }

    private IEnumerator WaitForDeathScreen(float timeToWait)
    {
        mDeathScreen.Play();
        transform.parent.GetChild(0).GetComponent<PlayerAnimations>().Animation_Death();
        transform.parent.GetComponent<PlayerMovement>().SetImmobile(8.2f, true);
        yield return new WaitForSeconds(timeToWait);

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            mHealthScript.Health_RestoreHealthToMax();
            transform.parent.GetChild(0).GetComponent<PlayerAnimations>().Animation_Respawn();
            GameObject.Find("SpawnPoints").GetComponent<SpawnPointManager>().Respawn();
        }   
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
