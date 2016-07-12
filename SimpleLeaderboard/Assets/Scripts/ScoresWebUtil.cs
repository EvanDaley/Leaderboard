using UnityEngine;
using System.Collections;

public class ScoresWebUtil : MonoBehaviour {

	public static ScoresWebUtil Instance;
	public string tableName;
	public string secretKey;

	void Awake () 
	{
		Instance = this;
	}

	public void PostScore(int score, string playerName, string mapName)
	{
		// use the secret key?

		// figure out how to send the smallest bit of data to the server and echo it back

		// echo "hello world"

		string result = "text";

		print (result);
	}
}
