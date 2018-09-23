using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLineScript : MonoBehaviour {

    [SerializeField]
    private Transform PlayerPosition;

    private LineRenderer ThisLineRenderer;
	// Use this for initialization
	void Awake () {
        ThisLineRenderer = GetComponent<LineRenderer>();
        ThisLineRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (ThisLineRenderer.enabled == true)
        DrawLineRenderer();

	}

    public void AreYouAiming(bool aim) {
        ThisLineRenderer.enabled = aim;
    }

    private void DrawLineRenderer() {

        Vector3 Direction = (PlayerPosition.position - transform.position) / 10;
        Vector3 MyTransform = transform.position - Direction;


        ThisLineRenderer.SetPosition(0, new Vector3(MyTransform.x, PlayerPosition.position.y + 0.5f, MyTransform.z));
        ThisLineRenderer.SetPosition(1, new Vector3(PlayerPosition.position.x, PlayerPosition.position.y + 0.5f, PlayerPosition.position.z));
    }
}
