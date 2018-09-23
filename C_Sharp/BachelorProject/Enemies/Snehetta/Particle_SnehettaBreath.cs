using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_SnehettaBreath : MonoBehaviour {

    private ParticleSystem breathParticle = null;

    [SerializeField]
    private DamageValues mDamageValues = null;

    List<ParticleCollisionEvent> collisionEvents;

    private void Awake()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        breathParticle = GetComponent<ParticleSystem>();
        
    }
    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(breathParticle, other, collisionEvents);


        for (int i = 0; i < collisionEvents.Count; i++)
        {
            if (collisionEvents[i].colliderComponent != null)
            {
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
