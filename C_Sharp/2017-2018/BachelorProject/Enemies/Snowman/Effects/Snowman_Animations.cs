using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Snowman_Animations : MonoBehaviour {

    private Animator mAnimator = null;

    [SerializeField]
    private PlayableDirector phaseChangeTimeline = null;

    [SerializeField]
    private PlayableDirector lastPhaseTimeline = null;

    [SerializeField]
    private PlayableDirector konckbackPlayer = null;

    [SerializeField]
    private PlayableDirector deathSceneTimeline = null;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void Animation_shortJump()
    {
        mAnimator.SetTrigger("ShortJump");
    }

    public void Animation_longJump()
    {
        mAnimator.SetTrigger("LongJump");
    }

    public void Animation_SnowballSpawning()
    {
        mAnimator.SetTrigger("GiantSnowball");
    }

    public void Animation_StartExhaust()
    {
        mAnimator.SetTrigger("startExhaust");
    }

    public void Animation_StopExhaust()
    {
        mAnimator.SetTrigger("stopExhaust");
    }

    public void Animation_TakeDamage()
    {
        mAnimator.SetTrigger("takeDamage");
    }

    public void Set_CurrentHealth(int health)
    {
        mAnimator.SetInteger("currentHealth", health);
    }

    public void Animation_HitGround()
    {
        mAnimator.SetTrigger("hittingGround");
    }

    public void Animation_Reatreating()
    {
        mAnimator.SetTrigger("retreating");
    }

    public void Animation_ReatreatingHitGround()
    {
        mAnimator.SetTrigger("reatreatHitGround");
    }

    public void StartPhaseChangeCutscene()
    {
        print("Phase started");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetImmobile(4.55f, true);
        phaseChangeTimeline.Play();
    }

    public void StartLastPhaseCutscene()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetImmobile(8f, true);
        lastPhaseTimeline.Play();
    }

    public void DeathScene()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetImmobile(4f, true);
        deathSceneTimeline.Play();
    }


    public void Animation_StopTwomping()
    {
        mAnimator.SetTrigger("shortJumpEnded");
    }

    public void SetPhaseInAnimator(int currentPhase)
    {
        ChangeAnimationLayer(currentPhase);
        mAnimator.SetInteger("currentPhase", currentPhase);
    }

    public void Animation_SetJumpContinuesly(bool shouldIt)
    {
        mAnimator.SetBool("continueJumping", shouldIt);
    }

    public bool Get_JumpContinouslyCondition()
    {
        return mAnimator.GetBool("continueJumping");
    }

    public void StartLastPhase()
    {
        mAnimator.SetTrigger("LastPhase");
    }


    public void PushBackPlayer()
    {
        konckbackPlayer.Play();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().Push(GameObject.Find("Snowman").transform.forward * 100f, 3f, true, false);
    }

    private void ChangeAnimationLayer(int layerID)
    {
        mAnimator.SetLayerWeight(layerID-1, 0);
        mAnimator.SetLayerWeight(layerID, 1);
    }

    public void PlayerCanContinue(bool canPlayerContinue)
    {
        mAnimator.SetBool("playerCanContinue", canPlayerContinue);
    }
    


}
