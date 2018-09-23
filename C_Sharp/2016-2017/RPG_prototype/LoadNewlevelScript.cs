using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadNewlevelScript : MonoBehaviour {

    [SerializeField]
    private Text StatusText;
    [SerializeField]
    private string[] newText;
    private int CurrentText = 0;
    private bool TextChangeStarted = false;

    private bool loadingNextSceneActivated = false;
	// Use this for initialization
	void Awake () {
        StatusText.text = newText[CurrentText];
        
        

        if (!loadingNextSceneActivated)
            StartCoroutine(LoadNextScene(2f));
	}

    void Update() {
        if (!TextChangeStarted) {
            StartCoroutine(LoadingScreenText());
        }
    }
    private IEnumerator LoadingScreenText() {
        TextChangeStarted = true;
        yield return new WaitForSeconds(0.33f);
        if (CurrentText < 2)
        {
            CurrentText++;
        }
        else {
            CurrentText = 0;
        }
        StatusText.text = newText[CurrentText];
        TextChangeStarted = false;

    }

    private IEnumerator LoadNextScene(float time) {
        loadingNextSceneActivated = true;
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        
    }

	
}
