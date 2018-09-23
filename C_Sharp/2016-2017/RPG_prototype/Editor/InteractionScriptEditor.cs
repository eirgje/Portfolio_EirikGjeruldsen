using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractionScript)), CanEditMultipleObjects]
public class InteractionScriptEditor : Editor {

    public SerializedProperty
        VoiceLineVariationP,    //audioclip
        GuardVoiceLinesP,       //String
        GuardTextLinesP,        //float
        TimeLenghtOfLinesP,
        HighlighterP,
        InteractionTypeP,
        questInteractionP;
	// Use this for initialization
	void OnEnable () {
        GuardVoiceLinesP = serializedObject.FindProperty("GuardVoiceLines");
        GuardTextLinesP = serializedObject.FindProperty("GuardTextLines");
        TimeLenghtOfLinesP = serializedObject.FindProperty("TimeLenghtOfLines");
        VoiceLineVariationP = serializedObject.FindProperty("VoiceLineVariation");
        HighlighterP = serializedObject.FindProperty("Highlighter");
        questInteractionP = serializedObject.FindProperty("questInteraction");
        InteractionTypeP = serializedObject.FindProperty("InteractionType");
    }
	
	// Update is called once per frame
	public override void OnInspectorGUI () {
        serializedObject.Update();

        GUILayout.BeginVertical("Window");
        EditorGUILayout.IntSlider(VoiceLineVariationP, 0, 10);
        GuardVoiceLinesP.arraySize = VoiceLineVariationP.intValue;
        GuardTextLinesP.arraySize = VoiceLineVariationP.intValue;
        TimeLenghtOfLinesP.arraySize = VoiceLineVariationP.intValue;

        GUILayout.Space(20);
        
        for (int i = 0; i < GuardVoiceLinesP.arraySize; i++)
        {
            GUILayout.BeginHorizontal("Window");
            SerializedProperty VoiceLine = GuardVoiceLinesP.GetArrayElementAtIndex(i);
            SerializedProperty TextLine = GuardTextLinesP.GetArrayElementAtIndex(i);
            SerializedProperty Duration = TimeLenghtOfLinesP.GetArrayElementAtIndex(i);

            
            GUILayout.Label("Audio " + (i + 1) + ":", GUILayout.Width(45));
            EditorGUILayout.PropertyField(VoiceLine, GUIContent.none, true, GUILayout.Width(80));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Text " + (i + 1) + ":", GUILayout.Width(45));
            EditorGUILayout.PropertyField(TextLine, GUIContent.none, true, GUILayout.MinWidth(10));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Duration " + (i + 1) + ":", GUILayout.Width(45));
            EditorGUILayout.PropertyField(Duration, GUIContent.none, true, GUILayout.Width(30));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

        }
        
        EditorGUILayout.PropertyField(HighlighterP);
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(InteractionTypeP);
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(questInteractionP);
        GUILayout.Space(20);
        GUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        
    }
}
