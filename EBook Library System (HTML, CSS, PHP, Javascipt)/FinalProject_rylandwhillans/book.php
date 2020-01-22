<!--
COIS 3420 Project Book Page
Author: Ryland Whillans

Page to allow display details of a book and provide options to download uploaded ebooks and edit/delete the book
-->
<?php
  //check if logged in
  include "includes/logincheck.php";
  // check if book selected and redirect to library if not
  if (!isset($_GET['book_id'])) {
      header('Location: library.php');
      exit();
  }

  //check if book id is valid and belongs to current user, redirect to library if not
  $book_id = $_GET['book_id'];
  $stmt = $pdo->prepare("SELECT * FROM proj_books WHERE id = ? && fk_userid = ?");
  $stmt->execute([$book_id, $_SESSION['user']]);
  if ($stmt->rowcount() == 0) {
      header('Location: library.php');
      exit();
  }

  // store book details in session, then select authors/genres/files from corresponding tables and append them to book details
  $_SESSION['book'] = $stmt->fetch();
  $book = &$_SESSION['book'];
  // get authors
  $stmt = $pdo->prepare("SELECT author FROM proj_authors WHERE fk_bookid = ?");
  $stmt->execute([$book_id]);
  $_SESSION['book']['authors'] = array();
  $authors = &$_SESSION['book']['authors'];
  foreach ($stmt as $author) {
      $authors[] = $author['author'];
  }
  // get genres
  $stmt = $pdo->prepare("SELECT genre FROM proj_genres WHERE fk_bookid = ?");
  $stmt->execute([$book_id]);
  $_SESSION['book']['genres'] = array();
  $genres = &$_SESSION['book']['genres'];
  foreach ($stmt as $genre) {
      $genres[] = $genre['genre'];
  }
  // get files
  $stmt = $pdo->prepare("SELECT id, filename FROM proj_bookfiles WHERE fk_bookid = ?");
  $stmt->execute([$book_id]);
  $_SESSION['book']['files'] = array();
  $files = &$_SESSION['book']['files'];
  foreach ($stmt as $file) {
      $files[] = array('id' => $file['id'], 'filename' => $file['filename']);
  }
?>
<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title><?= $book['title'] ?></title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body id="book">
  <!-- include nav -->
  <?php include "includes/nav.php"; ?>

  <!-- display all information that was stored about book -->
  <h1><em><?= $book['title'] ?></em></h1>
  <!-- cover or placeholder if not exist -->
  <img src="<?= $book['cover'] ? "/~rylandwhillans/www_data/" . $_SESSION['user'] . "/" . $book['id'] . "/cover" : "img/missing_cover.jpg" ?>" alt="<?= $book['title']  ?> Cover">
  <!-- title (only data that is required/always shown)-->
  <div>
    <h1>Title:</h1>
    <span><?= $book['title'] ?></span>
  </div>
  <!-- loop to display authors -->
  <?php if (!empty($authors)): ?>
  <div>
    <h1>Author(s):</h1>
    <ul>
      <?php foreach ($authors as $author): ?>
      <li><?= $author ?></li>
      <?php endforeach; ?>
    </ul>
  </div>
  <?php endif; ?>
  <!-- status -->
  <?php if (isset($book['status'])): ?>
  <div>
    <h1>Reading Status:</h1>
    <span><?= $book['status'] ?></span>
  </div>
  <?php endif; ?>
  <!-- loop to display genres -->
  <?php if (!empty($genres)): ?>
  <div>
    <h1>Genre(s):</h1>
    <ul>
      <?php foreach ($genres as $genre): ?>
      <li><?= $genre ?></li>
      <?php endforeach; ?>
    </ul>
  </div>
  <?php endif; ?>
  <!-- format -->
  <?php if (isset($book['format'])): ?>
  <div>
    <h1>Format:</h1>
    <span><?= $book['format'] ?></span>
  </div>
  <?php endif; ?>
  <!-- language -->
  <?php if (isset($book['language'])): ?>
  <div>
    <h1>Language:</h1>
    <span><?= $book['language'] ?></span>
  </div>
  <?php endif; ?>
  <!-- pub date -->
  <?php if (isset($book['pub_date'])): ?>
  <div>
    <h1>Date Published:</h1>
    <span><?= $book['pub_date'] ?></span>
  </div>
  <?php endif; ?>
  <!-- publisher -->
  <?php if (isset($book['publisher'])): ?>
  <div>
    <h1>Publisher:</h1>
    <span><?= $book['publisher'] ?></span>
  </div>
  <?php endif; ?>
  <!-- isbn -->
  <?php if (isset($book['isbn'])): ?>
  <div>
    <h1>ISBN:</h1>
    <span><?= $book['isbn'] ?></span>
  </div>
  <?php endif; ?>
  <!-- extra info -->
  <?php if (isset($book['extra'])): ?>
  <div>
    <h1>Additional Information:</h1>
    <span><?= $book['extra'] ?></span>
  </div>
  <?php endif; ?>
  <!-- loop through uploaded files and display each one as a link to download -->
  <?php if (!empty($files)): ?>
  <div>
    <h1>Download(s):</h1>
    <ul>
      <?php foreach ($files as $file): ?>
      <!-- change to download link -->
      <li><a href=<?="/~rylandwhillans/www_data/" . $_SESSION['user'] . "/" . $book['id'] . "/" . $file['id'] ?>><?= $file['filename'] ?></a></li>
      <?php endforeach; ?>
    </ul>
  </div>
  <?php endif; ?>
  <!-- links to edit and delete pages -->
  <div>
    <a href="editbook.php">Edit</a>
    <a href="deletebook.php">Delete</a>
  </div>
</body>

</html>
