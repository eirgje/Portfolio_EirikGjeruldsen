using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNisse : MonoBehaviour {

    private AudioSource mAudioSource = null;

    private void Start()
    {
        mAudioSource = transform.parent.transform.GetChild(1).GetComponent<AudioSource>();
    }

    #region Audio Clips

    [Header("Light footstep")]
    [SerializeField]
    private AudioClip lightFootstep = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float lightFootstepVolume = 1f;

    public void Play_lightFootstep()
    {
        mAudioSource.PlayOneShot(lightFootstep, lightFootstepVolume);
    }

    [Header("Throw")]
    [SerializeField]
    private AudioClip throwStart = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float throwStartVolume = 1f;

    public void Play_throwStart()
    {
        mAudioSource.PlayOneShot(throwStart, throwStartVolume);
    }

    [SerializeField]
    private AudioClip throwRelease = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float throwReleaseVolume = 1f;

    public void Play_throwRelease()
    {
        mAudioSource.PlayOneShot(throwRelease, throwReleaseVolume);
    }

    [Header("Giggle")]
    [SerializeField]
    private AudioClip giggle = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float giggleVolume = 1f;

    public void Play_giggle()
    {
        mAudioSource.PlayOneShot(giggle, giggleVolume);
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




    #endregion
}
