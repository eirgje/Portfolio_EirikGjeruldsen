using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSnowman : MonoBehaviour {

    private AudioSource mAudioSource = null;

    private void Start()
    {
        mAudioSource = transform.parent.transform.GetChild(1).GetComponent<AudioSource>();
    }

    #region Audio Clips

    [Header("Jump")]
    [SerializeField]
    private AudioClip jumpLiftOff = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float jumpLiftOffVolume = 1f;

    public void Play_jumpLiftOff()
    {
        mAudioSource.PlayOneShot(jumpLiftOff, jumpLiftOffVolume);
    }

    [SerializeField]
    private AudioClip jumpLanding = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float jumpLandingVolume = 1f;

    public void Play_jumpLanding()
    {
        mAudioSource.PlayOneShot(jumpLanding, jumpLandingVolume);
    }

    [SerializeField]
    private AudioClip jumpInAir = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float jumpInAirVolume = 1f;

    public void Play_jumpInAir()
    {
        mAudioSource.PlayOneShot(jumpInAir, jumpInAirVolume);
    }

    [Header("Barrage")]
    [SerializeField]
    private AudioClip barrageStart = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float barrageStartVolume = 1f;

    public void Play_barrageStart()
    {
        mAudioSource.PlayOneShot(barrageStart, barrageStartVolume);
    }

    [SerializeField]
    private AudioClip barrageDuring = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float barrageDuringVolume = 1f;

    public void Play_barrageDuring()
    {
        mAudioSource.PlayOneShot(barrageDuring, barrageDuringVolume);
    }

    [Header("Vulnerable")]
    [SerializeField]
    private AudioClip vulnerable = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float vulnerableVolume = 1f;

    public void Play_vulnerable()
    {
        mAudioSource.PlayOneShot(vulnerable, vulnerableVolume);
    }

    [Header("Hurt")]
    [SerializeField]
    private AudioClip hurt = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float hurtVolume = 1f;

    public void Play_hurt()
    {
        mAudioSource.PlayOneShot(hurt, hurtVolume);
    }

    [Header("Death")]
    [SerializeField]
    private AudioClip death = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float deathVolume = 1f;

    public void Play_death()
    {
        mAudioSource.PlayOneShot(death, deathVolume);
    }

    [Header("Cinematic Roar")]
    [SerializeField]
    private AudioClip cinematicRoar = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float cinematicRoarVolume = 1f;

    public void Play_cinematicRoar()
    {
        mAudioSource.PlayOneShot(cinematicRoar, cinematicRoarVolume);
    }




    #endregion
}
