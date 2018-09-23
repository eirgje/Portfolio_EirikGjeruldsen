using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsNisse : MonoBehaviour {

    private Animator mAnimator = null;

    [SerializeField]
    private float chargingUpProjectileDuration = 1f;

    private GameObject currentSnowball = null;

    void Awake ()
    {
        mAnimator = GetComponent<Animator>();
	}

    public void Animations_SetMovementState(bool isMoving) { mAnimator.SetBool("isMoving", isMoving); }

    public bool Animations_GetMovementState() { return mAnimator.GetBool("isMoving"); }

    public void Animation_StartAttack(GameObject snowball)
    {
        mAnimator.SetTrigger("startAttack");
        currentSnowball = snowball;
    }

    public void Animation_TakingDamage() { mAnimator.SetTrigger("tookDamage"); }

    public void DoneTakingDamage()
    {
        transform.parent.GetComponent<EnemyNisse>().AnimationEvent_DoneTakingDamage();
    }

    public void Dissolve() { transform.parent.GetComponent<DissolvingDeath>().Dissolve(); }

    public void Animation_Death() { mAnimator.SetTrigger("dying"); }

    public void Animation_ReadyToThrow() { mAnimator.SetTrigger("readyToThrow"); }

    public void StartChanneling()
    {
        StartCoroutine(LenghtOfThrow());
    }
    private IEnumerator LenghtOfThrow()
    {
        yield return new WaitForSeconds(chargingUpProjectileDuration + 0.1f);
        Animation_ReadyToThrow();

    }

    public void ReleaseSnowball()
    {
        if(currentSnowball != null)
            currentSnowball.GetComponent<Nisse_SnowballBehavior>().ChangeState();
        transform.parent.GetComponent<EnemyNisse>().DoneThrowing();
    }

    public void Animations_Giggle()
    {
        mAnimator.SetTrigger("giggle");
    }

}
