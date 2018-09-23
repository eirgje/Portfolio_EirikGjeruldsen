using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LanternBehavior : MonoBehaviour {

    [SerializeField]
    private PlayableDirector gateOpen = null;

    private bool isGateOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isGateOpen)
        {
            gateOpen.Play();
            isGateOpen = true;
        }
            
    }
}
