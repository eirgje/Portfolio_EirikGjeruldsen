using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioNisse))]
public class AudioNisseEditor : Editor
{
        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.TextField("Creating audio events", EditorStyles.boldLabel);
            GUILayout.TextField(
                "1. Go to the file containing the original animation..\n" +
                "2. Go to the animation tab, and go down to events.\n" +
                "3. Add event at the frame you want at the correct time.\n" +
                "4. In the event you can find function, \n    here you write the function that is going to be called.\n" +
                "Check under guidelines to find funtion-name.",
                EditorStyles.largeLabel);
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.TextField("Figuring out the name of the animationFunction", EditorStyles.boldLabel);
            GUILayout.TextField(
                "Example:\n" +
                "We have the cliped named is Footstep Snow\n" +
                "the variable is written with a lower case letter\n" +
                "the name is footstepSnow\n" +
                "the function would then be called:\n" +
                "Play_footstepSnow",
                EditorStyles.largeLabel);
            EditorGUILayout.EndVertical();
            GUILayout.Space(20);
            base.OnInspectorGUI();

            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

    }
