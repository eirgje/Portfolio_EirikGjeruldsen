using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public Transform _cube;
    public string _talk = "DialougeOptions";

    public int _dialougeCurrent = 0;

    // Use this for initialization
    void OnGUi ()
    {
		if (GUI.Button(new Rect(100,100,125,25), "save"))
        {
            SaveStuff();
        }
        if (GUI.Button(new Rect(100, 140, 125, 25), "load"))
        {
            LoadStuff();
        }
        if (GUI.Button(new Rect(100, 180, 125, 25), "increase"))
        {
            _dialougeCurrent++;
        }
        if (GUI.Button(new Rect(100, 220, 125, 25), "decrease"))
        {
            _dialougeCurrent--;
        }
        if (GUI.Button(new Rect(100, 260, 125, 25), "delete"))
        {
            DeleteSavedData();
        }
    }
	
	// Update is called once per frame
	void SaveStuff()
    {
        PlayerPrefs.SetInt("DialougeOptions", _dialougeCurrent);
	}
    void LoadStuff()
    {
        _dialougeCurrent = PlayerPrefs.GetInt("DialougeOptions", 0);
    }
    void DeleteSavedData()
    {

    }
}
