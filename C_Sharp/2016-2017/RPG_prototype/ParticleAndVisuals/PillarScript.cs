using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour {


    private LineRenderer ThisLineRenderer = null;
    [SerializeField]
    private Transform NextPillar;
	// Use this for initialization
	void Awake () {
        ThisLineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        DrawLineRenderer();
	}

    private void DrawLineRenderer()
    {
        ThisLineRenderer.SetPosition(0, new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z));
        ThisLineRenderer.SetPosition(1, new Vector3(NextPillar.position.x, NextPillar.position.y + 3, NextPillar.position.z));
    }
}
