<!--
COIS 3420 Project Account Deletion Page
Author: Ryland Whillans

Page to provide confirmation when users wish to delete their account and to remove all account data if so
-->
<?php
  // check if logged in
  include "includes/logincheck.php";

  // if user cancels deletion return to account page
  if (isset($_POST['cancel'])) {
      header('Location: account.php');
      exit();
  }

  $error = false; //flag if password invalid
  // if user confirms account deletion check if password valid and delete account
  if (isset($_POST['confirm'])) {
      $id = $_SESSION['user'];
      $password = $_POST['password'] ?? "";
      // load pass hash from database and check against entered password
      $stmt = $pdo->prepare("SELECT passhash FROM proj_users WHERE id = ?");
      $stmt->execute([$id]);
      $row = $stmt->fetch();
      // if password not correct set error flag
      if (!password_verify($password, $row['passhash'])) {
          $error = true;
      // if password is correct delete account
      } else {
          // delete user from users table, will cascade to all other tables
          $sql = "DELETE FROM proj_users WHERE id = ?";
          $pdo->prepare($sql)->execute([$id]);
          // construct path to directory for users files
          $path = "/home/rylandwhillans/public_html/www_data/" . $_SESSION['user'] . "/";
          $books = glob($path . "*");
          // loop through folders for books and delete each file then delte directory
          foreach ($books as $book) {
              $files = glob($book . "/*");
              foreach ($files as $file) {
                  echo $file . "<br>";
                  unlink($file);
              }
              echo "<br>" . $book . "<br>";
              rmdir($book);
          }
          // once empty delete user's main directory and redirect to logout
          rmdir($path);
          header('Location: logout.php');
          exit();
      }
  }
?>
<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>Delete Account</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <!-- include nav -->
  <?php include "includes/nav.php"; ?>

  <!-- confirm that user wishes to delte account -->
  <h1>Are you sure you wish to delete your account?</h1>
  <span>Enter your password and press confirm to delete your account</span>
  <form id="deleteaccount" action="deleteaccount.php" method="post">
    <!-- get password -->
    <div>
      <label for="password">Password:</label>
      <input type="password" name="password" value="" required>
      <span class="<?= $error ? "" : "no" ?>error">Incorrect Password</span>
    </div>
    <!-- confirm to delete or cancel to stop -->
    <input type="submit" name="confirm" value="Confirm">
    <input type="submit" name="cancel" value="Cancel" formnovalidate>
  </form>
</body>

</html>
