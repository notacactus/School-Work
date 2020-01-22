
<?php

// connect to database
$con=mysqli_connect("localhost","root","","inventory_management");
if (mysqli_connect_errno($con))
{
	echo "Failed to connect to MySQL: " . mysqli_connect_error();
}
else
{
	echo "Success; you are connected"; ?> <br> <?php
}

// list of all table names
$tableNames = array("customer", "customer_order", "order_details", "product", "product_location", "warehouse", "supplier", "supplier_shipment", "shipment_details");
$numTables = count($tableNames);
// iterate through tables and export each table as a csv file
for ($i = 0; $i < $numTables; $i++)
{
	$sql = "SELECT * FROM {$tableNames[$i]}
	INTO OUTFILE '{$tableNames[$i]}.csv'
	FIELDS TERMINATED BY ','
	OPTIONALLY ENCLOSED BY '\"'
    LINES TERMINATED BY '\\n'";
	
	if (mysqli_query($con,$sql))
	{
		echo "Data submitted successfully"; 
	}
	else
	{
		echo "Error submitting the data into the backend " . mysqli_error($con);
	}	
}
// close connection
mysqli_close($con);
?>