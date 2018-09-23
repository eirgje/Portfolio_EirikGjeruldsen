using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_LaunchBarrage : MonoBehaviour {

    private ParticleSystem barrageParticles = null;

    [SerializeField]
    private DamageValues mDamageValues = null;

    List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        barrageParticles = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    public void FireOneShot(Vector3 direction, float gravityY)
    {
        print(gravityY);
        if (!barrageParticles.isPlaying)
            barrageParticles.Play();
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = transform.position;
        emitParams.velocity = direction;
        
        barrageParticles.Emit(emitParams, 1);
        
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(barrageParticles, other, collisionEvents);


        for (int i = 0; i < collisionEvents.Count; i++)
        {
            if (collisionEvents[i].colliderComponent != null) {
                if (collisionEvents[i].colliderComponent.tag == "Player")
                {
                    if (other.transform.GetChild(2).GetComponent<HealthPlayer>() != null)
                    {
                        if (!other.transform.GetChild(2).GetComponent<InvincibilityFrames>().GetInvincibleState())
                        {
                            print("Player got hit");
                            other.transform.GetChild(2).GetComponent<InvincibilityFrames>().StartInvincibility();

                            other.transform.GetChild(2).GetComponent<HealthPlayer>().Player_TakingDamage(
                                mDamageValues.damage,
                                mDamageValues.canKnockBack,
                                mDamageValues.knockBackPower * (other.transform.position - transform.position).normalized +
                                mDamageValues.knockBackPower / 2 * other.transform.forward
                                );
                        }
                    }
                    EmitAtLocation(collisionEvents[i]);
                }
                else
                {
                    print(other.tag + " got hit (tag)");
                    print(other.name + " got hit (name)");
                    EmitAtLocation(collisionEvents[i]);
                }
            }

        }
        
    }


    private void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        print(particleCollisionEvent.colliderComponent.tag);
    }
}
