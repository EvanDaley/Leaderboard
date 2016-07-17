using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebUtil : MonoBehaviour {

	public Text echo;

	public Text nameInputField;
	public Text scoreInputField;

	public string pName;
	public string pScore;
	public float gameDuration;
	public string mapName = "Map1";

	public string leaderboardURL = "http://evandaley.net/unity/leaderboard/echo.php";

	void Start () {
	
	}
	
	void Update () {
	
	}

	public void SubmitScore()
	{
		pName = nameInputField.text;
		pScore = scoreInputField.text;
		print (pName + " " + pScore);

		StartCoroutine (UploadScore ());
	}

	IEnumerator UploadScore()
	{
		// check how many units they have killed
		// tally up the score and if its off by more than 1000 dont post the score

		// wait until the end of the frame to upload the score
		yield return new WaitForEndOfFrame ();

		// create a web form
		WWWForm form = new WWWForm();
		form.AddField ("pName", pName);
		form.AddField ("pScore", pScore);
		form.AddField ("map", mapName);
		form.AddField ("duration", 100);

		// possible tool for hack-proofing
		// form.AddBinaryData("fileUpload", bytes, "screenshot.png". image/png");

		WWW w = new WWW (leaderboardURL, form);
		yield return w;
		if (!string.IsNullOrEmpty (w.error)) {
			print (w.error);
		} else {
			print ("Finished Uploading Scores");
			parseText (w.text);
		}
	}

	public void parseText(string text)
	{
		
	}
}
