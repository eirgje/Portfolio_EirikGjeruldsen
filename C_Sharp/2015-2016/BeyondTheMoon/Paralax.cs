using UnityEngine;
using System.Collections;

public class Paralax : MonoBehaviour {
    public GameObject cam;
    public float paralxMultiplier;
    Vector3 origin;
    Vector3 cameraOrigin;
    
	void Awake () {
        origin = transform.position;
        cameraOrigin = cam.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = (origin + ((cam.transform.position - cameraOrigin) * paralxMultiplier));
	}
}
