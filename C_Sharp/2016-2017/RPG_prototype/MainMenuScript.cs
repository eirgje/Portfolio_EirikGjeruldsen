using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    [Header("------------------------")]
    [Header("Camera positions")]
    [Header("------------------------")]

    [SerializeField]
    private Transform CameraPosition = null;
    [SerializeField]
    private Transform MainPosition = null;
    [SerializeField]
    private Transform OptionsPosition = null;
    [SerializeField]
    private Transform AboutUsPosition = null;
    [SerializeField]
    private Transform ExitPosition = null;
    [SerializeField]
    private float CameraLerpSpeed = 5f;

    [Header("------------------------")]
    [Header("Canvases")]
    [Header("------------------------")]
    [SerializeField]
    private GameObject MainCanvas = null;
    [SerializeField]
    private GameObject AboutUsCanvas = null;
    [SerializeField]
    private GameObject OptionsCanvas = null;
    [SerializeField]
    private GameObject ExitCanvas = null;

    private Transform CurrentSlide = null;

    [Header("------------------------")]
    [Header("Sounds")]
    [Header("------------------------")]
    [SerializeField]
    private AudioClip ClickingSound;
    [Range(0, 1)]
    [SerializeField]
    private float Volume;
    private AudioSource MainAudioSource;




	// Use this for initialization
	void Awake () {
        MainAudioSource = GetComponent<AudioSource>();
        CameraPosition.position = MainPosition.position;
        CameraLerpSpeed *= Time.deltaTime;
        SortCanvases(MainCanvas);
	}
	
	// Update is called once per frame
	void Update () {
        if (CurrentSlide != null)
        {
            float distance = (CameraPosition.position - CurrentSlide.position).magnitude;
            if (distance > 1f)
            {
                CameraPosition.position = Vector3.Lerp(CameraPosition.position, CurrentSlide.position, CameraLerpSpeed);
            }
        }
	}

    private void SortCanvases(GameObject ActiveCanvas) {
        for (int i = 0; i < 4 + 1; i++) {
            if (i == 1) {
                MainCanvas.SetActive(false);
            }
            else if (i == 2)
            {
                AboutUsCanvas.SetActive(false);
            }
            else if (i == 3)
            {
                OptionsCanvas.SetActive(false);
            }
            else if (i == 4)
            {
                ExitCanvas.SetActive(false);
            }
        }
        ActiveCanvas.SetActive(true);
    }

    private void SetPressedTarget(Transform ButtonPressed) {
        CurrentSlide = ButtonPressed;
    }

    public void ButtonPressed_Start() {
        SceneManager.LoadScene(1);
        MainAudioSource.PlayOneShot(ClickingSound, Volume);
    }

    public void ButtonPressed_Main()
    {
        SetPressedTarget(MainPosition);
        SortCanvases(MainCanvas);
        MainAudioSource.PlayOneShot(ClickingSound, Volume);
    }

    public void ButtonPressed_AboutUs()
    {
        SetPressedTarget(AboutUsPosition);
        SortCanvases(AboutUsCanvas);
        MainAudioSource.PlayOneShot(ClickingSound, Volume);
    }

    public void ButtonPressed_Options()
    {
        SetPressedTarget(OptionsPosition);
        SortCanvases(OptionsCanvas);
        MainAudioSource.PlayOneShot(ClickingSound, Volume);
    }

    public void ButtonPressed_Exit() {
        SetPressedTarget(ExitPosition);
        SortCanvases(ExitCanvas);
        MainAudioSource.PlayOneShot(ClickingSound, Volume);
    }
    public void ButtonPressed_Exit_YesPress() {
        MainAudioSource.PlayOneShot(ClickingSound, Volume);
        Application.Quit();
    }
}
