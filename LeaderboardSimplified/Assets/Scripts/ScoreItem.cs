using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreItem : MonoBehaviour {

	public Text rankText;
	public Text nameText;
	public Text scoreText;

	public Image backgroundImage;

	public void Initialize(string rank, string pName, string pScore)
	{
		rankText.text = rank;
		nameText.text = pName;
		scoreText.text = pScore;
	}

	public void SetColor(Color color)
	{
		backgroundImage.color = color;
	}
}
