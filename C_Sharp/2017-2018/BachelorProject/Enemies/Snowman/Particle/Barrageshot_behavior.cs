using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrageshot_behavior : MonoBehaviour {

    private enum currentState
    {
        indestructable,
        destructable
    }

    private currentState mState = currentState.indestructable;


    [SerializeField]
    private GameObject prefabForSnowballHit = null;

    [SerializeField]
    private DamageValues mDamageValues = null;

    private Vector3 launchPos = Vector3.zero;
    public void SetLaunchPosition(Vector3 pos) { launchPos = pos; }

    private void Awake()
    {
        StartCoroutine(readyForDestruction());
    }

    private IEnumerator readyForDestruction()
    {
        yield return new WaitForSeconds(0.5f);
        mState = currentState.destructable;
    }

    #region TriggerCollision

    private void HitWithTrigger(Collider other)
    {
        if (mState == currentState.destructable)
        {
            if (other.gameObject.tag == "Player")
            {
                print(other.name);
                if (other.transform.GetChild(2).GetComponent<HealthPlayer>() != null)
                {
                    if (!other.transform.GetChild(2).GetComponent<InvincibilityFrames>().GetInvincibleState())
                    {

                        other.transform.GetChild(2).GetComponent<InvincibilityFrames>().StartInvincibility();

                        other.transform.GetChild(2).GetComponent<HealthPlayer>().Player_TakingDamage(
                            mDamageValues.damage,
                            mDamageValues.canKnockBack,
                            mDamageValues.knockBackPower * (other.transform.position - launchPos).normalized +
                            mDamageValues.knockBackPower / 2 * other.transform.forward
                            );
                    }
                }
            }
            if (other.gameObject.tag != "DamageZone")
            {
                GameObject snowballhitParticle = Instantiate(prefabForSnowballHit, transform.position, Quaternion.identity, null);
                Destroy(snowballhitParticle, 1f);
                Destroy(gameObject);
            }
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

    public Vector3 lookRotation;


    private void FixedUpdate()
    {
        lookRotation = transform.position + GetComponent<Rigidbody>().velocity;
        transform.LookAt(lookRotation);
    }

}
