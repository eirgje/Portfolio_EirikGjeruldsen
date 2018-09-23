using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSnowballScript : MonoBehaviour {

    private Vector3 directionToGo = -Vector3.forward;
    private Rigidbody mRigidbody = null;
    private float speed = 3000;

    private float mScale = 0.25f;

    [SerializeField]
    private DamageValues mDamage = null;

    private enum State
    {
        loading,
        rolling
    }

    private enum TypeOfGiantSnowball
    {
        wall,
        spikes
    }
    [SerializeField]
    private TypeOfGiantSnowball mType = TypeOfGiantSnowball.spikes;

    private State mState = State.loading;

    public void SetForward(Vector3 forwardDirection)
    {
        transform.forward = forwardDirection;
    }
    public void SetStartPoint(Vector3 startPoint)
    {
        transform.position = startPoint;
    }

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (mState == State.loading)
        {
            if (mType == TypeOfGiantSnowball.spikes)
            {
                if (mRigidbody.useGravity)
                    mRigidbody.useGravity = false;
                mScale += 30 * Time.fixedDeltaTime;
                if (mScale < 7f)
                    transform.localScale = new Vector3(mScale, mScale, mScale + 3);
                else
                {
                    mState = State.rolling;
                    Destroy(this.gameObject, 2f);
                }
            }
            else
            {
                if (mRigidbody.useGravity)
                    mRigidbody.useGravity = false;

                mState = State.rolling;
                Destroy(this.gameObject, 2f);
            }

                
        }
        else
        {
            if (mType == TypeOfGiantSnowball.spikes)
            {
                if (!mRigidbody.useGravity)
                    mRigidbody.useGravity = true;
            }


            mRigidbody.velocity = transform.forward * speed * Time.fixedDeltaTime;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (other.transform.GetChild(2).GetComponent<HealthPlayer>() != null)
            {
                if (!other.transform.GetChild(2).GetComponent<InvincibilityFrames>().GetInvincibleState())
                {

                    print("Hit player with a giant snowball");

                    other.transform.GetChild(2).GetComponent<InvincibilityFrames>().StartInvincibility();

                    other.transform.GetChild(2).GetComponent<HealthPlayer>().Player_TakingDamage(
                        mDamage.damage,
                        mDamage.canKnockBack,
                        mDamage.knockBackPower * directionToGo
                        );
                }
            }
        }
    }
}
