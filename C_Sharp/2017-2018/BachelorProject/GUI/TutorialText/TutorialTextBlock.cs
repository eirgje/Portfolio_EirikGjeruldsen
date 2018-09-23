using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialTextBlock : MonoBehaviour {

    [SerializeField]
    private TutorialText mText;

    private TextMeshProUGUI textInScene = null;

    private void Awake()
    {
        textInScene = GameObject.Find("TutorialCanvas").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && textInScene.text != mText.text)
        {
            textInScene.text = mText.text;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && textInScene.text == mText.text)
        {
            textInScene.text = "";
        }
    }


}