using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour {

    [SerializeField]
    private const int priority = 1;

    private bool isTargetDead = false;

    public bool IsTargetDead { get; set;}

    public int GetPriority() { return priority; }
}
