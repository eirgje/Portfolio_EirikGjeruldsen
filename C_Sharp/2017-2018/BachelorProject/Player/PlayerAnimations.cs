using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : HeadAnimatorBehavior
{
    private PlayerMovement mPlayerMovement = null;

    private InputManager mInputManager = null;


    private ParticlesPlayer mParticles = null;

    [SerializeField]
    private Transform camTransform = null;

    private bool isShooting = false;
    public void NoLongerShooting() { isShooting = false; }

    private bool throwingComplete = true;
    public bool ThrowingComplete { get; set; }

    private GameObject mBumSlider = null;
    [SerializeField]
    private float mBumSliderAnimationDuration = 0f;
    private float mBumSliderTimer = 0f;
    public void StartBumSliderTimer() { mBumSliderTimer += Time.deltaTime; }

    #region Animator set-animationstates

    private bool hasStopped = false;

    private void Animation_SetMovement()
    {
        mAnimator.SetFloat("Speed", mPlayerMovement.GetSpeedCurrentNormalized());
    }

    public void Animation_SetJump(bool valueIn)
    {
        mAnimator.SetBool("Jump", valueIn);
    }

    public bool Animation_GetJump()
    {
        return mAnimator.GetBool("Jump");
    }

    public void Animation_IsStartingToShoot()
    {
        mAnimator.SetLayerWeight(1, 0);
        mAnimator.SetLayerWeight(4, 1);

        mAnimator.SetLayerWeight(2, 0);
        mAnimator.SetLayerWeight(3, 1);


        mAnimator.SetTrigger("aimStart");
        isShooting = true;
    }

    public void Animation_CancelShooting()
    {
        mAnimator.SetTrigger("shouldReset");
    }

    public void Animation_ThrowSnowball()
    {
        mAnimator.SetTrigger("Throw");
        ThrowingComplete = false;
    }

    public void Animation_StartHanging(bool trueOrFalse)
    {
        mAnimator.SetBool("hang", trueOrFalse);
    }

    public bool GetHangingState()
    {
        return mAnimator.GetBool("hang");
    }

    public void Animation_StopHanging()
    {
        mAnimator.SetTrigger("shouldStopHanging");
        mAnimator.SetBool("hang", false);
    }

    public void Animation_StartClimbing()
    {
        mAnimator.SetTrigger("climb");
    }

    public void Animation_StopClimbing()
    {
        mAnimator.ResetTrigger("climb");
    }

    public void Animation_StartSliding()
    {
        mBumSlider.SetActive(true);
        mAnimator.SetBool("isSliding", true);
    }

    public void Animation_RemoveBumSlider()
    {
        mBumSlider.SetActive(false);
    }

    public void Animation_StopSliding()
    {
        mAnimator.SetBool("isSliding", false);
    }

    public void Animation_SetSlideDirection(float dir)
    {
        mAnimator.SetFloat("slideHorizontal", dir);
    }

    public void Animation_DoneThrowing()
    {
        ThrowingComplete = true;
    }

    public void Animation_Death()
    {
        mAnimator.SetTrigger("Death");
    }

    public void Animation_Respawn()
    {
        mAnimator.SetTrigger("Respawn");
    }


    #endregion

    #region Particles

    public void Particles_StartLeftFoot()
    {
        mParticles.LeftFoot_Start();
    }


    public void Particles_StartRightFoot()
    {
        mParticles.RightFoot_Start();
    }

    #endregion

    private bool rollUsed = false;
    private bool throwUsed = false;
    private bool jumpUsed = false;
    private bool impactHit = false;

    void Awake()
    {
        GetHeadAnimtorComponents();
        camTransform = Camera.main.transform;
        camTransform = Camera.main.transform;
        mInputManager = GameObject.Find("Managers").transform.GetChild(0).GetComponent<InputManager>();

        // If this script is on player
        if (transform.name != "Player Visual (1)")
        {
            mPlayerMovement = transform.parent.GetComponent<PlayerMovement>();
            mParticles = transform.parent.transform.GetChild(4).GetComponent<ParticlesPlayer>();
            mBumSlider = transform.GetChild(1).GetChild(1).gameObject;
            mBumSlider.SetActive(false);
            ThrowingComplete = true;
        }
        else
        {
            mPlayerMovement = GameObject.Find("Player_01").GetComponent<PlayerMovement>();
            mParticles = GameObject.Find("Player_01").transform.GetChild(4).GetComponent<ParticlesPlayer>();
        }
    }

    private void Update()
    {
        // Checking if bum slider is appearing when it shouldn't
        if (mBumSliderTimer > 0f && mPlayerMovement.GetState() != PlayerMovement.State.Slide)
        {
            mBumSliderTimer += Time.deltaTime;
            if (mBumSliderTimer >= mBumSliderAnimationDuration)
            {
                mBumSliderTimer = 0f;
                mBumSlider.SetActive(false);
            }
        }

        if (transform.parent.gameObject.activeSelf)
        {
            switch (mPlayerMovement.GetState())
            {
                case PlayerMovement.State.Locomotion:
                    {

                        if (mInputManager.GetTriggers().y != 0 && !mAnimator.GetBool("isAiming"))
                        {
                            mAnimator.SetBool("isAiming", true);
                            // You are not allowed to change the avatar state machine from PlayerAnimations. 
                            // I've relocated this code to the PlayerThrow script
                            //mPlayerMovement.IsAiming(true);
                        }
                        else if (mInputManager.GetTriggers().y == 0)
                        {
                            if (mAnimator.GetBool("isAiming"))
                            {
                                // You are not allowed to change the avatar state machine from PlayerAnimations. 
                                // I've relocated this code to the PlayerThrow script
                                //mPlayerMovement.IsAiming(false);
                                mAnimator.SetBool("isAiming", false);
                            }

                        }

                        if (mInputManager.GetTriggers().y != 0)
                        {
                            Vector3 lookpos = transform.parent.transform.position + new Vector3(camTransform.forward.x, 0f, camTransform.forward.z);
                            transform.parent.transform.LookAt(lookpos);

                            mAnimator.SetFloat("StrafeX", mInputManager.GetStickLeft().x);
                            mAnimator.SetFloat("StrafeY", mInputManager.GetStickLeft().y);

                            if (new Vector2(mInputManager.GetStickLeft().x, mInputManager.GetStickLeft().y).magnitude <= 0.01f)
                            {
                                mAnimator.SetFloat("StrafeX", 0f);
                                mAnimator.SetFloat("StrafeY", 0f);
                            }

                        }


                        Animation_SetMovement();

                        break;
                    }
                case PlayerMovement.State.Air:
                    {

                        break;
                    }

                case PlayerMovement.State.Jump:
                    {
                        if (!jumpUsed)
                        {
                            jumpUsed = true;
                            mAnimator.SetTrigger("Jump");
                        }
                        break;
                    }
                case PlayerMovement.State.Roll:
                    {
                        if (!rollUsed)
                        {
                            mAnimator.SetLayerWeight(1, 1);
                            mAnimator.SetLayerWeight(4, 0);

                            mAnimator.SetLayerWeight(2, 1);
                            mAnimator.SetLayerWeight(3, 0);
                            NoLongerShooting();

                            mAnimator.SetTrigger("Roll");
                            rollUsed = true;
                        }
                        break;
                    }
                case PlayerMovement.State.Hang:
                    {
                        break;
                    }
                case PlayerMovement.State.Balance:
                    {
                        break;
                    }
                case PlayerMovement.State.Slide:
                    {
                        break;
                    }
                default:
                    {
                        print("This should never trigger!");
                        break;
                    }
            }
        }

        if (mPlayerMovement.GetState() != PlayerMovement.State.Jump && mPlayerMovement.GetState() != PlayerMovement.State.Air && jumpUsed)
        {
            jumpUsed = false;
        }

        if (rollUsed && mPlayerMovement.GetState() != PlayerMovement.State.Roll && mPlayerMovement.GetImmobile() == false)
        {
            rollUsed = false;
        }
        if (impactHit && mPlayerMovement.GetImmobile() == false)
            impactHit = false;


    }

}
