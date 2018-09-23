using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNisse : MonoBehaviour {

    [SerializeField]
    private HealthType mHealthScript = null;
    private EnemyNisse mBehavior = null;

    void Awake()
    {
        mHealthScript.Health_RestoreHealthToMax();
        mBehavior = GetComponent<EnemyNisse>();
        // print(mHealthScript.Health_GetHealth() + " starting health");
    }

    public void Nisse_TakeDamage(int damage)
    {
        
        mHealthScript.TakeDamage(damage);

        if (mHealthScript.Health_CheckIfDead())
        {
            mBehavior.Initialize_Death();
        }
        else
        {
            mBehavior.TookDamage();
        }
    }

}
