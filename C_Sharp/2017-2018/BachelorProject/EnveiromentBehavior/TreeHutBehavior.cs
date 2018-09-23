using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TreeHutBehavior : MonoBehaviour
{

    #region Initialization

    private PlayableDirector mainCamera = null;
    private GameObject hutCam = null;
    [SerializeField]
    private PlayableDirector toTransition = null;
    [SerializeField]
    private PlayableDirector fromTransition = null;
    private MeshRenderer hutRoof = null;

    private bool isInside = false;

    private PlayerMovement playerMovement = null;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        mainCamera = GetComponent<PlayableDirector>();
        hutCam = transform.GetChild(0).GetChild(0).gameObject;
        hutCam.SetActive(false);
        hutRoof = transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>();

        if (!hutRoof.enabled)
            hutRoof.enabled = true;

    }

    #endregion

    #region Stance Switch

    private IEnumerator waitAFrame(bool inside)
    {
        yield return new WaitForFixedUpdate();
        hutRoof.enabled = !inside;
    }

    private void SwitchState(bool inside)
    {
        StartCoroutine(waitAFrame(inside));
        playerMovement.SetImmobile(0.25f, true);
        if (inside)
        {
            if (!hutCam.activeSelf)
                hutCam.SetActive(true);
            toTransition.Play();

        }
        else if (!inside)
        {

            fromTransition.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isInside)
        {
            isInside = true;
            SwitchState(isInside);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isInside)
        {
            isInside = false;
            SwitchState(isInside);
        }
    }

    #endregion
}
