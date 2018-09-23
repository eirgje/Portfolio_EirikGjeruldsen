using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTextAnimation : MonoBehaviour
{
    private Text ThisText;
    private int fontSize;
    private bool settingFontSize = false;
    [SerializeField]
    private GameObject RestartButton;
    [SerializeField]
    private PlayerStatScript player;
    // Use this for initialization
    void Awake()
    {
        ThisText = GetComponent<Text>();
        RestartButton.SetActive(false);
        fontSize = ThisText.fontSize;
    }


    void Update() {
        if (fontSize < 300 && !settingFontSize) {
            StartCoroutine(ChangeFontSize());
            ThisText.fontSize = fontSize;
        }
        else if (fontSize >= 300 && !RestartButton.activeSelf)
        {
            RestartButton.SetActive(true);
            Cursor.visible = true;
        }
    }

    private IEnumerator ChangeFontSize() {
        settingFontSize = true;
        yield return new WaitForSeconds(0.01f);
        fontSize++;
        fontSize++;
        settingFontSize = false;
    }

    public void RespawnPress() {
        player.RespawnPlayer();
    }
}
