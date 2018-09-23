using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialText : ScriptableObject {
    [TextArea(30,10)]
    public string text = "";
}
