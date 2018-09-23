using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameMenuScript : MonoBehaviour {


    private InputManager mInputs = null;

    private Event eventForControllerInput = null;

    private List<GameObject> main = null;
    [SerializeField]
    private List<GameObject> options = null;


    public void ContinuePress()
    {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OptionsPress()
    {
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
        SelectNewButton(options[0]);
        transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);        
    }

    public void ExitPress()
    {
        SceneManager.LoadScene(0);
    }

    public void BackPress()
    {
        transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
        SelectNewButton(main[0]);
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
    }

    private void Start()
    {

        main = new List<GameObject>();
        options = new List<GameObject>();

        Time.timeScale = 1;
        for (int i = 0; i < transform.GetChild(0).GetChild(1).childCount; i++)
        {
            main.Add(transform.GetChild(0).GetChild(1).transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < transform.GetChild(0).GetChild(2).childCount; i++)
        {
            options.Add(transform.GetChild(0).GetChild(2).transform.GetChild(i).gameObject);
        }

        transform.GetChild(0).gameObject.SetActive(false);
    }

    private bool moved = false;


    private void SelectNewButton(GameObject selectedGame)
    {
        transform.GetComponent<EventSystem>().SetSelectedGameObject(selectedGame);
    }

    private void Update()
    {

        #region Opening and exiting menu
        if (Input.GetButtonDown("Menu"))
        {
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                SelectNewButton(main[0]);
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                
                Time.timeScale = Mathf.Epsilon;
            }
            else if (transform.GetChild(0).gameObject.activeSelf)
            {
                    ContinuePress();
            }
        }
        #endregion
    }
}
