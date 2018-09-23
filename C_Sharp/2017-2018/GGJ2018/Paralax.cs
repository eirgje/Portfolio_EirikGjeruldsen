using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour {

    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private float lerpSpeed = 2f;

	// Update is called once per frame
	void Update () {

        Vector3 newPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed * Time.deltaTime);
	}
}
