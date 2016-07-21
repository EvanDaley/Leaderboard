using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebUtil : MonoBehaviour {

	public Text echo;

	public Button buttonSubmit;

	public Text nameInputField;
	public Text scoreInputField;

	public string pName;
	public string pScore;
	public string gameDuration;
	public string mapName = "Map1";

	private string leaderboardURL = "http://evandaley.net/unity/leaderboard/get.php";

	public ArrayList listOfScores;


	public void SubmitScore()
	{
		pName = nameInputField.text;
		pScore = scoreInputField.text;
		print (pName + " " + pScore);

		buttonSubmit.interactable = false;

		StartCoroutine (UploadScore ());
	}

	IEnumerator UploadScore()
	{
		if (!isScoreReasonable ()) 
		{
			Application.Quit ();

			// just for good measure, lets make sure the rest of this method doesn't run
			yield return new WaitForSeconds(10f);
		}

		// wait until the end of the frame to upload the score
		yield return new WaitForEndOfFrame ();

		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);
		form.AddField ("mapName", mapName);
		form.AddField ("duration", 100);

		// possible tool for hack-proofing
		// form.AddBinaryData("fileUpload", bytes, "screenshot.png". image/png");

		// submit the form
		WWW w = new WWW (leaderboardURL, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished Uploading Scores to " + leaderboardURL);

			// read the returned JSON
			parseText (w.text);
			print (w.text);

			echo.text = w.text;
		}
	}

	// parse the JSON returned from the server
	public void parseText(string text)
	{
		// next step: send the scores to the server and echo them back
		// next step: send the scores to the server, echo them back as json
		// next step: secho back a list of scores as json
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
	public string pName;
	public string pScore;
	public string mapName;
	public string duration;
}