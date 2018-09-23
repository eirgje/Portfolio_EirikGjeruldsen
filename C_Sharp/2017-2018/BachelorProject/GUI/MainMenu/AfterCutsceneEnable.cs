using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCutsceneEnable : MonoBehaviour {

    private void OnEnable()
    {
        GameObject.Find("ControllerSupport").GetComponent<MainMenuBehavior>().SetControllerTarget(transform.GetChild(2).GetChild(0).gameObject);
    }
}
