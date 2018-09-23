using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject groundGeo1;
    private SnowDeform snowDeform1 = null;
    [SerializeField]
    private GameObject groundGeo2;
    private SnowDeform snowDeform2 = null;

    private void Awake()
    {
        groundGeo1 = GameObject.Find("hedgehogGround_geo");
        snowDeform1 = groundGeo1.GetComponent<SnowDeform>();
        groundGeo2 = GameObject.Find("ground_geo_new");
        snowDeform2 = groundGeo2.GetComponent<SnowDeform>();
        snowDeform2.enabled = false;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<SpawnPointManager>().SlideFinished();

            snowDeform2.enabled = true;
            snowDeform1.enabled = false;

            Destroy(gameObject);
        }
    }
}
