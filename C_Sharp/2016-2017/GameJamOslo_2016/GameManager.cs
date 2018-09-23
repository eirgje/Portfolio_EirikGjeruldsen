using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
	public bool isPlaying = true;

    [Header("Level Settings")]
    public float time;
	public float countdownFloat;
	public int countdownInt;
	[Space(10)]
	public int chickenTimerMin;
	public int chickenTimerMax;

    [Header("Player Attributes")]
    public int score;
	public int baseScorePerChicken;
    public int lives;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		countdownFloat = time;

		DontDestroyOnLoad (GameObject.Find ("Canvas"));
		DontDestroyOnLoad (this);
    }

    void Update()
    {
		// Counts down time
		if (countdownFloat > 0) {
			countdownFloat -= Time.deltaTime;
			countdownInt = (int)countdownFloat;
		} else
			countdownFloat = 0;
    }
}