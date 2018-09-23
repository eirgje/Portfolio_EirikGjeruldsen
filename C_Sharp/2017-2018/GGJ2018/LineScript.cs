using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {

    private Animator mAnimator = null;

    public GameObject left = null;
    public GameObject right = null;
    public GameObject synapze = null;

    private ReadLinesByGaze MainController = null;

    public FadeScript mFade = null;
    
    void Awake ()
    {
        mAnimator = transform.GetChild(0).GetComponent<Animator>();
        MainController = transform.parent.GetComponent<ReadLinesByGaze>();
        mFade = Camera.main.transform.GetChild(0).GetComponent<FadeScript>();
	}

    public void AnimationEvent_NewDirection()
    {
        if (left != null && right != null)
            MainController.SwitchToNext();
        else
        {
            mFade.FadeOut();
            print("reached end point, you found nr." + synapze.GetComponent<EndPointBehavior>().index);
            synapze.transform.parent.GetComponent<XMLSerializationExample>().SaveNewInteger(synapze.GetComponent<EndPointBehavior>().index);
            
        }
            
            
    }


    public void Animation_ActivateLine()
    {
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Move");
    }

}
