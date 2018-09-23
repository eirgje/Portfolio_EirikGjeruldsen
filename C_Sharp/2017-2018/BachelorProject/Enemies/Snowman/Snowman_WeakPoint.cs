using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_WeakPoint : MonoBehaviour {

    #region Turning weakpoint on and off.

    [Header("Placement")]
    [SerializeField]
    private Transform pointOfRefrence = null;

    private EnemySnowman mainScript = null;

    private void Awake()
    {
        mainScript = transform.parent.GetComponent<EnemySnowman>();
    }

    private void Update()
    {
        if (mainScript.GetSnowmanState() == EnemySnowman.States.Exhausted)
        {
            if(!transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(true);

            transform.position = pointOfRefrence.position;
        }
        else
        {
            if (transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        print("Took damage, snowman should have a headache!");
        mainScript.TakeDamage();
    }

    #endregion



}
