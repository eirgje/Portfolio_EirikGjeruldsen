using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HiScoreTextScript : MonoBehaviour
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
		text.text = "Hi-Score: " + PlayerPrefs.GetInt("hiScore", 0);
	}
}