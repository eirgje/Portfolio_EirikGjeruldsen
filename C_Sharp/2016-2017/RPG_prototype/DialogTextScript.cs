using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogTextScript : MonoBehaviour {

    [Header("---------------------")]
    [Header("Chat-bubble stuff")]
    [Header("---------------------")]

    [SerializeField]
    private GameObject ChatBubble = null;
    [SerializeField]
    private Text ChatBubbleText;
    private Transform Target = null;
    private bool BubbleActivated = false;

	// Use this for initialization
	void Awake () {
        ChatBubble = GameObject.FindGameObjectWithTag("ChatBubble");
        ChatBubbleText = GameObject.FindGameObjectWithTag("ChatBubbleText").GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Target != null)
        {
            Vector3 TargetPosition = Target.position + new Vector3(0f, 3.5f, 0f);
            Vector3 TargetToScreenPoint = Camera.main.WorldToScreenPoint(TargetPosition);
            ChatBubble.transform.position = TargetToScreenPoint;
        }
        else {
            //ChatBubble.SetActive(false);
        }
	}

    public void OpenChatDialogWithATarget(string newText, float time, Transform target)
    {

        ChatBubble.SetActive(true);
        Target = target;
        ChatBubbleText.text = newText;
        if (!BubbleActivated) {
            StartCoroutine(BubbleAliveTime(time));
        }
    }

    private IEnumerator BubbleAliveTime(float time) {
        BubbleActivated = true;
        yield return new WaitForSeconds(time);
        ChatBubble.SetActive(false);
        Target = null;
        BubbleActivated = false;
    
    }
}
