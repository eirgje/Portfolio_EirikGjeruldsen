using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionWallScript : MonoBehaviour {


    private Vector3 FullSpawnSize = new Vector3(0f, 0.5f, 0f);
    [SerializeField]
    private float speed = 5f;
	
	// Update is called once per frame
	void Update () {
        if (transform.localPosition != FullSpawnSize)
        transform.localPosition = Vector3.Lerp(transform.localPosition, FullSpawnSize, speed * Time.deltaTime);
	}
}
