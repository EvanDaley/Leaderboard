using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;

public class WebUtil : MonoBehaviour {

	public Text echo;

	public Button buttonSubmit;

	public Text nameInputField;
	public Text scoreInputField;

	public string pName;
	public string pScore;
	public string gameDuration;
	public string mapName = "Map1";

	private string leaderboardURLGet = "http://evandaley.net/unity/leaderboard/get.php";

	public ArrayList listOfScores;

	public GameObject dynamicGrid;
	public GameObject entryPrefab;

	public Color colorOfMostRecentEntry;
	public Color colorOfPlayersEntries;

	public List<GameObject> entries = new List<GameObject>();

	public void SubmitScore()
	{
		//buttonSubmit.interactable = false;

		// grab the name and score that we entered
		pName = nameInputField.text;
		pScore = scoreInputField.text;

		echo.text = "Loading scores...";
		StartCoroutine (UploadAndRetrieveScore ());
	}
		
	IEnumerator UploadAndRetrieveScore()
	{
		if (!isScoreReasonable ()) 
		{
			Application.Quit ();

			// just for good measure, lets make sure the rest of this method doesn't run before the application quits
			yield return new WaitForSeconds(100000f);
		}

		// wait until the end of the frame to upload the score
		yield return new WaitForEndOfFrame ();

		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);
 
		// submit the form
		WWW w = new WWW (leaderboardURLGet, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished downloading scores from " + leaderboardURLGet);
			print (w.text);

			// read the returned JSON
			parseText (w.text);

			echo.text = "";
		}
	}

	// parse the JSON returned from the server
	public void parseText(string text)
	{
		EmptyList ();

		var objList = JsonConvert.DeserializeObject<List<ScoreEntry>> (text);

		int i = 1;

		foreach (var entry in objList)
		{
			GameObject entryInstance = GameObject.Instantiate (entryPrefab, transform.position, transform.rotation) as GameObject;
			entryInstance.transform.SetParent (dynamicGrid.transform, false);
			                                                                                               
			ScoreItem item = entryInstance.GetComponent<ScoreItem> ();
			item.Initialize (i.ToString(), entry.pName, entry.pScore);

			if(pName == entry.pName)
				item.SetColor (colorOfPlayersEntries);

			if(pName == entry.pName && pScore == entry.pScore)
				item.SetColor (colorOfMostRecentEntry);

			entries.Add (entryInstance);

			i++;
		}

	}

	void EmptyList()
	{
		foreach (GameObject obj in entries)
		{
			Destroy (obj);
		}

		entries.Clear ();
	}



	/// <summary>
	/// A simple method for validating whether the score to submit is valid. Checks if the score
	/// is disproportional to the number of kills and duration of game.
	/// </summary>
	/// <returns><c>true</c>, if score reasonable was reasonable, <c>false</c> otherwise.</returns>
	public bool isScoreReasonable()
	{
		// check how many units they have killed
		// tally up the score and if its off by more than 1000 dont post the score
		return true;
	}


}

public class ScoreEntry
{
	public string rank;
	public string pName;
	public string pScore;
}