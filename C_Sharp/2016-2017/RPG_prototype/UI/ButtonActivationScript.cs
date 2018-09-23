using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationScript : MonoBehaviour {

    [Header("Action bar buttons")]
    [Header("---------------------")]
    [SerializeField]
    private Image ButtonOne = null;
    [SerializeField]
    private Image ButtonTwo = null;
    [SerializeField]
    private Image ButtonThree = null;
    [SerializeField]
    private Image ButtonFour = null;
    [Header("---------------------")]

    [Header("Prefab images of action-bar buttons")]
    [Header("---------------------")]
    [Header("Melee")]
    [SerializeField]
    private Sprite MeleeNormal = null;
    [SerializeField]
    private Sprite MeleeActive = null;
    [Header("Magic cast")]
    [SerializeField]
    private Sprite CastNormal = null;
    [SerializeField]
    private Sprite CastActive = null;
    [Header("Wall")]
    [SerializeField]
    private Sprite WallNormal = null;
    [SerializeField]
    private Sprite WallActive = null;
    

	// Use this for initialization
	void Awake () {
        ButtonOne.sprite = MeleeNormal;
        ButtonTwo.sprite = CastNormal;
        ButtonThree.sprite = WallNormal;
	}

    public void ButtonOneActive(bool Pressed) {
        if (Pressed) {
            ButtonOne.sprite = MeleeActive;
        }
        else if (!Pressed)
        {
            ButtonOne.sprite = MeleeNormal;
        }
    }
    public void ButtonTwoActive(bool Pressed)
    {
        if (Pressed)
        {
            ButtonTwo.sprite = CastActive;
        }
        else if (!Pressed)
        {
            ButtonTwo.sprite = CastNormal;
        }
    }
    public void ButtonThreeActive(bool Pressed)
    {
        if (Pressed)
        {
            ButtonThree.sprite = WallActive;
        }
        else if (!Pressed)
        {
            ButtonThree.sprite = WallNormal;
        }
    }
}
