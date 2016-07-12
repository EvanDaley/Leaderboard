using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public Button goButton;
	public Text pNameText;

	public bool nameAvailable
	{
		get
		{ 
			return true;
		}
	}

	void Update () 
	{
		if (nameAvailable)
			goButton.interactable = true;
		else
			goButton.interactable = false;
	}

	public void StartGame()
	{
		GameController.Instance.GameState = GameState.combat;
		GameController.Instance.PlayerName = pNameText.text;
		SceneManager.LoadScene (1);
	}
}
