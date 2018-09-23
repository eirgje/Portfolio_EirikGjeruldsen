using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBoarderScript : MonoBehaviour {
    protected BackgroundMusic bMusic;
    protected enum MusicBorderType
    {
        Castle,
        Woods
    };
    [SerializeField]
    protected MusicBorderType MusicType;

    void Awake() {
         bMusic = transform.parent.GetComponent<BackgroundMusic>();
        }
    void OnTriggerExit(Collider other) {
        if (other.tag == "Player" && MusicType == MusicBorderType.Woods)
        {
            if (!bMusic.returnBoundaryState())
            {
                StartCoroutine(bMusic.EnteringTheWoods());
                bMusic.WoodsEnter();
            }
        }
        else if (other.tag == "Player" && MusicType == MusicBorderType.Castle)
        {
            if (!bMusic.returnBoundaryState()) {
                StartCoroutine(bMusic.EnteringTheCastle());
                bMusic.CastleEnter();
            }
                
        }
    }

}
