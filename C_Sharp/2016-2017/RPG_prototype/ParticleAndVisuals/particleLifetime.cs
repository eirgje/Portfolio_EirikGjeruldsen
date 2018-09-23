using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleLifetime : MonoBehaviour {

    [Header("General")]
    [Header("---------------------")]
	[Range(0,10)]
	[SerializeField] private float AliveForSeconds = 2f;

    private enum TypeOfParticle
    {
        Explosion,
        Wall
    };

    [SerializeField]
    private TypeOfParticle ParticleType;


    [Header("---------------------")]
    [Header("Wall")]
    [Header("---------------------")]

    [SerializeField]
    private GameObject WallDestroyPrefab = null;


	// Use this for initialization
	void Awake () {
		StartCoroutine (LifeTime (AliveForSeconds));
	}

	
	private IEnumerator LifeTime(float time){
		yield return new WaitForSeconds (time);
        if (ParticleType == TypeOfParticle.Explosion)
		Destroy (this.gameObject);
        if (ParticleType == TypeOfParticle.Wall)
        {
            GameObject CastBall = Instantiate(WallDestroyPrefab, transform.position, Quaternion.identity, null);
            Destroy(this.gameObject);  
      }
	}
    public void SetLifetime(float time) {
        AliveForSeconds = time;
    }
}
