using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPlayer : MonoBehaviour {

    private PlayerMovement mPlayerMovement = null;


    private ParticleSystem leftFoot = null;
    private ParticleSystem rightFoot = null;
    private ParticleSystem[] impact = new ParticleSystem[3];

    private ParticleSystem[] snowballRelease = new ParticleSystem[2];

    private float durationOfFootParticles = 0.1f;
    private float durationOfImpactParticle = 0.1f;

    private void Start()
    {
        if (mPlayerMovement == null)
            mPlayerMovement = transform.parent.transform.GetComponent<PlayerMovement>();

        leftFoot = transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();
        rightFoot = transform.GetChild(0).transform.GetChild(1).GetComponent<ParticleSystem>();
        

        leftFoot.Stop();
        rightFoot.Stop();

       //for (int i = 0; i < snowballRelease.Length; i++)
       //{
       //    snowballRelease[i] = transform.GetChild(2).transform.GetChild(i).GetComponent<ParticleSystem>();
       //}

         

    }

    #region Snowball release

   //public void Particles_ReleaseSnowball()
   //{
   //    for (int i = 0; i < snowballRelease.Length; i++)
   //    {
   //        snowballRelease[i].Stop();
   //        snowballRelease[i].Play();
   //    }
   //}

#endregion

    #region Left Foot particle
    public void LeftFoot_Start()
    {
        //if (!leftFoot.isPlaying)
        //{
            leftFoot.Play();
            StartCoroutine(LeftFoot_WaitForClose());
        //}
        
    }

    private IEnumerator LeftFoot_WaitForClose()
    {
        yield return new WaitForSeconds(durationOfFootParticles);
        leftFoot.Stop();
    }

    #endregion


    #region Right Foot particle
    public void RightFoot_Start()
    {
        //if (!rightFoot.isPlaying)
        //{
            rightFoot.Play();
            StartCoroutine(RightFoot_WaitToStop());
        //}
    }

    private IEnumerator RightFoot_WaitToStop()
    {
        yield return new WaitForSeconds(durationOfFootParticles);
        rightFoot.Stop();
    }
    #endregion

    #region Play and Stop
    private void Update()
    {
        if (mPlayerMovement.GetVelocityCurrent() == Vector3.zero)
        {
            leftFoot.Stop();
            rightFoot.Stop();
        }
        else
        {
            leftFoot.Play();
            rightFoot.Play();
        }
    }
    #endregion
}
