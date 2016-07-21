<?php

include 'includeMe.php';

echo $user;

$query = "SELECT * FROM Leaderboard";

$link = mysql_connect ($server, $user, $password);
if (!$link)
{
	die('ERROR: Could not connect: ' . mysql_error());
}

$db_selected = mysql_select_db($database, $link);
if (!$db_selected)
{
	do_close($link);
	die('ERROR: Could not select database: ' . $database . ': error = ' . mysql_error());
}

$result = mysql_query($query, $link);
if (!$result)
{
	$message = 'ERROR: ' . mysql_error();
	die($message);
}

while ($row = mysql_fetch_row($result)) 
{
	$count = count($row);
	$y = 0;
	
	$summation = array();
	
	if($count > 0)
	{
		while ($y < $count)
		{	
			$entry = array(
				"pName"=>current($row["pName"]),
				"pScore"=>current($row["pScore"]),
				"entryID"=>current($row["entryID"]),
				"entryDate"=>current($row["entryDate"]),
			);
				
			array_push($summation, $entry);
			
			echo current($row);
			next($row);
			$y = $y + 1;
			
			
		}
		
		// take the giant array we just built and encode it into json, then echo it
		$output = json_encode($summation);
		echo $output;
	}
}

mysql_free_result($result);
do_close($link);


function do_close($link)
{
	mysql_close($link);
}
?>