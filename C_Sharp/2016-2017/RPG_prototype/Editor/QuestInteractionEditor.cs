using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestInteraction))]
public class QuestInteractionEditor : Editor
{

    public SerializedProperty
    QuestInteractionVariationP,
    QuestGiverVariationP,
    HeadlineP,
    DescriptionP,
    ProgressP,
    QuestTrackingHEADLINE_P,
    QuestTrackingTEXT_P,
    QuestGiverP,
    FinishedQuestP,
    GetQuestP,
    MainCameraP,
    QuestTrackingAnimatorP,
    QuestProgressionSoundP;

    void OnEnable()
    {
        QuestTrackingHEADLINE_P = serializedObject.FindProperty("QuestTrackingHEADLINE");
        QuestTrackingTEXT_P = serializedObject.FindProperty("QuestTrackingTEXT");
        QuestGiverP = serializedObject.FindProperty("QuestGiver");
        FinishedQuestP = serializedObject.FindProperty("FinishedQuest");
        MainCameraP = serializedObject.FindProperty("MainCamera");
        GetQuestP = serializedObject.FindProperty("GetQuest");

        QuestInteractionVariationP = serializedObject.FindProperty("QuestInteractionVariation");
        QuestGiverVariationP = serializedObject.FindProperty("QuestGiverVariation");

        HeadlineP = serializedObject.FindProperty("Headline");
        DescriptionP = serializedObject.FindProperty("Description");
        ProgressP = serializedObject.FindProperty("Progress");

        QuestTrackingAnimatorP = serializedObject.FindProperty("QuestTrackingAnimator");
        QuestProgressionSoundP = serializedObject.FindProperty("QuestProgressionSound");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.Space(20);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Text-objects used", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(HeadlineP);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(DescriptionP);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(ProgressP);
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Animators used", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(QuestTrackingAnimatorP);
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Sounds used", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(FinishedQuestP);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(GetQuestP);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(QuestProgressionSoundP);
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Cameras used", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        EditorGUILayout.PropertyField(MainCameraP);
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();

        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Quest-givers", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();  
        EditorGUILayout.BeginVertical("Window");
        EditorGUILayout.IntSlider(QuestGiverVariationP, 0, 10);
        QuestGiverP.arraySize = QuestGiverVariationP.intValue;
        EditorGUILayout.EndVertical();
        GUILayout.Space(5);

        QuestGiverUI();

        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Refrences to the different Quest-texts", EditorStyles.largeLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Window");
        EditorGUILayout.IntSlider(QuestInteractionVariationP, 0, 50);
        EditorGUILayout.EndVertical();
        QuestTrackingHEADLINE_P.arraySize = QuestInteractionVariationP.intValue;
        QuestTrackingTEXT_P.arraySize = QuestInteractionVariationP.intValue;
        GUILayout.Space(5);
        
        QuestTrackingGUI();



        serializedObject.ApplyModifiedProperties();
    }


    private void QuestTrackingGUI()
    {

        for (int i = 0; i < QuestTrackingHEADLINE_P.arraySize; i++)
        {

            GUILayout.BeginVertical("Window");
            SerializedProperty Headline = QuestTrackingHEADLINE_P.GetArrayElementAtIndex(i);
            SerializedProperty Text = QuestTrackingTEXT_P.GetArrayElementAtIndex(i);

            GUILayout.BeginVertical("Window");
            GUILayout.FlexibleSpace();
            GUILayout.Label("((Headline of the quest)");
            EditorGUILayout.PropertyField(Headline);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();


            
            GUILayout.BeginVertical("Window");
            GUILayout.FlexibleSpace();
            GUILayout.Label("(Description");
            EditorGUILayout.PropertyField(Text);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
    }

    private void QuestGiverUI()
    {

        for (int i = 0; i < QuestGiverP.arraySize; i++)
        {
            GUILayout.BeginVertical("Window");
            SerializedProperty QuestGiver = QuestGiverP.GetArrayElementAtIndex(i);

                //GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("QuestGiver " + (i + 1) + ":", GUILayout.Width(45));
                EditorGUILayout.PropertyField(QuestGiver);
                GUILayout.FlexibleSpace();
               // GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.Space(5);

        }



    }
}
