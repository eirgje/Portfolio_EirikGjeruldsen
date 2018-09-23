using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumsliderPickup : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }
}
