using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BackgroundMusic))]
public class BackgroundMusicEditor : Editor {


    Texture2D woodsImage = null;
    Texture2D townImage = null;
    Texture2D castleImage = null;
    public SerializedProperty
    CastleClipP,
    CastleTextP,
    TownClipP,
    TownTextP,
    WoodsClipP,
    WoodsTextP,
    SceneTypeP,
    ZoneTextP;

    void OnEnable()
    {
        CastleClipP = serializedObject.FindProperty("CastleClip");
        CastleTextP = serializedObject.FindProperty("CastleText");
        TownClipP = serializedObject.FindProperty("TownClip");
        TownTextP = serializedObject.FindProperty("TownText");
        WoodsClipP = serializedObject.FindProperty("WoodsClip");
        WoodsTextP = serializedObject.FindProperty("WoodsText");

        SceneTypeP = serializedObject.FindProperty("SceneType");

        ZoneTextP = serializedObject.FindProperty("ZoneText");

        castleImage = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Editor/EditorPictures/CastlePicture.png", typeof(Texture2D));
        townImage = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Editor/EditorPictures/TownPicture.png", typeof(Texture2D));
        woodsImage = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Editor/EditorPictures/WoodsPicture.png", typeof(Texture2D));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        GUILayout.Space(20);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.PropertyField(ZoneTextP);
        GUILayout.Space(20);
        EditorGUILayout.PropertyField(SceneTypeP);
        EditorGUILayout.EndVertical();
        GUILayout.Space(20);

        BackgroundMusicGUI(CastleTextP, CastleClipP, castleImage);
        BackgroundMusicGUI(TownTextP, TownClipP, townImage);
        BackgroundMusicGUI(WoodsTextP, WoodsClipP, woodsImage);

        serializedObject.ApplyModifiedProperties();
    }


    private void BackgroundMusicGUI(SerializedProperty propertyText, SerializedProperty propertyClip, Texture2D image)
    {

        EditorGUILayout.BeginVertical("Window");
        //EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(propertyText);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(propertyClip);
        GUILayout.Space(25);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(image);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        


    }

}
