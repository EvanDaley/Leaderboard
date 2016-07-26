<?php
	include_once("includeMe.php");
	include_once("log.php");

	$pName = htmlspecialchars($_POST["pName"]);
	$pScore = htmlspecialchars($_POST["pScore"]);
	
	$mysqli = new mysqli($host, $user , $pw, $db);
	if ($mysqli->connect_errno) 
	{
		echo "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error;
	}
	
	$sql = $mysqli->prepare("INSERT INTO Leaderboard (pName,pScore) VALUES (?,?)");
	$sql->bind_param('ss', $pName, $pScore);

	if($sql->execute())
	{
		writeLog("Wrote ".$pName." ".$pScore." to database");
	}
	else
	{
		writeLog("Failed to write ".$pName." ".$pScore." to database");
	}
	
	// Would it be better to create a temporary table and then calculate ranks across the table with a few sql calls?
	$sql = 'SELECT * FROM Leaderboard ORDER BY pScore DESC LIMIT 5';
	
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
			$newScore = $row["pScore"];
			$pRank = file_get_contents($url);
			
			$entry = array(
				"pName"=>$row["pName"],
				"pScore"=>$newScore,
				"pRank"=>"top5"
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