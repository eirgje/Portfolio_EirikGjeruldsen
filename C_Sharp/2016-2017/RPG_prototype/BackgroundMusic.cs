using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour {

    [SerializeField]
    private AudioClip CastleClip;
    [SerializeField]
    private string CastleText;

    [SerializeField]
    private AudioClip TownClip;
    [SerializeField]
    private string TownText;

    [SerializeField]
    private AudioClip WoodsClip;
    [SerializeField]
    private string WoodsText;

    private enum TypeOfScene {StartingScene, Desert, RainForest};
    [SerializeField]
    private TypeOfScene SceneType;

    [SerializeField]
    private Text ZoneText = null;
    private Animator ZoneTextAnimator;
    private Animator musicAnimator;

    private AudioSource ThisAudioSource;

    private bool EnteredTheWood = false;
    private bool EnteredTheCastle = true;
    private bool EnteredTheTown = false;
    private bool BoundaryHit = false;

	// Use this for initialization
	void Awake () {
        ThisAudioSource = GetComponent<AudioSource>();
        ThisAudioSource.clip = TownClip;
        ZoneTextAnimator = ZoneText.transform.GetComponent<Animator>();
        ZoneText.text = CastleText;
        musicAnimator = GetComponent<Animator>();
    }

    void Update() {
        if (EnteredTheWood && ThisAudioSource.clip != WoodsClip && !EnteredTheCastle)
        {
            ThisAudioSource.clip = WoodsClip;
            ZoneText.text = WoodsText;
            ThisAudioSource.Play();
        }
        else if (EnteredTheCastle && ThisAudioSource.clip != CastleClip && !EnteredTheWood)
        {
            ThisAudioSource.clip = CastleClip;
            ZoneText.text = CastleText;
            ThisAudioSource.Play();
        }
        else if (!EnteredTheCastle && ThisAudioSource.clip != TownClip && !EnteredTheWood)
        {
            ThisAudioSource.clip = TownClip;
            ZoneText.text = TownText;
            ThisAudioSource.Play();
        }

    }

    public void WoodsEnter() {
        EnteredTheWood = !EnteredTheWood;
        ZoneTextAnimator.SetTrigger("fadeIn");
        musicAnimator.SetTrigger("fadeIn");
        ZoneTextAnimator.SetTrigger("fadeOut");
        musicAnimator.SetTrigger("fadeOut");
    }
    public void CastleEnter() {
        EnteredTheCastle = !EnteredTheCastle;
        ZoneTextAnimator.SetTrigger("fadeIn");
        musicAnimator.SetTrigger("fadeIn");
        ZoneTextAnimator.SetTrigger("fadeOut");
        musicAnimator.SetTrigger("fadeOut");
    }

    public IEnumerator EnteringTheWoods() {
        BoundaryHit = true;
        yield return new WaitForSeconds(0.005f);

        BoundaryHit = false;
    }

    public IEnumerator EnteringTheCastle()
    {
        BoundaryHit = true;
        yield return new WaitForSeconds(0.25f);
        BoundaryHit = false;
    }
    public bool returnBoundaryState() { return BoundaryHit; }
    public void ChangeTheMusic(AudioClip clip) {
        ThisAudioSource.clip = clip;
    }
}
