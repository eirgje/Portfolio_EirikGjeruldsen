using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAvatar : MonoBehaviour
{
    private AudioSource mAudioSource = null;

    [Header("General")]
    [Range(0f, 0.1f)]
    [SerializeField]
    private float reverb = 0.1f;

    private void Start()
    {
        mAudioSource = transform.parent.transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Update()
    {
        //if (mAudioSource.reverbZoneMix != (mAudioSource.reverbZoneMix + reverb))
        //{
        //    mAudioSource.reverbZoneMix = 1f + reverb;
        //}
    }

    #region Audio clips

    #region foot step


    [Header("Foot steps")]

    [Range(0f, 1f)]
    [SerializeField]
    private float footstepVolume = 1f;
    [SerializeField]
    private AudioClip footstepLeft = null;
    [SerializeField]
    private AudioClip footstepRight = null;

    private bool isPlayingLeft = false;
    private IEnumerator waitBeforeNewStepLeft()
    {
        isPlayingLeft = true;
        yield return new WaitForSeconds(footstepLeft.length);
        isPlayingLeft = false;
    }
    public void Play_footstepLeft()
    {
        if (!isPlayingLeft)
        {
            mAudioSource.PlayOneShot(footstepLeft, footstepVolume);
            StartCoroutine(waitBeforeNewStepLeft());
        }
        
    }
    private bool isPlayingRight = false;
    private IEnumerator waitBeforeNewStepRight()
    {
        isPlayingRight = true;
        yield return new WaitForSeconds(footstepRight.length);
        isPlayingRight = false;
    }
    public void Play_footstepRight()
    {
        if (!isPlayingRight)
        {
            mAudioSource.PlayOneShot(footstepRight, footstepVolume);
            StartCoroutine(waitBeforeNewStepRight());
        }
        
    }

    [Space(10)]



    [SerializeField]
    private AudioClip footstepWood = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float footstepWoodVolume = 1f;

    public void Play_footstepWood()
    {
        mAudioSource.PlayOneShot(footstepWood, footstepWoodVolume);
    }


    #endregion


    #region winter suit

    [Header("Winter Suit")]
    [SerializeField]
    private AudioClip winterSuitQuickMovement = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float winterSuitQuickMovementVolume = 1f;

    public void Play_winterSuitQuickMovement()
    {
        mAudioSource.PlayOneShot(winterSuitQuickMovement, winterSuitQuickMovementVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip winterSuitSlowMovement = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float winterSuitSlowMovementVolume = 1f;
    public void Play_winterSuitSlowMovement()
    {
            mAudioSource.PlayOneShot(winterSuitSlowMovement, winterSuitSlowMovementVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip winterSuitRepetetiveMovement = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float winterSuitRepetetiveMovementVolume = 1f;
    private bool isWinterSuitPlaying = false;
    private IEnumerator waitForSuitToFinish()
    {
        isWinterSuitPlaying = true;
        yield return new WaitForSeconds(winterSuitRepetetiveMovement.length);
        isWinterSuitPlaying = false;
    }
    public void Play_winterSuitRepetetiveMovement()
    {
        if (!isWinterSuitPlaying)
        {
            mAudioSource.PlayOneShot(winterSuitRepetetiveMovement, winterSuitRepetetiveMovementVolume);
            StartCoroutine(waitForSuitToFinish());
        }
    }


    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField]
    private AudioClip jumpLiftOff = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float jumpLiftOffVolume = 1f;

    public void Play_jumpLiftOff()
    {
        mAudioSource.PlayOneShot(jumpLiftOff, jumpLiftOffVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip landing = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float landingVolume = 1f;

    public void Play_landing()
    {
        mAudioSource.PlayOneShot(landing, landingVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip rolling = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float rollingVolume = 1f;

    public void Play_rolling()
    {
        mAudioSource.PlayOneShot(rolling, rollingVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip rollCrash = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float rollCrashVolume = 1f;

    public void Play_rollCrash()
    {
        mAudioSource.PlayOneShot(rollCrash, rollCrashVolume);
    }

    #endregion

    #region Snowball

    [Header("Snowball")]
    [SerializeField]
    private AudioClip snowballThrowRelease = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float snowballThrowReleaseVolume = 1f;

    public void Play_snowballThrowRelease()
    {
        mAudioSource.PlayOneShot(snowballThrowRelease, snowballThrowReleaseVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip snowballNotReady = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float snowballChargeThrowVolume = 1f;

    public void Play_snowballNotReady()
    {
        mAudioSource.PlayOneShot(snowballNotReady, snowballChargeThrowVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip snowballThrowChargeReadyCue = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float snowballThrowChargeReadyCueVolume = 1f;

    public void Play_snowballThrowChargeReadyCue()
    {
        mAudioSource.PlayOneShot(snowballThrowChargeReadyCue, snowballThrowChargeReadyCueVolume);
    }

    #endregion

    #region Damage

    [Header("Damage / Death")]
    [SerializeField]
    private AudioClip hurt = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float hurtVolume = 1f;

    public void Play_hurt()
    {
        mAudioSource.PlayOneShot(hurt, hurtVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip death = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float deathVolume = 1f;

    public void Play_death()
    {
        mAudioSource.PlayOneShot(death, deathVolume);
    }

    #endregion

    #region Bum slider

    [Header("Bum slider")]
    [SerializeField]
    private AudioClip bumSliderStart = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float bumSliderStartVolume = 1f;

    public void Play_bumSliderStart()
    {
        mAudioSource.PlayOneShot(bumSliderStart, bumSliderStartVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip bumSliderDuring = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float bumSliderDuringVolume = 1f;

    private float normalVolume = 0f;

    public void Play_bumSliderDuring()
    {
        if (!mAudioSource.isPlaying && mAudioSource.clip != bumSliderDuring)
        {
            mAudioSource.Stop();
            normalVolume = mAudioSource.volume;
            mAudioSource.clip = bumSliderDuring;
            mAudioSource.volume = bumSliderDuringVolume;
            mAudioSource.Play(); 
        }
    }

    public void Stop_bumSliderDuring()
    {
        if (mAudioSource.isPlaying && mAudioSource.clip == bumSliderDuring)
        {
            mAudioSource.clip = null;
            mAudioSource.volume = normalVolume;
            mAudioSource.Stop();
        }
    }

    [Space(10)]

    [SerializeField]
    private AudioClip bumSliderEnd = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float bumSliderEndVolume = 1f;

    public void Play_bumSliderEnd()
    {
        mAudioSource.PlayOneShot(bumSliderEnd, bumSliderEndVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip bumSliderSteering = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float bumSliderSteeringVolume = 1f;

    public void Play_bumSliderSteering()
    {
        mAudioSource.PlayOneShot(bumSliderSteering, bumSliderSteeringVolume);
    }


    #endregion

    #region Human sounds

    [Header("Human sounds")]
    [SerializeField]
    private AudioClip gentleSneeze = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float gentleSneezeVolume = 1f;

    public void Play_gentleSneeze()
    {
        mAudioSource.PlayOneShot(gentleSneeze, gentleSneezeVolume);
    }

    [Space(10)]

    [SerializeField]
    private AudioClip freezingSound = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float freezingSoundVolume = 1f;

    public void Play_freezingSound()
    {
        mAudioSource.PlayOneShot(freezingSound, freezingSoundVolume);
    }

    #endregion

    #endregion
}
