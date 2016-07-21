<?php
	include_once("includeMe.php");
	include_once("log.php");

	$mysqli = new mysqli($host, $user , $pw, $db);
	if ($mysqli->connect_errno) 
	{
		echo "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error;
	}
	
	writeLog($pw);
	
	$sql = 'SELECT * FROM Leaderboard';
	
	if(!$results = $mysqli->query($sql))
	{
		die("Error running query");
	}
	else
	{
		//writeLog('SQL query successful: '.$sql);
	}
	
	if ($results->num_rows > 0) 
	{
		$summation = array();
		
		// store each line in an array and then push it into 'summation'
		while($row = $results->fetch_assoc())
		{
			$entry = array(
				"pName"=>$row["pName"],
				"pScore"=>$row["pScore"],
				"entryID"=>$row["entryID"],
				"entryDate"=>$row["entryDate"],
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