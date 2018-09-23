using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {


    [SerializeField]
    private Slider volumeSlider = null;

    [SerializeField]
    private Toggle fullscreenToggle = null;

    [SerializeField]
    private Toggle inverseYToggle = null;

    private static bool isFullscreenOn = true;
    private static bool isInverseYOn = true;


    public void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCameraController>().SetInverseY(isInverseYOn);
        inverseYToggle.isOn = isInverseYOn;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void OnToggleFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        isFullscreenOn = fullscreenToggle.isOn;
        print(isFullscreenOn + " fullscreen state");
    }
    public void OnToggleInverseY()
    {
        isInverseYOn = inverseYToggle.isOn;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCameraController>().SetInverseY(isInverseYOn);
            print(isInverseYOn + " inverse state");
        }
        
    }

    public void OnSlider()
    {
            Debug.Log("Volume changed!");
            AudioListener.volume = volumeSlider.value;
    }
}
