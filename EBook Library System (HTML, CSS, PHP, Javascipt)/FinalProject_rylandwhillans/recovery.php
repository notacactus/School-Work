<!--
COIS 3420 Account Recovery Page
Author: Ryland Whillans

Allows the user to request a password reset via email
self-processing to validate email
-->
<?php

  // if user cancels return to login page
  if (isset($_POST['cancel'])) {
      header('Location: account.php');
      exit();
  }

  include "includes/pdo.php";
  session_start();
  $email = &$_SESSION['email']; //email from login page
  $error = false; // flag for incorrect email

  // if recover selected check if email valid and redirect to recovery confirmation/processing
  if (isset($_POST['recover'])) {
      $pdo=connectdb();
      $email=$_POST['email'];
      // check if account exists and set error if no
      $stmt = $pdo->prepare("SELECT id FROM proj_users WHERE email = ?");
      $stmt->execute([$email]);
      if ($stmt->rowcount() == 0) {
          $error = true;
      }
      // if no errors redirect to confirmation/processing
      if (!$error) {
          $_SESSION['reco_id'] = ($stmt->fetch())['id'];
          $_SESSION['reco_mail'] = $email;
          header('Location: resetconfirm.php');
          exit();
      }
  }
?>
<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>Account Recovery</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <h1>Account Recovery</h1>
  <form id="recovery" action="recovery.php" method="post">
    <div>
      <!-- email input -->
      <label for="email">Email Address:</label>
      <input type="email" name="email" id="email" value="<?= $email ?>" required />
      <span class="<?= $error ? "" : "no" ?>error">There is no account associated with this email address</span>
    </div>
    <!-- options to request recovery or cancel -->
    <input type="submit" name="recover" value="Recover Password" />
    <input type="submit" name="cancel" value="Cancel" formnovalidate>
  </form>
</body>

</html>
