using UnityEngine;
using System.Collections;

public class ChickenAssignTimers : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject[] chickenlist;
    private float[] timerList;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        chickenlist = new GameObject[transform.childCount];
        timerList = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            chickenlist[i] = transform.GetChild(i).gameObject;
            timerList[i] = gameManager.chickenTimerMin + i * (gameManager.chickenTimerMax - gameManager.chickenTimerMin) / (transform.childCount - 1);
        }

        foreach (GameObject chicken in chickenlist)
        {
            int r = Random.Range(0, 8);
            while (timerList[r] == 0)
                r = Random.Range(0, 8);

            if (timerList[r] != 0)
            {
                chicken.GetComponent<ChickenSplat>().timer = timerList[r];
                timerList[r] = 0;
            }
        }
    }

    void Update()
    {

    }
}