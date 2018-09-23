using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour {

    public int VoiceLineVariation;

    [SerializeField]
    private AudioClip[] GuardVoiceLines;
    [SerializeField]
    private string[] GuardTextLines;
    [SerializeField]
    private float[] TimeLenghtOfLines;

    private bool RandomNumberBeingSet = false;
    private AudioClip choosenAudio;
    private string choosenText;
    private float choosenTime;

    [SerializeField]
    private Transform Highlighter = null;
    [SerializeField]
    private Transform InteractionType = null;
    private Transform HighlightTarget = null;


    [SerializeField]
    private QuestInteraction questInteraction;

    private string CurrentInteractionType = "";
    private bool DialogHappening = false;

    void Update() {
        if (HighlightTarget == null)
        {
            Highlighter.position = new Vector3(130f, 30f, -50f);
            InteractionType.position = new Vector3(1300, 3000f, 1f);
            CurrentInteractionType = "";
            InteractionType.GetComponent<Text>().text = CurrentInteractionType;
        }
        else if (HighlightTarget != null && !DialogHappening)
        {
            Vector3 newHighlightPosition = HighlightTarget.position + new Vector3(0f, 3f, 0f);
            Vector3 worldToScreenPointPosition = HighlightTarget.position + new Vector3(0f, 2.75f, 0f);
            Vector3 newInteractionPosition = Camera.main.WorldToScreenPoint(worldToScreenPointPosition);

            Highlighter.position = newHighlightPosition;
            InteractionType.position = newInteractionPosition;
            InteractionType.GetComponent<Text>().text = CurrentInteractionType;
        }
        else {
            HighlightTarget = null;
        }
    }

    public IEnumerator ChooseALine() {
        RandomNumberBeingSet = true;
        int RandomNumber = Random.Range(0, GuardVoiceLines.Length);
        choosenAudio = GuardVoiceLines[RandomNumber];
        choosenText = GuardTextLines[RandomNumber];
        choosenTime = TimeLenghtOfLines[RandomNumber];
        yield return new WaitForSeconds(choosenTime);
        HighlightTarget = null;
        RandomNumberBeingSet = false;
    }
    public IEnumerator ChosenLine() {
        RandomNumberBeingSet = true;
        choosenAudio = GuardVoiceLines[3];
        choosenText = GuardTextLines[3];
        choosenTime = TimeLenghtOfLines[3];
        yield return new WaitForSeconds(choosenTime);
        HighlightTarget = null;
        RandomNumberBeingSet = false;
    }


    void OnTriggerStay(Collider other) {
        if ((other.tag == "Guard" || other.tag == "QuestGiver" || other.tag == "PickUp") && !DialogHappening) {
            HighlightTarget = other.transform;
            CurrentInteractionType = other.tag;
        }
        if (other.tag == "Guard" && Input.GetKeyDown(KeyCode.E) && questInteraction.ReadCurrentQuest() != 0) {

            print(choosenTime);
            StartCoroutine(ChooseALine());
            print(choosenAudio + " - " + choosenText);
            if (!other.GetComponent<GuardDialogScript>().GetVoiceLineBool())
                StartCoroutine(other.GetComponent<GuardDialogScript>().PlayTheVoiceLine(choosenAudio, choosenText, choosenTime));
        }
        else if (other.tag == "Guard" && Input.GetKeyDown(KeyCode.E) && questInteraction.ReadCurrentQuest() == 0){

            print(choosenTime);
            StartCoroutine(ChosenLine());
            print(choosenAudio + " - " + choosenText);
            if (!other.GetComponent<GuardDialogScript>().GetVoiceLineBool())
                StartCoroutine(other.GetComponent<GuardDialogScript>().PlayTheVoiceLine(choosenAudio, choosenText, choosenTime));
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.tag == "Guard" || other.tag == "QuestGiver" || other.tag == "PickUp") {
            HighlightTarget = null;
        }
    }

    public void SetDialogState(bool isItRunning) {
        DialogHappening = isItRunning;
    }
}
