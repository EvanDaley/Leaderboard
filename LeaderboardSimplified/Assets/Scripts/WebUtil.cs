using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;

		// TODO: USE SALT TO CONNECT TO DATABASE ON POST.PHP
		// CHANGE RETRIEVE BACK FROM GET TO POST

public class WebUtil : MonoBehaviour {

	public Text loadingText;
	public Button buttonSubmit;

	public Text nameInputField;
	public Text scoreInputField;

	public string pName;
	public string pScore;

	private string leaderboardPost = "http://evandaley.net/unity/leaderboard/post.php";
	private string leaderboardRetrieve = "http://evandaley.net/unity/leaderboard/retrieve.php";
	private string leaderboardCount = "http://evandaley.net/unity/leaderboard/count.php";
	private string leaderboardRank = "http://evandaley.net/unity/leaderboard/rank.php";

	public ArrayList listOfScores;

	public GameObject dynamicGrid;
	public GameObject entryPrefab;

	public Color colorOfMostRecentEntry;
	public Color colorOfPlayersEntries;

	public int count = 0;
	public int curRank = 0;

	public void SubmitAndShow()
	{
		// empty leaderboard and indicate that we are loading scores
		EraseBoard();

		// enable UI

		// submit score

		// check rank

		// if rank is in top 10: show top 15
		// use: www form with fields
		// LOWER: lower=1
		// UPPER: upper=15

		if(curRank < 10)
			StartCoroutine(GetScoresInRankRange(1,15));
		//else
			
		// else 
		// {

		// get top 5

		// get 2 before player and 7 after

		// }

		// once everything else has finished, display all the data we downloaded
	}

	IEnumerator GetScoresInRankRange(int lowerBound, int upperBound)
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("lower", lowerBound);
		form.AddField ("upper", upperBound);

		// submit the form
		string url = leaderboardRetrieve;
		WWW w = new WWW (url, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished downloading top 5 scores from " + url);
			print ("Result: \n" + w.text);
		}
	}


	// list maintanance
	public List<GameObject> concreteEntries = new List<GameObject>();	// the gameobjects being instantiated on the menu
	public List<ScoreEntry> dataItems = new List<ScoreEntry> ();		// the data downloaded from the server

	public void PopulateBoard()
	{
		// iterate thru list and create entries for each item
	}

	public void EraseBoard()
	{
		// erase all items on the list
	}

}

public class ScoreEntry
{
	public string pRank;
	public string pName;
	public string pScore;
}