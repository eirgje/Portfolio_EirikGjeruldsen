using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverScript : MonoBehaviour {

    //Buttons & Text
    [SerializeField]
    private GameObject AnswerButton;
    [SerializeField]
    private Text QuestDialogue;
    private Text AnswerButtonText;

    //VoiceActing
    [SerializeField]
    private AudioClip[] VoiceLines;
    [SerializeField]
    private AudioClip[] AnswerLines;
    private AudioSource ThisAudioSource;
    //UI
    [SerializeField]
    private GameObject MainUI;
    [SerializeField]
    private GameObject MyQuestUI;

    //Scripts
    [SerializeField]
    private QuestInteraction questInteraction;
    [SerializeField]
    private PlayerControllerTwo playerController;

    //Cameras
    [SerializeField]
    private GameObject QuestCamera;
    [SerializeField]
    private GameObject MainCamera;
    public int AmountOfCutsceneCamerasConnectedToThisQuestGiver;
    [SerializeField]
    private GameObject[] CutsceneCameras;
    private GameObject CurrentCutsceneCamera = null;
    private int CutsceneCameraNumber = 0;

    //Conversation
    private bool AnswerHasBeenGiven = false;
    private int ConversationProgress = 0;
    private bool HasConversationStarted = false;

    public int TextLinesAmount;
    [SerializeField]
    private string[] QuestGiverTextLines;
    [SerializeField]
    private string[] AnswerTextLines;

    [SerializeField]
    private Transform[] NewPlayerTransform;
    private int CurrentTransform = 0;
    public int AmountOfPlayerTransforms;



    private bool havePlayedAudio = false;
    private bool CameraDelay = false;
    //----------------------------------------------------------------------    
    // Use this for initialization
    void Awake () {
        ThisAudioSource = GetComponent<AudioSource>();
        AnswerButton.GetComponent<Button>().onClick.AddListener(AnswerButtonPress);
        AnswerButtonText = AnswerButton.transform.GetChild(0).GetComponent<Text>();
        QuestCamera.SetActive(false);
        MyQuestUI.SetActive(false);
    }

    void Update() {
        OverviewOfStoryProgression();
        if (questInteraction.ReturnDummyDead() && !CameraDelay) {
            StartCoroutine(CameraStartOnDelay(2f));
        }
    }

    private IEnumerator CameraStartOnDelay(float timer) {
        CameraDelay = true;
        yield return new WaitForSeconds(timer);
        UseCutsceneCamera();
    }

    void AnswerButtonPress()
    {
        ConversationProgress++;
        havePlayedAudio = false;
        ThisAudioSource.Stop();
    }

    private void OverviewOfStoryProgression()
    {

        //first Conversation

        if (questInteraction.ReadCurrentQuest() == 1 && ConversationProgress == 0 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
            MainCamera.SetActive(false);
        }
        else if (questInteraction.ReadCurrentQuest() == 1 && ConversationProgress == 1 && HasConversationStarted) {
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 1 && ConversationProgress == 2 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 1 && ConversationProgress == 3 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
            print(AnswerTextLines[ConversationProgress]);
        }
        else if (questInteraction.ReadCurrentQuest() == 1 && ConversationProgress == 4 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
            UseCutsceneCamera();
            CurrentCutsceneCamera.SetActive(true);
            QuestCamera.SetActive(false);
            MyQuestUI.SetActive(false);
            MainUI.SetActive(true);
            Cursor.visible = false;
            questInteraction.AddToCurrentQuest();
            playerController.isDialogActive(false);
            havePlayedAudio = false;
            ThisAudioSource.Stop();

        }

        //second conversation
        if (questInteraction.ReadCurrentQuest() == 2 && ConversationProgress == 5 && HasConversationStarted)
        {
            if (!ThisAudioSource.isPlaying && !havePlayedAudio)
            {
                ThisAudioSource.PlayOneShot(VoiceLines[ConversationProgress], 1f);
                havePlayedAudio = true;
            }
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 2 && ConversationProgress == 6 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 2 && ConversationProgress == 7 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 2 && ConversationProgress == 8 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
            UseCutsceneCamera();
            CurrentCutsceneCamera.SetActive(true);
            QuestCamera.SetActive(false);
            MyQuestUI.SetActive(false);
            MainUI.SetActive(true);
            Cursor.visible = false;
            questInteraction.AddToCurrentQuest();
            playerController.isDialogActive(false);
            havePlayedAudio = false;
            ThisAudioSource.Stop();
        }

        //Third Conversation
        else if (questInteraction.ReadCurrentQuest() == 3 && ConversationProgress == 9 && HasConversationStarted)
        {
            if (!ThisAudioSource.isPlaying && !havePlayedAudio)
            {
                ThisAudioSource.PlayOneShot(VoiceLines[ConversationProgress], 1f);
                havePlayedAudio = true;
            }
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 3 && ConversationProgress == 10 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
        }
        else if (questInteraction.ReadCurrentQuest() == 3 && ConversationProgress == 11 && HasConversationStarted)
        {
            SetDialogAndAnswerText(ConversationProgress);
            UseCutsceneCamera();
            CurrentCutsceneCamera.SetActive(true);
            QuestCamera.SetActive(false);
            MyQuestUI.SetActive(false);
            MainUI.SetActive(true);
            Cursor.visible = false;
            questInteraction.AddToCurrentQuest();
            playerController.isDialogActive(false);
            havePlayedAudio = false;
            ThisAudioSource.Stop();
        }



    }


    private void SetDialogAndAnswerText(int i) {
        AnswerButtonText.text = AnswerTextLines[i];
        QuestDialogue.text = QuestGiverTextLines[i];

        if (!ThisAudioSource.isPlaying && !havePlayedAudio)
        {
            ThisAudioSource.PlayOneShot(VoiceLines[ConversationProgress], 1f);
            havePlayedAudio = true;
        }
    }

    private IEnumerator AnswerButtonPressed() {
        AnswerHasBeenGiven = true;
        ConversationProgress++;
        yield return new WaitForSeconds(0.1f);
        havePlayedAudio = false;
        AnswerHasBeenGiven = false;
    }



    private void UseCutsceneCamera() {
        CurrentCutsceneCamera = CutsceneCameras[CutsceneCameraNumber];
        CurrentCutsceneCamera.SetActive(true);
        QuestCamera.SetActive(false);
        CutsceneCameraNumber++;
    }

    public GameObject ReadQuestCamera() {
        return QuestCamera;
    }
    public Transform ReadQuestPlayerPosition()
    {
        return NewPlayerTransform[CurrentTransform];
    }
    public void OpenDialog()
    {
        MainUI.SetActive(false);
        MyQuestUI.SetActive(true);
        HasConversationStarted = true;
        if (ConversationProgress != 0)
        ConversationProgress++;
        
    }



}
