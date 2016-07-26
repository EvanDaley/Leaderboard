using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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

	public void SubmitScore()
	{
<<<<<<< HEAD
		pName = nameInputField.text;
		pScore = scoreInputField.text;
=======
		// empty leaderboard and indicate that we are loading scores
		EraseBoard();
>>>>>>> origin/master

		// empty leaderboard and indicate that we are loading scores
		EraseBoard();

		// submit score
		StartCoroutine(PostScore());

		Invoke ("PopulateBoard", 2f);
	}

<<<<<<< HEAD
	IEnumerator PostScore()
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);

		// submit the form
		string url = leaderboardPost;
		WWW w = new WWW (url, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			//print ("Finished uploading score to " + url);
			//print ("Result: \n" + w.text);
		}
=======
		// if rank is in top 10: show top 15
		// use: www form with fields
		// LOWER: lower=1
		// UPPER: upper=15

		if(curRank < 10)
			StartCoroutine(GetScoresInRankRange(1,15));
		//else
			
		// else 
		// {
>>>>>>> origin/master

		// check rank
		StartCoroutine(GetRank());
	}

	IEnumerator GetRank()
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pScore", pScore);

<<<<<<< HEAD
		// submit the form
		string url = leaderboardRank;
		WWW w = new WWW (url, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			//print ("Finished downloading rank from " + url);
			print ("Rank: \n" + w.text);

			int.TryParse (w.text, out curRank);

			if (curRank < 10)
			{
				StartCoroutine(GetScoresInRankRange(6,15));
			} else
			{
				StartCoroutine(GetScoresInRankRange(curRank - 4, curRank + 15));
			}
		}
=======
		// }

		// once everything else has finished, display all the data we downloaded
>>>>>>> origin/master
	}
		
	IEnumerator GetScoresInRankRange(int lowerBound, int upperBound)
	{
		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("lower", 1);
		form.AddField ("upper", 5);

		// submit the form
		string url = leaderboardRetrieve;
		WWW w = new WWW (url, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			//print ("Finished downloading top 5 scores from " + url);
			print ("Result: \n" + w.text);

			ParseJSON (w.text);
		}

		// create a web form
		form = new WWWForm();
		form.AddField ("lower", lowerBound);
		form.AddField ("upper", upperBound);

		// submit the form
		w = new WWW (url, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			//print ("Finished downloading top 5 scores from " + url);
			print ("Result: \n" + w.text);

			ParseJSON (w.text);
		}
	}

	public void ParseJSON(string text)
	{
		var jsonList = JsonConvert.DeserializeObject<List<DataItem>> (text);

		foreach (var entry in jsonList)
		{
			DataItem newEntry = new DataItem (entry.pRank, entry.pName, entry.pScore);
			dataItems.Add (newEntry);
		}
	}

	// list maintanance
	public List<GameObject> concreteEntries = new List<GameObject>();	// the gameobjects being instantiated on the menu
	public List<DataItem> dataItems = new List<DataItem> ();		// the data downloaded from the server

	public void PopulateBoard()
	{
		// erase all items on the list
		loadingText.gameObject.SetActive(false);

		if (dataItems.Count == 0)
		{
			// instantiate prefab button that says failed to load, retry?
		}

		// sort the list based on rank
		//dataItems = dataItems.OrderByDescending(o=>o.pScore).ToList();

		print (dataItems.Count);

		// iterate thru list and create entries for each item
		foreach (DataItem dataItem in dataItems)
		{
			print (dataItem.pRank + " " + dataItem.pName + " " + dataItem.pScore);
			CreateConcreteEntry (dataItem);
		}
	}

	public void CreateConcreteEntry(DataItem entry)
	{
		GameObject entryInstance = GameObject.Instantiate (entryPrefab, transform.position, transform.rotation) as GameObject;
		entryInstance.transform.SetParent (dynamicGrid.transform, false);

		ScoreItem item = entryInstance.GetComponent<ScoreItem> ();
		item.Initialize (entry.pRank, entry.pName, entry.pScore);

		if(pName == entry.pName)
			item.SetColor (colorOfPlayersEntries);

		if(pName == entry.pName && pScore == entry.pScore)
			item.SetColor (colorOfMostRecentEntry);

		concreteEntries.Add (entryInstance);
	}

	public void EraseBoard()
	{
		// erase all items on the list
		loadingText.gameObject.SetActive(true);
		loadingText.text = "Loading scores...";

		foreach (GameObject entry in concreteEntries)
		{
			Destroy (entry);
		}

		concreteEntries.Clear ();
		dataItems.Clear ();
	}

}

public class DataItem
{
	public string pRank;
	public string pName;
	public string pScore;

	public DataItem (string pRank, string pName, string pScore)
	{
		this.pRank = pRank;
		this.pName = pName;
		this.pScore = pScore;
	}
	
}