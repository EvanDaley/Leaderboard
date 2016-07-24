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

	private string leaderboardURLGetTop5 = "http://evandaley.net/unity/leaderboard/getTop5.php";
	private string leaderboardURLGetCount = "http://evandaley.net/unity/leaderboard/getCount.php";
	private string leaderboardURLGetRank = "http://evandaley.net/unity/leaderboard/getRank.php";
	private string leaderboardURLGetBottom3 = "http://evandaley.net/unity/leaderboard/getBottom3.php";


	public ArrayList listOfScores;

	public GameObject dynamicGrid;
	public GameObject entryPrefab;

	public Color colorOfMostRecentEntry;
	public Color colorOfPlayersEntries;

	public List<GameObject> entries = new List<GameObject>();

	public int count = 0;
	public int curRank = 0;

	public void SubmitScore()
	{
		//buttonSubmit.interactable = false;

		// grab the name and score that we entered
		pName = nameInputField.text;
		pScore = scoreInputField.text;

		EmptyList ();

		echo.text = "Loading scores...";
		StartCoroutine (UploadAndRetrieveTopScores ());
	}
		
	IEnumerator UploadAndRetrieveTopScores()
	{
		if (!isScoreReasonable (pScore)) 
		{
			Application.Quit ();

			// just for good measure, lets make sure the rest of this method doesn't run before the application quits
			yield return new WaitForSeconds(100000f);
		}

		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);
 
		// submit the form
		WWW w = new WWW (leaderboardURLGetTop5, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished downloading top 5 scores from " + leaderboardURLGetTop5);
			print (w.text);

			// read the returned JSON
			ParseTop5 (w.text);

			echo.text = "";
		}


		StartCoroutine (GetCount ());
	}

	IEnumerator GetCount()
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);

		// submit the form
		WWW w = new WWW (leaderboardURLGetCount, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Recieved count from " + leaderboardURLGetCount + " (" + w.text + ")");

			int.TryParse (w.text, out count);
		}
			
		// start next sequence
		StartCoroutine (GetCurrentRank ());
	}
		
	IEnumerator GetCurrentRank()
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);

		// submit the form
		string url = leaderboardURLGetRank + "?pScore=" + pScore.ToString();
		WWW w = new WWW (url, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished downloading top 5 scores from " + url);
			print ("Rank: " + w.text);

			int.TryParse (w.text, out curRank);

			// create entry
			CreateEntry(curRank.ToString(), pName, pScore, colorOfMostRecentEntry);
		}

		// start next sequence
		StartCoroutine (GetBottomScores ());
	}

	// create an entry in the leaderboard UI
	void CreateEntry(string pRank, string pName, string pScore, Color color)
	{
		GameObject entryInstance = GameObject.Instantiate (entryPrefab, transform.position, transform.rotation) as GameObject;
		entryInstance.transform.SetParent (dynamicGrid.transform, false);

		ScoreItem item = entryInstance.GetComponent<ScoreItem> ();
		item.Initialize (pRank, pName, pScore);
		item.SetColor (color);

		entries.Add (entryInstance);
	}

	IEnumerator GetBottomScores()
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);

		// submit the form
		WWW w = new WWW (leaderboardURLGetBottom3, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished downloading top 5 scores from " + leaderboardURLGetBottom3);
			print (w.text);

			// read the returned JSON
			ParseBottom3 (w.text);
		}

		// start next sequence
	}

	public void ParseTop5(string text)
	{
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

	public void ParseBottom3(string text)
	{
		// get the last three results and parse them
		var objList = JsonConvert.DeserializeObject<List<ScoreEntry>> (text);

		int i = count;

		//foreach (var entry in objList)

		for(int j = objList.Count - 1; j > -1; j--)
		{
			var entry = objList [j];

			GameObject entryInstance = GameObject.Instantiate (entryPrefab, transform.position, transform.rotation) as GameObject;
			entryInstance.transform.SetParent (dynamicGrid.transform, false);

			ScoreItem item = entryInstance.GetComponent<ScoreItem> ();
			item.Initialize (i.ToString(), entry.pName, entry.pScore);

			if(pName == entry.pName)
				item.SetColor (colorOfPlayersEntries);

			if(pName == entry.pName && pScore == entry.pScore)
				item.SetColor (colorOfMostRecentEntry);

			entries.Add (entryInstance);
		}
	}


	// parse the JSON returned from the server
	public void parseText(string text)
	{
		EmptyList ();

		var objList = JsonConvert.DeserializeObject<List<ScoreEntry>> (text);

		foreach (var entry in objList)
		{
			GameObject entryInstance = GameObject.Instantiate (entryPrefab, transform.position, transform.rotation) as GameObject;
			entryInstance.transform.SetParent (dynamicGrid.transform, false);
			                                                                                               
			ScoreItem item = entryInstance.GetComponent<ScoreItem> ();
			item.Initialize (entry.pRank, entry.pName, entry.pScore);

		

			if(pName == entry.pName)
				item.SetColor (colorOfPlayersEntries);

			if(pName == entry.pName && pScore == entry.pScore)
				item.SetColor (colorOfMostRecentEntry);

			entries.Add (entryInstance);
		}
	}




	//LIST MAINTANANCE

	// TODO: I need to download everything and put it in a list
	// add the current entry last and if it is ahead of anything else push the others down and 
	// modify their ranks!


	// TODO: get rank of last entry instead of simply using the count

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
	public bool isScoreReasonable(string pScore)
	{
		int score = 0;
		int.TryParse (pScore, out score);

		if (score < 0)
			return false;

		// check how many units they have killed
		// tally up the score and if its off by more than 1000 dont post the score
		return true;
	}


}

public class ScoreEntry
{
	public string pRank;
	public string pName;
	public string pScore;
}