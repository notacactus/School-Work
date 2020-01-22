<!--
COIS 3420 Project Book Deletion Page
Author: Ryland Whillans

Page to provide confirmation when users wish to delete a book and to remove all account data if so
-->
<?php
  // check if logged in
  include "includes/logincheck.php";
  // if book not selected return to library
  if (!isset($_SESSION['book'])) {
      header('Location: library.php');
      exit();
  }

  $book = $_SESSION['book'];
  // if user confirms delete, remove book from library
  if (isset($_POST['delete'])) {
      // delete book from books table, will cascade to other tables
      $sql = "DELETE FROM proj_books WHERE id = ?";
      $pdo->prepare($sql)->execute([$book['id']]);
      // construct path to book directory and delete all files/directory
      $path = "/home/rylandwhillans/public_html/www_data/" . $_SESSION['user'] . "/" . $book['id'] . "/";
      $files = glob($path . "*");
      foreach ($files as $file) {
          unlink($file);
      }
      rmdir($path);
      // unset session variable and return to library
      unset($_SESSION['book']);
      header('Location: library.php');
      exit();

  // if user cancels delete retunr to book page
  } elseif (isset($_POST['cancel'])) {
      header('Location: book.php?book_id=' . $book['id']);
      exit();
  }
?>

<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>Delete: <?= $book['title'] ?></title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <!-- include nav -->
  <?php include "includes/nav.php"; ?>
  <h1>Deleting <em><?= $book['title'] ?></em></h1>
  <!-- form to confirm deltion or cancel -->
  <form id="delete" action="deletebook.php" method="post">
    <h1>Are you sure you wish to delete this book?</h1>
    <input type="submit" name="delete" value="Delete">
    <input type="submit" name="cancel" value="Cancel">
  </form>
</body>

</html>
