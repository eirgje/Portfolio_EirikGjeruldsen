using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour {

    public AudioMixer _mixer;

    public Slider _musicVolSlider;
    public Slider _masterVolSlider;
    public Slider _soundVolSlider;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        _mixer.SetFloat("MusicVol", _musicVolSlider.value);
        _mixer.SetFloat("MasterVol", _masterVolSlider.value);
        _mixer.SetFloat("SoundVol", _soundVolSlider.value);

    }

    //public void 
    
} 
