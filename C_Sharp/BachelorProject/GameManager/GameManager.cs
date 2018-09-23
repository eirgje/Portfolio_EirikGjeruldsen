using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool testingMode = false;

    [SerializeField]
    private static int collected_pickups = 0;

    public int Get_collectedPickUps() { return collected_pickups; }
    public void Add_collectedPickUp() { collected_pickups++; }

    [SerializeField]
    private static int max_pickups = 0;
    public int Get_maxPickUps() { return max_pickups; }
    public void Add_maxPickUp(int amount) { max_pickups += amount; }

    public void ResetPickUps()
    {
        max_pickups = 0;
        collected_pickups = 0;
    }

    private void Awake()
    {
        if (testingMode)
        {
            collected_pickups = 50;
            max_pickups = 105;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
            ResetPickUps();
    }

}
