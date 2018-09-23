using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{

    private enum cutsceneType
    {
        enter,
        buttonPressed,
        bumslider
    }

    [SerializeField]
    private cutsceneType mCutsceneType = cutsceneType.enter;

    [SerializeField]
    private PlayableDirector mCutscene = null;

    [SerializeField]
    private GameObject interactionButtons = null;
    [SerializeField]
    private Vector3 offsetForInteractionButtons = Vector3.zero;

    private void Awake()
    {
        if (cutsceneType.buttonPressed == mCutsceneType)
        {
            if (interactionButtons == null)
                interactionButtons = GameObject.Find("InteactionButtons");

            if (interactionButtons != null)
            {
                if (interactionButtons.activeSelf)
                    interactionButtons.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (mCutsceneType == cutsceneType.enter)
            {
                mCutscene.Play();
                Destroy(this.gameObject);
            }

            else if (cutsceneType.bumslider == mCutsceneType)
            {
                mCutscene.Play();
                Debug.Log("You can now use the bumslider");
                other.gameObject.GetComponent<PlayerAutoSlide>().SetBumSliderUnlocked();
                other.gameObject.GetComponent<PlayerMovement>().SetImmobile(5.5f, true);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (mCutsceneType == cutsceneType.buttonPressed)
            {

                if (!interactionButtons.activeSelf)
                    interactionButtons.SetActive(true);
                Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(transform.position + offsetForInteractionButtons);

                interactionButtons.transform.position = worldToScreenPos;

                if (Input.GetButtonDown("Interact") || Input.GetAxisRaw("DPadY") < -0.5f)
                {
                    other.GetComponent<PlayerMovement>().SetImmobile(5f, true);
                    mCutscene.Play();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactionButtons.activeSelf)
        {
            interactionButtons.SetActive(false);
        }
    }
}
