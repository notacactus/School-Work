<!--
COIS 3420 Project Account Page
Author: Ryland Whillans

Page to allow changing account details (email, password), and deleting account
Self-processing form handles changing email/password, delete redirects to confirmation page
-->
<?php
// check if logged in
  include "includes/logincheck.php";

  $errors = array(); // array for validation errors
  // if delete selected, redirect
  if (isset($_POST['delete'])) {
      header('Location: deleteaccount.php');
      exit();
  // if back selected return to base account page
  } elseif (isset($_POST['back'])) {
      header('Location: account.php');
      exit();
  // if form submitted process it
  } elseif (isset($_POST['password'])) {
      // alias user and entered password
      $id = $_SESSION['user'];
      $password = $_POST['password'];
      // retrieve password hash
      $stmt = $pdo->prepare("SELECT id, passhash FROM proj_users WHERE email = ?");
      $stmt->execute([$_SESSION['email']]);
      $row = $stmt->fetch();
      // check if password is correct
      if ($stmt->rowcount() == 0 || $row['id'] != $_SESSION['user'] || !password_verify($password, $row['passhash'])) {
          $errors['pass_correct'] = "Incorrect password";
      }

      // if email change selected, validate/update email
      if (isset($_POST['email'])) {
          $email = $_POST['email'];
          //check if email is valid format
          if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
              $errors['email_valid'] = "Invalid Email";
          // check if too long
          } elseif (strlen($email) > 254) {
              $errors['email_valid'] = "Email must not exceed 254 characters";
          } else {
              // if valid check if in use already
              $stmt = $pdo->prepare("SELECT 1 FROM proj_users WHERE email = ?");
              $stmt->execute([$email]);
              if ($stmt->rowcount() > 0) {
                  $errors['email_valid'] = "Email already in use";
              }
          }
          // if no issues update account and return to base account page
          if (empty($errors)) {
              $pdo->prepare("UPDATE proj_users SET email = ? WHERE id = ?")->execute([$email, $id]);
              $_SESSION['email'] = $email;
              header('Location: account.php');
              exit();
          }

          // if password change, validate and update password
      } elseif (isset($_POST['newpass'])) {
          $newpass = $_POST['newpass'];
          $newpass2 = $_POST['newpass2'];
          // check if password is long enough and has required characters
          if (strlen($newpass) < 8) {
              $errors['pass_valid'] = "Password is not long enough";
          } elseif (!preg_match("~\A(?=\D*\d)(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\w*\W)~", $newpass)) {
              $errors['pass_valid'] = "Password does not contain required characters";
          }
          //check if passwords match
          if ($newpass != $newpass2) {
              $errors['pass_match'] = "Passwords do not match";
          }
          // if no issues, hash new password and update account, then invalidate any reset tokens/persistent sessions and return to base account page
          if (empty($errors)) {
              $hash = password_hash($newpass, PASSWORD_DEFAULT);
              $pdo->prepare("UPDATE proj_users SET passhash = ? WHERE id = ?")->execute([$hash, $id]);
              $pdo->prepare("DELETE FROM proj_reset_requests WHERE id = ?")->execute([$id]);
              $pdo->prepare("DELETE FROM proj_persistent_sessions WHERE id = ?")->execute([$id]);
              header('Location: account.php');
              exit();
          }
      }
  }
?>
<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>My Account</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <!-- include nav -->
  <?php include "includes/nav.php"; ?>

  <!-- if on base page display options (email/pass/delete) -->
  <?php if (!isset($_POST['submit'])): ?>
  <h1>My Account</h1>
  <form id="editaccount" action="account.php" method="post">
    <input type="submit" name="submit" value="Change Email">
    <input type="submit" name="submit" value="Change Password">
    <input type="submit" name="delete" value="Delete Account">
  </form>
  <!-- if email change display form with  fields for new email and current password -->
  <?php elseif ($_POST['submit'] == "Change Email"): ?>
  <h1>Change Email Address</h1>
  <form id="editaccount" action="account.php" method="post">
    <!-- new email -->
    <div>
      <label for="email">New Email Address:</label>
      <input type="email" name="email" id="email" value="<?= $_POST['email'] ?? "" ?>" required />
      <span class="<?= isset($errors['email_valid']) ? "" : "no" ?>error"><?= $errors['email_valid'] ?? "" ?></span>
    </div>
    <!-- current password -->
    <div>
      <label for="password">Password:</label>
      <input type="password" name="password" id="password" value="" required />
      <span class="<?= isset($errors['pass_correct']) ? "" : "no" ?>error"><?= $errors['pass_correct'] ?? "" ?></span>
    </div>
    <input type="submit" name="submit" value="Change Email">
    <input type="submit" name="back" value="Back" formnovalidate>
  </form>
  <!-- if pass change display form with fileds for old pass, new pass, and new pass confirmation -->
  <?php elseif ($_POST['submit'] == "Change Password"): ?>
  <h1>Change Password</h1>
  <form id="editaccount" action="account.php" method="post">
    <!-- current password -->
    <div>
      <label for="email">Old Password:</label>
      <input type="password" name="password" id="password" value="" required />
      <span class="<?= isset($errors['pass_correct']) ? "" : "no" ?>error"><?= $errors['pass_correct'] ?? "" ?></span>
    </div>
    <!-- new password -->
    <div>
      <label for="newpass">New Password:</label>
      <input type="password" name="newpass" id="newpass" value="" required />
      <span>Password must be at least 8 characters long and contain at least one uppercase letter, lowercase letter, number, and special character</span>
      <span class="<?= isset($errors['pass_valid']) ? "" : "no" ?>error"><?= $errors['pass_valid'] ?? "" ?></span>
    </div>
    <!-- confirm new password -->
    <div>
      <label for="newpass2">Confirm New Password:</label>
      <input type="password" name="newpass2" id="newpass2" value="" required />
      <span class="<?= isset($errors['pass_match']) ? "" : "no" ?>error"><?= $errors['pass_match'] ?? "" ?></span>
    </div>
    <input type="submit" name="submit" value="Change Password">
    <input type="submit" name="back" value="Back" formnovalidate>
  </form>
  <?php endif; ?>

  <!-- include js files -->
  <?php include 'includes/js_includes.php';?>
  <script src="js/account_scripts.js"></script>
</body>

</html>
