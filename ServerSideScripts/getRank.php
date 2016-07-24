<?php
	include_once("includeMe.php");
	include_once("log.php");

	$pScore = htmlspecialchars($_GET["pScore"]);

	$mysqli = new mysqli($host, $user , $pw, $db);
	if ($mysqli->connect_errno) 
	{
		echo "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error;
	}
	
	$mysqli->query("SET @newScore = ". "'" . $mysqli->real_escape_string($pScore) . "'");
	
	$result = $mysqli->query("CALL GetRank(@newScore)");
	
	if(!$result)
	{
		echo "failed";
		writeLog("Call to GetAllNames failed");
	}
	else
	{
		writeLog("Call to GetAllNames succeeded");
	}
	
	if ($result->num_rows > 0) 
	{
		$summation = array();
		
		// store each line in an array and then push it into 'summation'
		while($row = $result->fetch_assoc())
		{
			$entry = array(
				"pRank"=>$row["pRank"],
			);
			array_push($summation, $entry);
		}
		
		// take the giant array we just built and encode it into json, then echo it
		$output = json_encode($summation);
		echo $output;
	} 
	else
	{
		echo "0 results";
	}
	
	$mysqli ->close();
	
?>