using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ImpNpcScript : MonoBehaviour {

    [SerializeField]
    private Transform[] ListOfPathNodes;
    [Range(1,30)]
    [SerializeField]
    private int currentDestination = 0;
    private Animator ThisAnimator;
    private Transform PathNodeParent;

    private bool hasSetNewDest = false;
    private NavMeshAgent ThisNavMesh;
    private enum TypeOfNPC{
        Worker,
        Artist,
        Guard,
        Walking_Guard,
        Child

    };

    [SerializeField]
    private TypeOfNPC NPC_type;

    [SerializeField]
    private Material MyMaterial;

	// Use this for initialization
	void Awake () {
        PathNodeParent = GameObject.FindGameObjectWithTag("PathNodeParent").GetComponent<Transform>();
        
        ThisNavMesh = GetComponent<NavMeshAgent>();
        ThisAnimator = GetComponent<Animator>();
        if (NPC_type == TypeOfNPC.Worker) {
            transform.localScale = new Vector3(1.2f, 1.0f, 1.2f);
        }
        else if (NPC_type == TypeOfNPC.Child) {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (NPC_type == TypeOfNPC.Artist)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (NPC_type == TypeOfNPC.Guard)
        {
            transform.localScale = new Vector3(1.25f, 1.5f, 1.25f);
        }
        else if (NPC_type == TypeOfNPC.Walking_Guard)
        {
            GetListOfNods(48);
            ListOfPathNodes[0] = PathNodeParent.GetChild(0).GetComponent<Transform>();
            transform.localScale = new Vector3(1.25f, 1.5f, 1.25f);
            ThisNavMesh.destination = ListOfPathNodes[currentDestination].position;
            
        }
	}

    void GetListOfNods(int listLength) {
        for (int i = 0; i < listLength; i++) {
            ListOfPathNodes[i+1] = PathNodeParent.GetChild(i+1).GetComponent<Transform>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (NPC_type == TypeOfNPC.Walking_Guard)
        {
            ThisAnimator.SetBool("isWalking", true);
            if (ThisNavMesh.remainingDistance <= ThisNavMesh.stoppingDistance)
            {
                if (currentDestination < ListOfPathNodes.Length - 1)
                {
                    if (!hasSetNewDest)
                        StartCoroutine(SetNewDestination(1f));
                }
                else
                {
                    currentDestination = 0;
                }
            }
        }
		
	}

    private IEnumerator SetNewDestination(float time) {
        hasSetNewDest = true;
        currentDestination++;
        ThisNavMesh.destination = ListOfPathNodes[currentDestination].position;
        yield return new WaitForSeconds(time);
        hasSetNewDest = false;
    }
}
