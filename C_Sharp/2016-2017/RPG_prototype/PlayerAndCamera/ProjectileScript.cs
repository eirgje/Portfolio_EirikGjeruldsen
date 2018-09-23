using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	private Rigidbody rb;
    private Transform PlayerTransform = null;
    private Transform CameraDirection = null;



    private Transform ThisTransform = null;
    [SerializeField]
    private float DistanceFromGround = 0.75f;
    [SerializeField]
    private float AngleSpeed = 5f;
    private Vector3 DestUp = Vector3.zero;



	private bool doneCasting = false;
	private bool rotationSet = false;

	[SerializeField] private GameObject ExplotionParticle;

	[Range(10,50)]
	[SerializeField] private float projectileSpeed;

    private AudioSource ThisAudioSource;

    private bool HasBeenSent = false;

    [SerializeField]
    private GameObject FlyingEffectPrefab;

    [SerializeField]
    private float ExplosionRadius = 1.8f;
    [SerializeField]
    private LayerMask EnemyLayer;
    [Range(1, 40)]
    [SerializeField]
    private int ProjectileDamage = 10;

    [Range(1f, 5f)]
    [SerializeField]
    private float ProjectileRange = 5f;
    private bool SpawnedMagicBal = false;
    private Vector3 direction = Vector3.zero;
	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> ();
        ThisAudioSource = GetComponent<AudioSource>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ThisTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!SpawnedMagicBal)
            StartCoroutine(LifeTime(ProjectileRange));

		if (doneCasting) {
			
			if (!rotationSet) {
                transform.parent = null;
                CameraDirection = GameObject.FindGameObjectWithTag("CameraPointer").GetComponent<Transform>();

                direction = (CameraDirection.position - PlayerTransform.position).normalized;

                ThisAudioSource.Play();
                //GameObject explosiveTrail = Instantiate(FlyingEffectPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f), this.transform);
				rotationSet = true;
                HasBeenSent = true;
			}

            
            Vector3 NewPos = ThisTransform.position;
            RaycastHit Hit;

            NewPos += direction* projectileSpeed * Time.deltaTime;
            if (Physics.Raycast(ThisTransform.position, -Vector3.up, out Hit))
            {
                NewPos.y = (Hit.point + Vector3.up * DistanceFromGround).y;
                DestUp = Hit.normal;
            }
            ThisTransform.position = NewPos;
            ThisTransform.up = Vector3.Slerp(ThisTransform.up, DestUp, AngleSpeed * Time.deltaTime);

		}

	}

    private IEnumerator LifeTime(float lifeT) {
        SpawnedMagicBal = true;
        yield return new WaitForSeconds(lifeT);
        Destroy(this.gameObject);
        GameObject Explosion = Instantiate(ExplotionParticle, transform.position, Quaternion.identity, null);
        ExplosionDamage(transform.position, ExplosionRadius);
        SpawnedMagicBal = false;
    }

	public void SetCastingBool(bool cast){
		doneCasting = cast;
	}

    private void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, EnemyLayer);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                hitColliders[i].GetComponent<MeleeNpcScript>().DealDamageToMeleeNPC(ProjectileDamage);
                hitColliders[i].GetComponent<MeleeNpcScript>().SetImpactFromDamage(true, PlayerTransform.position, 1);
                print(ProjectileDamage + " :dealt to: " + hitColliders[i] + " the remaining health: " + hitColliders[i].GetComponent<MeleeNpcScript>().GetNpcHealth());
            }
            if (hitColliders[i].tag == "Dummy") {
                hitColliders[i].GetComponent<DummyScript>().DealDamageToDummy(ProjectileDamage);
            }
            i++;
        }
        
    }

	void OnTriggerEnter(Collider other){
		if ((other.tag != "Player" || other.tag != "PlayerBody") && other != null && HasBeenSent) {
            GameObject Explosion = Instantiate(ExplotionParticle, transform.position, Quaternion.identity, null);
            ExplosionDamage(transform.position, ExplosionRadius);
            Destroy (this.gameObject);
		}
	}
}
