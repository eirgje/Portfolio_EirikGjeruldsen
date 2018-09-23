using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCastleScript : MonoBehaviour {
    [Range(1, 2)]
    [SerializeField]
    private int SideOfTheWorld = 2;
    void OnTriggerStay(Collider other) {

        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && SideOfTheWorld == 2) {
            SceneManager.LoadSceneAsync(2);
        }
        else if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && SideOfTheWorld == 1) {
            SceneManager.LoadSceneAsync(4);
        }
    }
}
