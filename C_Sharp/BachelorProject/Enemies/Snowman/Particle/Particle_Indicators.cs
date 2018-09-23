using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Indicators : MonoBehaviour {

    private ParticleSystem indicators;

    private void Start()
    {
        indicators = GetComponent<ParticleSystem>();
    }

    public void SpawnOneIndicator(Vector3 position)
    {
        if (!indicators.isPlaying)
            indicators.Play();
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position;

        indicators.Emit(emitParams, 1);
    }
}
