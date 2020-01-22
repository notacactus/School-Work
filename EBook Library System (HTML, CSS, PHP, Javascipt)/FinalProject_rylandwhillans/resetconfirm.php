<!--
COIS 3420 Reset Email Processing and Confirmation Page
Author: Ryland Whillans

Page to send email when password recovery requested and confirm if email sent correctly
-->
<?php
  session_start();
  // redirect to login if not arriving from revocery page
  if (!isset($_SESSION['reco_id'])) {
      header('Location: login.php');
      exit();
  }
  include "includes/pdo.php";
  require_once "Mail.php";
  // generate reset token
  $token = bin2hex(random_bytes(20));

  // set properties for email
  $from = "Password System Reset <noreply@loki.trentu.ca>";
  $to = "<".$_SESSION['reco_mail'].">";
  $subject = "E-Bookshelf Email Recovery";
  $body = "<h1>Click the following link to reset your password: </h1><a href='https://loki.trentu.ca/~rylandwhillans/3420/project/passreset.php?token=" . $token . "'> Recover password</a>";
  $host = "smtp.trentu.ca";
  // send email with reset link
  $headers = array('From' => $from,'To' => $to,'Subject' => $subject);
  $smtp = Mail::factory('smtp', array('host' => $host));
  $mail = $smtp->send($to, $headers, $body);

  $pdo = connectdb();
  // remove any old reset tokens
  $stmt = $pdo->prepare("DELETE FROM proj_reset_requests WHERE fk_userid = ?")->execute([$_SESSION['reco_id']]);
  // save reset token in database with 1hr timeout
  $pdo->prepare("INSERT INTO proj_reset_requests (fk_userid, token, timeout) VALUES (?, ?, ?)")->execute([$_SESSION['reco_id'], $token, time()+3600]);

?>

<!DOCTYPE html>
<html lang="en" dir="ltr">
  <head>
    <meta charset="utf-8">
    <title>Account Recovery</title>
    <link rel="stylesheet" href="css/reset.css" />
    <link rel="stylesheet" href="css/basestyles.css" />
  </head>
  <body>
    <!-- displays confirmation or error message if mail failed to sent -->
    <h1>
      <?php
      if (PEAR::isError($mail)) {
          echo("Error Sending Recovery Email: ". $mail->getMessage());
      } else {
          echo("Recovery Email Sent");
      }
       ?>
    </h1>
    <!-- link back to login -->
    <a href="login.php">Return to login page</a>
  </body>
</html>
