<!--
COIS 3420 Project Registration Page
Author: Ryland Whillans

Page to allow account creation
Self-processing to validate input and create account
-->
<?php
  // if user cancels return to login page
  if (isset($_POST['cancel'])) {
      header('Location: account.php');
      exit();
  }

  include "includes/pdo.php";
  session_start();

  $email = &$_SESSION['email'];
  $password = $_POST['password'] ?? "";
  $password2 = $_POST['password2'] ?? "";
  $errors = array();  // validation errors

  // if submitted process form
  if (isset($_POST['register'])) {
      $pdo=connectdb();

      $email=$_POST['email'];
      // check if email is valid format and not too long
      if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
          $errors['email_valid'] = "Invalid Email";
      } elseif (strlen($email) > 254) {
          $errors['email_valid'] = "Email must not exceed 254 characters";
      // check if email already in use
      } else {
          $stmt = $pdo->prepare("SELECT 1 FROM proj_users WHERE email = ?");
          $stmt->execute([$email]);
          if ($stmt->rowcount() > 0) {
              $errors['email_valid'] = "Email already in use";
          }
      }
      // check if password long enough and contains required characters
      if (strlen($password) < 8) {
          $errors['pass_valid'] = "Password is not long enough";
      } elseif (!preg_match("~\A(?=\D*\d)(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\w*\W)~", $password)) {
          $errors['pass_valid'] = "Password does not contain required characters";
      }
      // check if passwords match
      if ($password != $password2) {
          $errors['pass_match'] = "Passwords do not match";
      }
      // if no errors create account and log user in
      if (empty($errors)) {
          // hash password and insert with email into database
          $hash = password_hash($password, PASSWORD_DEFAULT);
          $pdo->prepare("INSERT INTO proj_users (email, passhash) VALUES (?, ?)")->execute([$email, $hash]);
          // log user in and redirect to library
          $_SESSION['user'] = $pdo->lastInsertId();
          $_SESSION['email'] = $email;
          header('Location: library.php');
          exit();
      }
  }
?>

<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>Account Creation</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <h1>Account Creation</h1>
  <!-- account creation form -->
  <form id="register" action="register.php" method="post">
    <!-- email input -->
    <div>
      <label for="email">Email Address:</label>
      <input type="email" name="email" id="email" value="<?= $email ?>" required />
      <span class="<?= isset($errors['email_valid']) ? "" : "no" ?>error"><?= $errors['email_valid'] ?? "" ?></span>
    </div>
    <!-- password input -->
    <div>
      <label for="password">Password:</label>
      <input type="password" name="password" id="password" value="" required />
      <span>Password must be at least 8 characters long and contain at least one uppercase letter, lowercase letter, number, and special character</span>
      <span class="<?= isset($errors['pass_valid']) ? "" : "no" ?>error"><?= $errors['pass_valid'] ?? "" ?></span>
    </div>
    <!-- pasword confirmation -->
    <div>
      <label for="password2">Confirm Password:</label>
      <input type="password" name="password2" id="password2" value="" required />
      <span class="<?= isset($errors['pass_match']) ? "" : "no" ?>error"><?= $errors['pass_match'] ?? "" ?></span>
    </div>
    <!-- option to submit registration or retunr to login -->
    <input type="submit" name="register" value="Sign Up" />
    <input type="submit" name="cancel" value="Cancel" formnovalidate>
  </form>

  <!-- include js files -->
  <?php include 'includes/js_includes.php';?>
  <script src="js/register_scripts.js"></script>
</body>

</html>
