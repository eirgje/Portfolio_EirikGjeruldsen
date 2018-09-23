using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Playables;

public class PickupBehavior : MonoBehaviour
{

    /*
        To do: 
        - Display pickups in GUI
        - Store amount of pickups
    */
    [Header("References")]
    [SerializeField]
    private Text mCollectibleCounter = null;
    private static int mNormalPickups = 0;
    private int mRarePickups = 0;
    [SerializeField]
    private Transform mCollectibleParent = null;
    [SerializeField]
    private static int mCollectiblesTotal = 0;

    [SerializeField]
    private PlayableDirector hudAnimation = null;

    [SerializeField]
    private GameManager mGameManager = null;



    private void Awake()
    {
        //if (mCollectibleCounter == null)
            //mCollectibleCounter = GameObject.Find("HUD Canvas_01").transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

        if(mCollectibleParent == null)
        {
            if (GameObject.Find("PickupParent").transform != null)
                mCollectibleParent = GameObject.Find("PickupParent").transform;
        }
        if (mGameManager == null)
            mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (mCollectibleParent != null)
            mGameManager.Add_maxPickUp(mCollectibleParent.childCount);



        mCollectibleCounter = GameObject.Find("HUD Canvas_01").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        mCollectibleCounter.text = mGameManager.Get_collectedPickUps().ToString() + " / " + mGameManager.Get_maxPickUps().ToString();
        hudAnimation = GameObject.Find("HUD Canvas_01").GetComponent<PlayableDirector>();
        //hudAnimation.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Pickup") /* 9: Pickup */)
        {
            if (col.gameObject.tag == "Collectible")
            {
                if(!hudAnimation.gameObject.activeSelf)
                    hudAnimation.gameObject.SetActive(true);

                mGameManager.Add_collectedPickUp();
                //mNormalPickups++;
                if (hudAnimation != null)
                {
                    hudAnimation.Stop();
                    hudAnimation.Play();
                }
                    
                mCollectibleCounter.text = mGameManager.Get_collectedPickUps().ToString() + " / " + mGameManager.Get_maxPickUps().ToString();
                Destroy(col.gameObject);
            }
        }
    }
}