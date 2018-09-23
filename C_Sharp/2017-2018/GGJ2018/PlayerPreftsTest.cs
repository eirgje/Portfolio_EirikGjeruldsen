using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreftsTest : MonoBehaviour
{

    string _talk = "DialougeOptions";
    public int _value;
    public static PlayerPreftsTest Instance;

    public int Dialouge
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
            if (PlayerPrefs.GetInt(_talk) < _value)
            {
                PlayerPrefs.SetInt(_talk, _value);
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        if (!PlayerPrefs.HasKey(_talk))
        {
            PlayerPrefs.SetInt(_talk, 0);
        }
        //PlayerPrefs.SetInt(_talk, _value);
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(PlayerPrefs.GetInt(_talk));

        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetInt(_talk, _value++);
        }

    }
}