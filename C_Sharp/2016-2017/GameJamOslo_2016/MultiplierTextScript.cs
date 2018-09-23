using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplierTextScript : MonoBehaviour
{
    private Text text;
    public GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "x" + gameManager.countdownInt;
    }
}