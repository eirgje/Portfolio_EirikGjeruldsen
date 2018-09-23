using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionNEW : MonoBehaviour
{

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    LayerMask DefaultLayer;
    [SerializeField]
    LayerMask NegativeLayer;

    private RotateAroundTwo RotationScript;

    private float towardsCameraDistance = 0f;
    private float behindCameraDistance = 0f;
    private int amountHitingCamera = 0;
    private bool CameraPositioning = false;
    void Awake()
    {
        RotationScript = GetComponent<RotateAroundTwo>();
    }

    // Update is called once per frame
    void Update()
    {
            if (!playerTransform.GetComponent<PlayerStatScript>().GameOver())
                CameraCollision();
      //  print(towardsCameraDistance + " Ray to camera           |           " + behindCameraDistance + " Ray behind camera");
    }

    void CameraCollision()
    {


        int height = Camera.main.pixelHeight;
        int width = Camera.main.pixelWidth;

        Vector3 TopRightScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(width, height, Camera.main.nearClipPlane));
        Vector3 TopLeftScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, height, Camera.main.nearClipPlane));
        Vector3 BottomLeftScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 BottomRightScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(width, 0, Camera.main.nearClipPlane));
        Vector3 MiddleScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(width / 2, height / 2, Camera.main.nearClipPlane));

        

        //checking if there is anything in the way.
        LineOfSightOfPlayer(MiddleScreenPoint, TopRightScreenPoint, TopLeftScreenPoint, BottomRightScreenPoint, BottomLeftScreenPoint);

        //Checking if it should return to current max pivotdistance.
        ReturnToPreviousCameraPosition(MiddleScreenPoint, TopRightScreenPoint, TopLeftScreenPoint, BottomRightScreenPoint, BottomLeftScreenPoint);


        //print(amountHitingCamera);
    }



    private void LineOfSightOfPlayer(Vector3 middle, Vector3 topRight, Vector3 topLeft, Vector3 bottomRight, Vector3 bottomLeft) {
        Vector3 screenPoint = Vector3.zero;
        RaycastHit hit;
        for (int i = 0; i < 5; i++)
        {
            if (i == 0){screenPoint = middle;}
            else if (i == 1){screenPoint = topRight;}
            else if (i == 2){screenPoint = topLeft;}
            else if (i == 3){screenPoint = bottomLeft;}
            else if (i == 4) {screenPoint = bottomRight;}
            if (Physics.Raycast(playerTransform.position + new Vector3(0, 0.25f, 0), (screenPoint - playerTransform.position), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(playerTransform.position + new Vector3(0, 0.25f, 0), (screenPoint - playerTransform.position), Color.blue);
                float distance = (hit.point - playerTransform.position).magnitude;
               // print(distance);
                if (i == 0)
                    towardsCameraDistance = distance;
                if (hit.collider.tag != "MainCamera" && !RotationScript.ReadCollisionState() && hit.collider.tag != "Player" && hit.collider.tag != "PlayerBody")
                {
                    print(hit.collider.name);
                    print(hit.collider.tag);
                    RotationScript.SetCollisionState(true, hit.point + (hit.point - playerTransform.position).normalized, 0f);
                    i = 5;
                    
                }
                else
                {
                    amountHitingCamera += 1;
                }
            }
        }
    }

    private void ReturnToPreviousCameraPosition(Vector3 middle, Vector3 topRight, Vector3 topLeft, Vector3 bottomRight, Vector3 bottomLeft)
    {
        RaycastHit hit;
        Vector3 screenPoint = Vector3.zero;
        if (amountHitingCamera == 5)
        {
            for (int i = 0; i < 5; i++){
                if (i == 0) { screenPoint = middle; }
                else if (i == 1) { screenPoint = topRight; }
                else if (i == 2) { screenPoint = topLeft; }
                else if (i == 3) { screenPoint = bottomLeft; }
                else if (i == 4) { screenPoint = bottomRight; }
                Debug.DrawRay(screenPoint, -transform.forward, Color.red);
                if (Physics.Raycast(screenPoint, -transform.forward, out hit, 15f, NegativeLayer))
                {
                
                float distance = (hit.point - playerTransform.position).magnitude;
                if (i == 0)
                   // behindCameraDistance = distance;
                //print(hit.collider);
                if (behindCameraDistance - towardsCameraDistance * -1 > 1 && !CameraPositioning)
                {
                    RotationScript.SetCollisionState(false, Vector3.zero, behindCameraDistance - towardsCameraDistance * -1);
                    i = 5;
                }
            }
        }

     }
        amountHitingCamera = 0;
  }

}

