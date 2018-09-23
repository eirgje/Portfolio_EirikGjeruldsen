using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimation : MonoBehaviour {

    public void AnimationEvent_NewDirection()
    {
        transform.parent.GetComponent<LineScript>().AnimationEvent_NewDirection();
    }
}
