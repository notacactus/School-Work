<!--
COIS 3420 Project Password Change Page
Author: Ryland Whillans

Page to allow users who have requested a password reset to change password after receiving a token via email
-->
<?php
  include "includes/pdo.php";
  session_start();
  $pdo=connectdb();
  $errors = array();  // validation errors
  // if token in url, validate token
  if (isset($_GET['token'])) {
      $_SESSION['token'] = $_GET['token'];
      // check if token valid
      $stmt = $pdo->prepare("SELECT fk_userid, timeout FROM proj_reset_requests WHERE token = ?");
      $stmt->execute([$_SESSION['token']]);
      // if no token found set error
      if ($stmt->rowcount() == 0) {
          $errors['no_token'] = true;
      // if token expired set error
      } else {
          $row = $stmt->fetch();
          $_SESSION['reset_id'] = $row['fk_userid'];
          $timeout = $row['timeout'];
          if ((int)$timeout < time()) {
              $errors['old_token'] = true;
          }
      }
      // if token is valid and user has submitted form, validate password and update account
  } elseif (isset($_POST['passreset']) && isset($_SESSION['reset_id'])) {
      $password = $_POST['password'];
      $password2 = $_POST['password2'];
      // check if password to short
      if (strlen($password) < 8) {
          $errors['pass_valid'] = "Password must be at least 8 characters long";
      // check if countains required characters
      } elseif (!preg_match("~\A(?=\D*\d)(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\w*\W)~", $password)) {
          $errors['pass_valid'] = "Password must contain at least one uppercase letter, lowercase letter, number, and special character";
      }
      // check if passwords match
      if ($password != $password2) {
          $errors['pass_match'] = "Passwords do not match";
      }
      // if no errors update password
      if (empty($errors)) {
          // hash password and update account
          $hash = password_hash($password, PASSWORD_DEFAULT);
          $pdo->prepare("UPDATE proj_users SET passhash = ? WHERE id = ?")->execute([$hash, $_SESSION['reset_id']]);
          // clear all reset requests and persistent sessions
          $pdo->prepare("DELETE FROM proj_reset_requests WHERE id = ?")->execute([$_SESSION['reset_id']]);
          $pdo->prepare("DELETE FROM proj_persistent_sessions WHERE id = ?")->execute([$_SESSION['reset_id']]);
          // log user in and redirect to library
          $_SESSION['user'] = $row['id'];
          $stmt = $pdo->prepare("SELECT email FROM proj_users WHERE id = ?");
          $stmt->execute([$row['id']]);
          $_SESSION['email'] = $stmt->fetch()['email'];
          header('Location: library.php');
          exit();
      }
      // if arriving without tokenm redirect to login
  } else {
      header('Location: login.php');
      exit();
  }
?>
<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>Password Reset</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <h1>Title</h1>
  <!-- erros for invalid tokens -->
  <?php if (isset($errors['no_token'])): ?>
  <p>Invalid token for password reset, please ensure that the URL is correct or <a href="recovery.php">send new reset request</a></p>
  <?php elseif (isset($errors['old_token'])): ?>
  <p>This reset link has expired</p>
  <a href="recovery.php">Send new reset request</a>
  <?php else: ?>
  <!-- form to enter new password if token valid -->
  <form id="passreset" action="passreset.php" method="post">
    <!-- password -->
    <div>
      <label for="password">New Password:</label>
      <input type="password" name="password" id="password" value="" required />
      <span>Password must be at least 8 characters long and contain at least one uppercase letter, lowercase letter, number, and special character</span>
      <span class="<?= isset($errors['pass_valid']) ? "" : "no" ?>error"><?= $errors['pass_valid'] ?? "" ?></span>
    </div>
    <!-- password confirmation -->
    <div>
      <label for="password2">Confirm Password:</label>
      <input type="password" name="password2" id="password2" value="" required />
      <span class="<?= isset($errors['pass_match']) ? "" : "no" ?>error"><?= $errors['pass_match'] ?? "" ?></span>
    </div>
    <input type="submit" name="passreset" value="Change Password" />
  </form>
  <?php endif; ?>

  <!-- include js files -->
  <?php include 'includes/js_includes.php';?>
  <script src="js/reset_scripts.js"></script>
</body>

</html>
