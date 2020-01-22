<!--
COIS 3420 Login Check
Author: Ryland Whillans

Checks to see if user is logged in or has a valid persistent session and redirects to login if not
also includes pdo, connects to db and starts session
-->
<?php
// include pdo/connect/start session
include "includes/pdo.php";
$pdo=connectdb();
session_start();
// if user is logged in continue to page, otherwise check for persistent session
if (!isset($_SESSION['user'])) {
    $loggedin = false; //flag for valid login
    // check if persistent session coolie exists
    if (isset($_COOKIE['login_token'])) {
        // get data from cookie and check if it exists in database
        list($token, $_SESSION['user']) = explode(" ", $_COOKIE['login_token']);
        $stmt = $pdo->prepare("SELECT id, timeout FROM proj_persistent_sessions WHERE token = ? AND fk_userid = ?");
        $stmt->execute([$token, $_SESSION['user']]);
        // if matching token in database check if expired
        if ($stmt->rowcount() > 0) {
            $row = $stmt->fetch();
            // if not expired regenerate token and log user in
            if ((int)$row['timeout'] > time()) {
                // regenrate and replace token in database and cookie
                $token = bin2hex(random_bytes(20));
                $timeout = time() + (60 * 60 * 24 * 7);
                $pdo->prepare("UPDATE proj_persistent_sessions SET token = ?, timeout = ? WHERE id = ?")->execute([$token, $timeout, $row['id']]);
                setcookie("login_token", $token . " " . $_SESSION['user'], $timeout);
                // get email from database and save
                $stmt = $pdo->prepare("SELECT email FROM proj_users WHERE id = ?");
                $stmt->execute([$row['id']]);
                $_SESSION['email'] = $stmt->fetch()['email'];
                // set logged in flag
                $loggedin  = true;
            }
        }
    }
    // redirect to login if no valid session and not already at login
    if (!isset($login) && !$loggedin) {
        unset($_SESSION['user']);
        header('Location: login.php');
        exit();
    }
}
