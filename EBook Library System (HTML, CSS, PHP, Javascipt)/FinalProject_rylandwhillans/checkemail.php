<?php
/*
Author(s): Ryland Whillans
checks if email is in use for use with ajax call
based on lab 10
*/
// check if passed email is in use and return result
  include 'includes/pdo.php';
  $pdo =  connectdb();
  $stmt = $pdo->prepare('SELECT 1 FROM proj_users WHERE email = ?');
  $stmt->execute([$_GET['email']]);
  echo $stmt->rowcount() > 0;
?>
