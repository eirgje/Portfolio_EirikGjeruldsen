using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDialogScript : MonoBehaviour {

    private AudioSource ThisAudioSource;
    private AudioClip CurrentAudioClip;
    private string CurrentTextLine;
    private DialogTextScript TextScript;
    private bool VoiceLineStarted = false;
    private Animator ThisAnimator;

    void Awake() {
        ThisAudioSource = GetComponent<AudioSource>();
        TextScript = GetComponent<DialogTextScript>();
        ThisAnimator = GetComponent<Animator>();
    }

    public bool GetVoiceLineBool() { return VoiceLineStarted; }

    public IEnumerator PlayTheVoiceLine(AudioClip newAudioClip, string newTextLine, float time)
    {
            VoiceLineStarted = true;
            CurrentAudioClip = newAudioClip;
            CurrentTextLine = newTextLine;
            TextScript.OpenChatDialogWithATarget(newTextLine, time, transform);
            ThisAudioSource.PlayOneShot(CurrentAudioClip, 1f);
            ThisAnimator.SetBool("isTalking", true);

            yield return new WaitForSeconds(time);
            ThisAnimator.SetBool("isTalking", false);
            VoiceLineStarted = false;
    }
}
