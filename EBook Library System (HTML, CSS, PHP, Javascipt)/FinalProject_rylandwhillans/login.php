<!--
COIS 3420 Project Login Page
Author: Ryland Whillans

Page to allow users to login with links to account recover or creation and option to stay logged in
Self processing to handie validation and logging in/creating persistent session
-->
<?php
  $login = true; // flag to prevent redirect loop with login check
  // check if logged in
  include "includes/logincheck.php";
  // if logged in redirect to library
  if (isset($_SESSION['user'])) {
      header('Location: library.php');
      exit();
  }

  $_SESSION['email'] = $_POST['email'] ?? "";
  // if registration selected redirect to registration page
  if (isset($_POST['register'])) {
      header('Location: register.php');
      exit();
  //if recovery selected redirect to recovery page
  } elseif (isset($_POST['recovery'])) {
      header('Location: recovery.php');
      exit();
  }

  $email = &$_SESSION['email'];
  $password = $_POST['password'] ?? "";
  $error = false; // flag for error logging in
  // if login selected process login
  if (isset($_POST['login'])) {
      // retrieve account details based on email
      $stmt = $pdo->prepare("SELECT id, passhash FROM proj_users WHERE email = ?");
      $stmt->execute([$email]);
      $row = $stmt->fetch();
      // if account doesnt exist or password incorrect set error
      if ($stmt->rowcount() == 0 || !password_verify($password, $row['passhash'])) {
          $error = true;
      // otherwise log user in
      } else {
          // set user session variable (used to check if logged in)
          $_SESSION['user'] = $row['id'];
          // if remember selected create persistent session
          if (isset($_POST['remember'])) {
              // generate token and save in database with 1 week timeout
              $token = bin2hex(random_bytes(20));
              $timeout = time() + (60 * 60 * 24 * 7);
              $pdo->prepare("INSERT INTO proj_persistent_sessions (fk_userid, token, timeout) VALUES (?, ?, ?)")->execute([$_SESSION['user'], $token,  $timeout]);
              // set cookie for session
              setcookie("login_token", $token . " " . $_SESSION['user'], $timeout);
          }
          // redirect to library
          header('Location: library.php');
          exit();
      }
  }
?>

<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>E-Bookshelf Login</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <h1>E-Bookshelf Login</h1>
  <!-- login form -->
  <form id="login" action="login.php" method="post">
    <!-- email -->
    <div>
      <label for="email">Email Address:</label>
      <input type="email" name="email" id="email" value="<?= $email ?>" required />
    </div>
    <!-- password -->
    <div>
      <label for="password">Password:</label>
      <input type="password" name="password" id="password" value="" required />
      <!-- error if invalid login -->
      <span class="<?= $error ? "" : "no" ?>error">Incorrect Email or Password</span>
    </div>
    <!-- persistent login -->
    <div>
      <input type="checkbox" name="remember" id="remember" value="remember" <?= isset($_POST['remember']) ? 'checked' : "" ?>>
      <label for="remember">Remember Me</label>
    </div>

    <!-- options to login or register -->
    <div>
      <input type="submit" name="login" value="Login" />
      <span>or</span>
      <input type="submit" name="register" value="Register" formnovalidate />
    </div>
    <!-- account recovery -->
    <input type="submit" name="recovery" value="Forgot my password" formnovalidate />
  </form>
</body>

</html>
