using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem _eventSystem;
    public GameObject _selectedObject;

    private bool _buttonSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetAxisRaw ("Vertical") != 0 && _buttonSelected == false)
        {
            _eventSystem.SetSelectedGameObject(_selectedObject);
            _buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        _buttonSelected = false;
    }
}
