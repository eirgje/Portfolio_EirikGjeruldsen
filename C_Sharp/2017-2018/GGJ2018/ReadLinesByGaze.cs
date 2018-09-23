using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.SceneManagement;

public class ReadLinesByGaze : MonoBehaviour
{

    private GameObject left, right, synapze;
    private GameObject tobyTarget = null;

    [SerializeField]
    private GameObject currentGameObjectInFocus = null;


    private void Start()
    {
        currentGameObjectInFocus.GetComponent<LineScript>().Animation_ActivateLine();

        left = currentGameObjectInFocus.GetComponent<LineScript>().left;
        right = currentGameObjectInFocus.GetComponent<LineScript>().right;
        synapze = currentGameObjectInFocus.GetComponent<LineScript>().synapze;
        Camera.main.GetComponent<CameraBehavior>().targetPos = new Vector3(synapze.transform.position.x, synapze.transform.position.y, Camera.main.transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        RaycastHit hit;

		GazePoint mGazePoint = TobiiAPI.GetGazePoint ();
		//Vector3 mScreenPoint = new Vector3 (mGazePoint.Screen.x, mGazePoint.Screen.y, -20f);
        Vector3 mScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -20f);


        float DebugX = mGazePoint.Screen.x;
		float DebugY = mGazePoint.Screen.y;


        for (int i = -30; i < 30; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(mScreenPoint + Vector3.right * i);
            
            if (Physics.Raycast(ray, out hit, 100f))
            {
                tobyTarget = hit.collider.gameObject;
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
                i = 31;
                print("Hit!");
            }
        }

    }
    public void SwitchToNext()
    {
        if (tobyTarget != null)
        {
            if (left == tobyTarget.transform.parent.gameObject)
            {
                left.GetComponent<LineScript>().Animation_ActivateLine();
                currentGameObjectInFocus = left;
                synapze.GetComponent<Animator>().SetTrigger("activated");
            }
            else if (right == tobyTarget.transform.parent.gameObject)
            {
                right.GetComponent<LineScript>().Animation_ActivateLine();

                currentGameObjectInFocus = right;
                synapze.GetComponent<Animator>().SetTrigger("activated");
            }

            else
            {
                int direction = Random.Range(0, 2);

                if (direction == 0)
                {
                    left.GetComponent<LineScript>().Animation_ActivateLine();
                    currentGameObjectInFocus = left;
                    synapze.GetComponent<Animator>().SetTrigger("activated");
                }
                else if (direction == 1)
                {
                    right.GetComponent<LineScript>().Animation_ActivateLine();
                    currentGameObjectInFocus = right;
                    synapze.GetComponent<Animator>().SetTrigger("activated");


                }
            }
        }
        else
        {
            int direction = Random.Range(0, 2);

            if (direction == 0)
            {
                left.GetComponent<LineScript>().Animation_ActivateLine();
                currentGameObjectInFocus = left;
                synapze.GetComponent<Animator>().SetTrigger("activated");
            }
            else if (direction == 1)
            {
                right.GetComponent<LineScript>().Animation_ActivateLine();
                currentGameObjectInFocus = right;
                synapze.GetComponent<Animator>().SetTrigger("activated");


            }
        }
        left = currentGameObjectInFocus.GetComponent<LineScript>().left;
        right = currentGameObjectInFocus.GetComponent<LineScript>().right;
        synapze = currentGameObjectInFocus.GetComponent<LineScript>().synapze;
        Camera.main.GetComponent<CameraBehavior>().targetPos = new Vector3(synapze.transform.position.x, synapze.transform.position.y, Camera.main.transform.position.z);
        tobyTarget = null;
    }



}
