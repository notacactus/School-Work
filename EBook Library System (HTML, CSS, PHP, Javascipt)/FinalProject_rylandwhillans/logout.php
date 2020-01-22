<!--
COIS 3420 Project Logout
Author: Ryland Whillans

Process logout and redirect to login
-->
<?php
session_start();
// if logged in, log out
if (isset($_SESSION['user'])) {
    // clear session variables and destroy session
    $_SESSION = array();
    session_destroy();
    // clear persistent session cookie if exists
    if (isset($_COOKIE['login_token'])) {
        unset($_COOKIE['login_token']);
        setcookie("login_token", '', 1);
    }
}
// redirect to login
header('Location: login.php');
exit();
?>
