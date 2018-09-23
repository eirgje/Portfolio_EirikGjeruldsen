using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuBehavior : MonoBehaviour {

    [SerializeField]
    private GameObject mainCanvas = null;
    [SerializeField]
    private GameObject optionsCanvas = null;
    [SerializeField]
    private GameObject creditsCanvas = null;
    [SerializeField]
    private GameObject exitCanvas = null;

    [SerializeField]
    private AudioClip buttonPress = null;

    [SerializeField]
    private AudioClip startGameSound = null;

    private AudioSource mAudioSource = null;
    private Animator cameraAnimator = null;

    private EventSystem mEventSystem = null;

    private GameObject mainList = null;
    private GameObject optionsList = null;
    private GameObject creditsList = null;
    private GameObject exitList = null;

    public void SetControllerTarget(GameObject target)
    {
        mEventSystem.SetSelectedGameObject(target);
    }

    private void Update()
    {
        print(mEventSystem.currentSelectedGameObject);
    }

    private void Start()
    {

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        cameraAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();

        optionsCanvas.SetActive(true);
        creditsCanvas.SetActive(true);
        exitCanvas.SetActive(true);

        mainList = mainCanvas.transform.GetChild(2).GetChild(0).gameObject;
        optionsList = optionsCanvas.transform.GetChild(0).gameObject;
        creditsList = creditsCanvas.transform.GetChild(1).GetChild(0).gameObject;
        exitList = exitCanvas.transform.GetChild(1).GetChild(0).gameObject;

        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        exitCanvas.SetActive(false);

        mEventSystem = GetComponent<EventSystem>();

        SetControllerTarget(mainList);
    }

    #region Buttons
    public void StartPress()
    {
        SceneManager.LoadSceneAsync(1);
        mAudioSource.PlayOneShot(buttonPress, 1f);
        mAudioSource.PlayOneShot(startGameSound, 1f);
        mainCanvas.SetActive(false);
    }

    public void OptionsPress()
    {
        optionsCanvas.SetActive(true);
        mAudioSource.PlayOneShot(buttonPress, 1f);
        SetControllerTarget(optionsList);
        mainCanvas.SetActive(false);
        print("OPTIONS");
    }

    public void CreditPress()
    {
        creditsCanvas.SetActive(true);
        mAudioSource.PlayOneShot(buttonPress, 1f);
        SetControllerTarget(creditsList);
        mainCanvas.SetActive(false);
        print("CREDITS");
    }

    public void ExitPress()
    {
        exitCanvas.SetActive(true);
        mAudioSource.PlayOneShot(buttonPress, 1f);
        SetControllerTarget(exitList);
        mainCanvas.SetActive(false);
    }

    public void BackPressed()
    {
        mainCanvas.SetActive(true);
        mAudioSource.PlayOneShot(buttonPress, 1f);
        SetControllerTarget(mainList);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        exitCanvas.SetActive(false);
    }

    public void CloseProgram()
    {
        Application.Quit();
    }
    #endregion
}
