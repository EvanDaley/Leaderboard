using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	private int score = 0;
	public int Score
	{
		get
		{ 
			return score;	
		}

		set
		{ 
			score = value;
			ScoreUI.Instance.UpdateScore ();
		}
	}

	public static ScoreController Instance;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		ScoreUI.Instance.UpdateScore ();
	}
}
