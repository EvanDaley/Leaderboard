using UnityEngine;
using System.Collections;

public enum GameState
{
	startMenu,
	combat,
	highScores
}

public class GameController : MonoBehaviour {

	private string pName;
	public string PlayerName
	{
		get
		{ 
			return pName;
		}

		set
		{ 
			pName = value;
		}
	}

	public static GameController Instance;
	private GameState gameState;
	public GameState GameState
	{
		get 
		{ 
			return gameState;
		}

		set
		{ 
			gameState = value;

			if (gameState == GameState.combat)
				StartCombat ();
			
			if (gameState == GameState.highScores)
				ShowHighScores ();

		}
	}

	void Awake () 
	{
		Instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void StartCombat()
	{
		if(ScoreController.Instance != null)
			ScoreController.Instance.Score = 0;
	}

	void ShowHighScores()
	{
		if (ScoreUI.Instance != null)
			ScoreUI.Instance.ShowScores ();

		// post score
		print("Posting score");

		if (ScoresWebUtil.Instance != null)
			ScoresWebUtil.Instance.PostScore (ScoreController.Instance.Score, PlayerName, "Map1");


		// get list
		print("Getting list");


	}
	
	void Update () 
	{
		if (Input.GetButton ("Fire1") && GameState == GameState.combat)
		{
			ScoreController.Instance.Score += 1;
		}

		if (Input.GetButtonDown ("Fire2"))
		{
			GameState = GameState.highScores;
		}

		if (Input.GetKeyDown (KeyCode.R))
		{
			GameState = GameState.combat;

		}
	}
}
