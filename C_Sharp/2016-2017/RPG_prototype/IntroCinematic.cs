using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour {
    [SerializeField]
    private string[] RealText;
    [SerializeField]
    private string[] InteruptionText;

    [SerializeField]
    private Text Text_Story;
    [SerializeField]
    private Text Text_Interupt;
    [SerializeField]
    private GameObject Text_ShoveIt;
    [SerializeField]
    private GameObject Text_ShoveIt2;
    [SerializeField]
    private GameObject MySonText;

    [SerializeField]
    private Animator Story;
    [SerializeField]
    private Animator Interupt;

    private int PartOfAct = 0;
    private bool ActStarted = false;

    [SerializeField]
    private GameObject[] AssetsToTurnOn;
    [SerializeField]
    private GameObject[] AssetsToTurnOffWhenCutscene;
    [SerializeField]
    private GameObject FirstCamera = null;

	// Use this for initialization
	void Awake () {
        Text_ShoveIt.SetActive(false);
        Text_ShoveIt2.SetActive(false);
        MySonText.SetActive(false);
        for (int i = 0; i < AssetsToTurnOn.Length - 1; i++)
        {
            AssetsToTurnOn[i].SetActive(false);
        }
	}

    private IEnumerator RunAct() {

        ActStarted = true;
        float timer = 1f;

        if (PartOfAct == 0) {
            Story.SetTrigger("Fade_In");
            Text_Story.text = RealText[0];
            timer = 4f;
        }
        else if (PartOfAct == 1) { 
            Story.SetTrigger("Fade_Out");
            timer = 1f;
        }
        else if (PartOfAct == 2)
        {
            Story.SetTrigger("Fade_In");
            Text_Story.text = RealText[1];
            timer = 5f;
        }
        else if (PartOfAct == 3)
        {
            Story.SetTrigger("Fade_Out");
            Interupt.SetTrigger("Fade_In");
            Text_Interupt.text = InteruptionText[0];
            timer = 4f;
        }
        else if (PartOfAct == 4)
        {
            Text_Interupt.text = InteruptionText[1];
            timer = 1f;

        }
        else if (PartOfAct == 5){
            Text_Interupt.text = InteruptionText[2];
            timer = 0.81f;
        }
        else if (PartOfAct == 6){
            Text_Interupt.text = InteruptionText[3];
            timer = 2.5f;
        }
        else if (PartOfAct == 7)
        {
            Text_Interupt.text = InteruptionText[4];
            timer = 2f;
        }
        else if (PartOfAct == 8)
        {
            Story.SetTrigger("Fade_In");
            Interupt.SetTrigger("Fade_Out");
            Text_Story.text = RealText[2];
            timer = 4f;
        }
        else if (PartOfAct == 9)
        {
            Story.SetTrigger("Fade_Out");
            Interupt.SetTrigger("Fade_In");
            Text_Interupt.text = InteruptionText[5];
            timer = 1f;
        }
        else if (PartOfAct == 10)
        {
            Text_Interupt.text = InteruptionText[6];
            timer = 2f;
        }
        else if (PartOfAct == 11)
        {
            Text_ShoveIt.SetActive(true);
            Text_Interupt.text = "";
            timer = 2f;
        }
        else if (PartOfAct == 12)
        {
            Text_ShoveIt.SetActive(false);
            Text_Interupt.text = InteruptionText[7];
            timer = 2f;
        }
        else if (PartOfAct == 13)
        {
            Story.SetTrigger("Fade_In");
            Interupt.SetTrigger("Fade_Out");
            Text_Story.text = RealText[3];
            timer = 9f;
        }
        else if (PartOfAct == 14)
        {
            Story.SetTrigger("Fade_Out");
            Interupt.SetTrigger("Fade_In");
            Text_Interupt.text = InteruptionText[8];
            timer = 2.5f;
        }
        else if (PartOfAct == 15)
        {
            Text_ShoveIt2.SetActive(true);
            Interupt.SetTrigger("Fade_Out");
            timer = 1f;
        }
        else if (PartOfAct == 16)
        {
            Text_ShoveIt2.SetActive(false);
            timer = 2f;
        }
        else if (PartOfAct == 17)
        {
            MySonText.SetActive(true);
            timer = 4.5f;
        }
        else if (PartOfAct == 18)
        {
            MySonText.SetActive(false);
            FirstCamera.SetActive(false);
            for (int i = 0; i < AssetsToTurnOn.Length - 1; i++) {
                AssetsToTurnOn[i].SetActive(true);
            }
            for (int i = 0; i < AssetsToTurnOffWhenCutscene.Length - 1; i++)
            {
                AssetsToTurnOffWhenCutscene[i].SetActive(false);
            }
            timer = 7f;
        }
        else if (PartOfAct == 19)
        {
            SceneManager.LoadScene(2);
        }



        yield return new WaitForSeconds(timer);
        PartOfAct++;
        ActStarted = false;
    }

	
	// Update is called once per frame
	void Update () {
        if (!ActStarted) {
           StartCoroutine(RunAct());
        }
	}
}
