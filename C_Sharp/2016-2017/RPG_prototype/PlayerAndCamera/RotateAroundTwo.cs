using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class RotateAroundTwo : MonoBehaviour {

	public Transform Pivot = null;
	private Transform ThisTransform = null;
	private Quaternion DestRot = Quaternion.identity;

	private float PivotDistance = 5f;
    private float MaxPivotDistance = 15f;
	public float RotSpeed;
	private float RotX = 0f;
	private float RotY = 0f;
	public float PivotOffset = 1f;

    [SerializeField]
    private float StartPivotDistance = 15f;

    [SerializeField]
    private PlayerControllerTwo playerController;


    private bool hasCollided = false;
    private float hitPoint = 0f;
    private Vector3 rayHIT;
    private bool startZoomOut = false;
    private float zoomOutPivotDistance = 1f;

	// Use this for initialization
	void Awake () {
		ThisTransform = GetComponent<Transform> ();
		Cursor.visible = false;
		ThisTransform.position = new Vector3(Pivot.position.x, Pivot.position.y + PivotOffset, Pivot.position.z) + ThisTransform.rotation * Vector3.forward * -PivotDistance;
        PivotDistance = StartPivotDistance;

	}
	
	// Update is called once per frame
	void Update () {
        if (!Pivot.GetComponent<PlayerStatScript>().GameOver())
		CameraRotation ();

	}


	void CameraRotation(){
        float Horz = 0;
        float Vert = 0;
        if (!playerController.CameraStop()) {
            Horz = CrossPlatformInputManager.GetAxis("Mouse X"); 
            Vert = CrossPlatformInputManager.GetAxis("Mouse Y");
        }
        else if (playerController.CameraStop()) {
            Horz = 0;
            Vert = 0;
        }



//		Some bugs needs fixing (once you have done a negative rotation)
//		if (ThisTransform.localRotation.x > 0.5f) {
//			Vert = -0.05f;
//		} else if (ThisTransform.localRotation.x < 0f) {
//			Vert = 0.05f;
//		}

        RotX += -Vert;
        RotY += Horz;

		Quaternion YRot = Quaternion.Euler (0f, RotY, 0f);
		DestRot = YRot * Quaternion.Euler (RotX, 0f, 0f);

        
        ThisTransform.rotation = Quaternion.Slerp(transform.rotation, DestRot, RotSpeed * Time.deltaTime);

        if (!hasCollided)
        {
        //    if (zoomOutPivotDistance < 1)
        //    {
        //        zoomOutPivotDistance = 1;
        //    }
        //    else if (zoomOutPivotDistance > 1)
        //    {
        //        zoomOutPivotDistance -= 20 * Time.deltaTime;
        
         //   if (zoomOutPivotDistance >= 1f)
          //      ThisTransform.position = new Vector3(Pivot.position.x, Pivot.position.y + PivotOffset, Pivot.position.z) + ThisTransform.rotation * Vector3.forward * -PivotDistance / zoomOutPivotDistance;
         //   else if (zoomOutPivotDistance < 1f)
            
                ThisTransform.position = new Vector3(Pivot.position.x, Pivot.position.y + PivotOffset, Pivot.position.z) + ThisTransform.rotation * Vector3.forward * -PivotDistance;
            
        }

        else if (hasCollided)
            ThisTransform.position = rayHIT + ThisTransform.rotation * Vector3.forward / 2;
        //ThisTransform.position = new Vector3(Pivot.position.x, Pivot.position.y + PivotOffset, Pivot.position.z) + ThisTransform.rotation * Vector3.forward * -hitPoint;

        ZoomWithWheel();
	}


    public void SetCollisionState(bool state, Vector3 rayHit, float theDistanceForSmoothing)
    {
        hasCollided = state;
        rayHIT = rayHit;


     //   if (rayHitMagnitude < 15 && rayHitMagnitude > 1.5f && state)
    //        hitPoint = rayHitMagnitude;
     //   else if (rayHitMagnitude < 1.5f && state)
     //   {
     //       hitPoint = 1.5f;
    //    }
   //     else
    //    {
   //         hitPoint = PivotDistance;
    //    }

   //     if (rayHitMagnitude < 5f)
    //    {
  //          PivotOffset = 1.25f;
   //     }
   //     else
   //     {
    //        PivotOffset = 1;
    //    }

    //     if (!state && theDistanceForSmoothing > 1) {
    //          zoomOutPivotDistance = theDistanceForSmoothing * 10f;
    //      }
    }


    private void ZoomWithWheel() {
       // print(PivotDistance + " = the pivot distance " + Time.timeSinceLevelLoad);
        if (Input.GetAxis("Mouse ScrollWheel") < -0.05f) {

            if (MaxPivotDistance < 20 && PivotDistance < 20) {
                MaxPivotDistance += 1;
                PivotDistance += 1;
            }
                
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
        {
            if (MaxPivotDistance > 2 && PivotDistance > 2)
            {
                MaxPivotDistance -= 1;
                PivotDistance -= 1;
            }
        }
        if (PivotDistance < 2)
            PivotDistance = 2;
        if (PivotDistance > 20)
            PivotDistance = 20f;

        if (MaxPivotDistance < 2 )
            MaxPivotDistance = 2;
        else if (MaxPivotDistance > 20) {
            MaxPivotDistance = 20;
        }

        if (PivotDistance > MaxPivotDistance) {
            PivotDistance = MaxPivotDistance;
        }
    }



    public bool ReadCollisionState() {
        return hasCollided;
    }
}
