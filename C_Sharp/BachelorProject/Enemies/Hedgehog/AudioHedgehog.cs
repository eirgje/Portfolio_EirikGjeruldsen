using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHedgehog : MonoBehaviour {

    private AudioSource mAudioSource = null;

    private void Start()
    {
        mAudioSource = transform.parent.transform.GetChild(4).GetComponent<AudioSource>();
    }

    #region Audio clips

    [Header("Wading in snow")]
    [SerializeField]
    private AudioClip wadingInSnow = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float wadingInSnowVolume = 1f;

    public void Play_wadingInSnow()
    {
        mAudioSource.PlayOneShot(wadingInSnow, wadingInSnowVolume);
    }

    [Header("Battle cry (scream)")]
    [SerializeField]
    private AudioClip battleCry = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float battleCryVolume = 1f;

    public void Play_battleCry()
    {
        mAudioSource.PlayOneShot(battleCry, battleCryVolume);
    }

    [Header("Sprinting")]
    [SerializeField]
    private AudioClip sprint = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float sprintVolume = 1f;

    public void Play_sprint()
    {
        mAudioSource.PlayOneShot(sprint, sprintVolume);
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
