using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour {
    [Range(0, 100)]
    [SerializeField]
    private float RotationSpeed = 0.3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0f, RotationSpeed * Time.deltaTime, 0f) * transform.rotation;
	}
}
