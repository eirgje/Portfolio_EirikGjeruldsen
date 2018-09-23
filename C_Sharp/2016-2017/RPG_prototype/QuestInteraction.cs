using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuestInteraction : MonoBehaviour {

    public int QuestInteractionVariation;
    public int QuestGiverVariation;

    [SerializeField]
    private Text Headline;
    [SerializeField]
    private Text Description;
    [SerializeField]
    private Text Progress;

    private Transform PlayerTransform;
    private InteractionScript interactionScript;
    private PlayerControllerTwo playerController;

    [SerializeField]
    private GameObject QuestTracker;
    [SerializeField]
    private string[] QuestTrackingHEADLINE;
    [SerializeField]
    private string[] QuestTrackingTEXT;
    private string QuestTrackingPROGRESS;
    [SerializeField]
    private QuestGiverScript[] QuestGiver;

    private int CurrentQuest = 0;


    //Sounds
    private AudioSource QuestAudioSource;
    [SerializeField]
    private AudioClip FinishedQuest;
    [SerializeField]
    private AudioClip GetQuest;
    [SerializeField]
    private AudioClip QuestProgressionSound;

    //Cameras
    [SerializeField]
	private GameObject MainCamera;
    private GameObject CurrentQuestCamera;

    //Animator
    [SerializeField]
    private Animator QuestTrackingAnimator;

    private bool treasurePickedUp = false;
    private bool dummyDead = false;


    void Awake () {

		MainCamera.SetActive (true);
		

        QuestAudioSource = transform.GetChild(0).GetComponent<AudioSource>();

        PlayerTransform = transform.parent.GetComponent<Transform> ();
		playerController = PlayerTransform.GetComponent<PlayerControllerTwo> ();
        interactionScript = GetComponent<InteractionScript>();


        Headline.text = QuestTrackingHEADLINE[CurrentQuest];
        Description.text = QuestTrackingTEXT[CurrentQuest];
        Description.color = Color.red;
        QuestTrackingAnimator.SetTrigger("swishIn");
    }

	void OnTriggerStay(Collider other) {
        if (other.tag == "Guard" && Input.GetKeyDown(KeyCode.E) && CurrentQuest == 0)
        {
            AddToCurrentQuest();
        }
        if (other.tag == "QuestGiver" && Input.GetKeyDown(KeyCode.E) && CurrentQuest == 1) {
            
            InteractWithQuestGiver();
            QuestGiver[CurrentQuest].OpenDialog();
        }
        if (other.tag == "PickUp" && Input.GetKeyDown(KeyCode.E) && CurrentQuest == 2) {
            treasurePickedUp = true;
            QuestProgressComplete();
            QuestAudioSource.PlayOneShot(QuestProgressionSound, 0.2f);
            Destroy(other.gameObject);
        }
        if (other.tag == "QuestGiver" && Input.GetKeyDown(KeyCode.E) && CurrentQuest == 2 && treasurePickedUp)
        {
            treasurePickedUp = false;
            QuestProgressComplete();
            InteractWithQuestGiver();
            QuestGiver[CurrentQuest].OpenDialog();
        }

        if (other.tag == "QuestGiver" && Input.GetKeyDown(KeyCode.E) && CurrentQuest == 3 && dummyDead)
        {
            treasurePickedUp = false;
            QuestProgressComplete();
            InteractWithQuestGiver();
            QuestGiver[CurrentQuest].OpenDialog();
        }
    }

    private void QuestProgressComplete() {
        Description.text = "Return to the old man";
        Description.color = Color.green;
    }

    public void SetQuestTrackingText() {
        Headline.text = QuestTrackingHEADLINE[CurrentQuest];
        Description.text = QuestTrackingTEXT[CurrentQuest];
        QuestTrackingAnimator.SetTrigger("swishOut");
        QuestTrackingAnimator.SetTrigger("swishIn");
        QuestAudioSource.PlayOneShot(GetQuest, 0.1f);
    }

    private void InteractWithQuestGiver() {
        MainCamera.SetActive(false);
        CurrentQuestCamera = QuestGiver[CurrentQuest].ReadQuestCamera();
        CurrentQuestCamera.SetActive(true);
        Cursor.visible = true;
        playerController.isDialogActive(true);
        interactionScript.SetDialogState(true);
        PlayerTransform.position = new Vector3(QuestGiver[CurrentQuest].ReadQuestPlayerPosition().position.x, PlayerTransform.position.y, QuestGiver[CurrentQuest].ReadQuestPlayerPosition().position.z);
        PlayerTransform.rotation = QuestGiver[CurrentQuest].ReadQuestPlayerPosition().rotation;
    }



    public int ReadCurrentQuest() { return CurrentQuest; }
    public void IsDummyDead(bool isIt) {
        dummyDead = isIt;
    }
    public bool ReturnDummyDead() { return dummyDead; }
    public void AddToCurrentQuest() {
        CurrentQuest++;
        SetQuestTrackingText();
        QuestAudioSource.PlayOneShot(FinishedQuest, 0.25f);
    }
}
