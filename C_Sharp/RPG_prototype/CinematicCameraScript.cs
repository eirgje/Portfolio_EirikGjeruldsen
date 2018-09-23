using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraScript : MonoBehaviour {
    [SerializeField]
    private GameObject MainCamera;
    [SerializeField]
    private float AnimationTime = 5f;
    [SerializeField]
    private InteractionScript interactionScript;
    // Use this for initialization
    void Awake () {
        StartCoroutine(ReturnToMainCamera(AnimationTime));
        interactionScript.SetDialogState(true);
    }

    IEnumerator ReturnToMainCamera(float timer) {
        yield return new WaitForSeconds(timer);
        MainCamera.SetActive(true);
        interactionScript.SetDialogState(false);
        Destroy(this.gameObject);
    }
}
