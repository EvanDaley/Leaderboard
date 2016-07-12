using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	public Text scoreText;
	public Text nameText;
	public static ScoreUI Instance;

	void Awake () 
	{
		Instance = this;
	}
	
	public void UpdateScore()
	{
		scoreText.text = ScoreController.Instance.Score.ToString();
		nameText.text = GameController.Instance.PlayerName;
	}

	public void ShowScores()
	{
		// show scores prefab

		// 
	}
}
