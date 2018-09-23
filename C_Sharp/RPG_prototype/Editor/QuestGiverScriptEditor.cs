using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestGiverScript))]
public class QuestGiverScriptEditor : Editor
{

    public SerializedProperty
    AnswerButtonP,    MainUI_P,    MyQuestUI_P,    questInteractionP,    QuestCameraP,    MainCameraP,    CutsceneCamerasP,

    AmountOfCutsceneCamerasConnectedToThisQuestGiverP,    playerControllerP,    QuestDialogueP,    

    QuestGiverTextLinesP,    NewPlayerTransformP,   AmountOfTransformsP,    TextLinesAmountP,    AnswerTextLinesP, VoiceLinesP, AnswerLinesP;
    

    void OnEnable()
    {
        AmountOfCutsceneCamerasConnectedToThisQuestGiverP = serializedObject.FindProperty("AmountOfCutsceneCamerasConnectedToThisQuestGiver");
        QuestCameraP = serializedObject.FindProperty("QuestCamera");
        MainCameraP = serializedObject.FindProperty("MainCamera");
        CutsceneCamerasP = serializedObject.FindProperty("CutsceneCameras");

        AnswerButtonP = serializedObject.FindProperty("AnswerButton");

        QuestGiverTextLinesP = serializedObject.FindProperty("QuestGiverTextLines");
        TextLinesAmountP = serializedObject.FindProperty("TextLinesAmount");
        AnswerTextLinesP = serializedObject.FindProperty("AnswerTextLines");

        MainUI_P = serializedObject.FindProperty("MainUI");
        MyQuestUI_P = serializedObject.FindProperty("MyQuestUI");

        questInteractionP = serializedObject.FindProperty("questInteraction");
        playerControllerP = serializedObject.FindProperty("playerController");

        NewPlayerTransformP = serializedObject.FindProperty("NewPlayerTransform");
        AmountOfTransformsP = serializedObject.FindProperty("AmountOfPlayerTransforms");

        QuestDialogueP = serializedObject.FindProperty("QuestDialogue");
        VoiceLinesP = serializedObject.FindProperty("VoiceLines");
        AnswerLinesP = serializedObject.FindProperty("AnswerLines");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        GUILayout.Space(20);

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different UIs", EditorStyles.largeLabel);
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(MainUI_P);
        GUILayout.Label("(The normal UI)");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(MyQuestUI_P);
        GUILayout.Label("(The current dialog UI)");
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("DialogLines", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.IntSlider(TextLinesAmountP, 0, 30);
        QuestGiverTextLinesP.arraySize = TextLinesAmountP.intValue;
        AnswerTextLinesP.arraySize = TextLinesAmountP.intValue;
        VoiceLinesP.arraySize = TextLinesAmountP.intValue;
        AnswerLinesP.arraySize = TextLinesAmountP.intValue;
        for (int i = 0; i < QuestGiverTextLinesP.arraySize; i++)
        {

            SerializedProperty QuestGiverTextLines = QuestGiverTextLinesP.GetArrayElementAtIndex(i);
            SerializedProperty AnswerTextLines = AnswerTextLinesP.GetArrayElementAtIndex(i);
            SerializedProperty DialogVoice = VoiceLinesP.GetArrayElementAtIndex(i);
            SerializedProperty Answervoice = AnswerLinesP.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(20);
            EditorGUILayout.PropertyField(QuestGiverTextLines);
            GUILayout.Label("(Textline: " + (i + 1) + ")");
            EditorGUILayout.PropertyField(AnswerTextLines);
            GUILayout.Label("(Answer: " + (i + 1) + ")");
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }
        for (int i = 0; i < VoiceLinesP.arraySize; i++)
        {
            SerializedProperty DialogVoice = VoiceLinesP.GetArrayElementAtIndex(i);
            SerializedProperty Answervoice = AnswerLinesP.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(DialogVoice);
            GUILayout.Label("(Dialog-Audio: " + (i + 1) + ")");
            EditorGUILayout.PropertyField(Answervoice);
            GUILayout.Label("(Answer-Audio: " + (i + 1) + ")");
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Cameras", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(MainCameraP);
        GUILayout.Label("(The camera you usually use)");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(QuestCameraP);
        GUILayout.Label("(The dialog camera)");
        GUILayout.Space(20);
        EditorGUILayout.IntSlider(AmountOfCutsceneCamerasConnectedToThisQuestGiverP, 0, 15);
        CutsceneCamerasP.arraySize = AmountOfCutsceneCamerasConnectedToThisQuestGiverP.intValue;
        for (int i = 0; i < CutsceneCamerasP.arraySize; i++) {

            SerializedProperty CutsceneCamera = CutsceneCamerasP.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(20);
            EditorGUILayout.PropertyField(CutsceneCamera);
            GUILayout.Label("(CutsceneCamera: " + (i+1) + ")");
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();



        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Scripts", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(questInteractionP);
        GUILayout.Label("(The script that deals with Quests)");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(playerControllerP);
        GUILayout.Label("(The script that controls the Player)");
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();







        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Different conversation transforms", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.IntSlider(AmountOfTransformsP, 0, 30);
        NewPlayerTransformP.arraySize = AmountOfTransformsP.intValue;
        for (int i = 0; i < NewPlayerTransformP.arraySize; i++)
        {
            SerializedProperty PlayerTransforms = NewPlayerTransformP.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(20);
            EditorGUILayout.PropertyField(PlayerTransforms);
            GUILayout.Label("(Transform number: " + (i + 1) + ")");
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("The text dialog", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(QuestDialogueP);
        GUILayout.Label("(The answer you give in your Dialog-UI)");
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("The button pressed to answer in dialog", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(AnswerButtonP);
        GUILayout.Label("(The answer you give in your Dialog-UI)");
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();


        serializedObject.ApplyModifiedProperties();
    }
}
