using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverPositionChangingScript : MonoBehaviour {

    [SerializeField]
    private Transform QuestGiver;

    public void ChangePositionOfQuestGiver() {
        //QuestGiver.parent = transform;
        QuestGiver.position = transform.position;
    }
}
