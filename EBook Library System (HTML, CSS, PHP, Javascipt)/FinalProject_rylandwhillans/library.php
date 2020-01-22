<!--
COIS 3420 Project Library Page
Author: Ryland Whillans

Page to display user's books
Allows search by title
Uses pagination to limit results and has options to change count per page and direction
-->
<?php
  // check if logged in
  include "includes/logincheck.php";

  $title = ($_GET['title'] ?? "") . "%";  // append wildcard to end of search parameter
  $page = abs(intval($_GET['page'] ?? 1)); // page parameter as positive int
  $displaycount = intval($_GET['displaycount'] ?? 10); // count per page as int
  // set to 1 if , 0
  if ($displaycount <= 0) {
      $displaycount = 1;
  }
  $order = (isset($_GET['order']) && $_GET['order'] == 'DESC') ? 'DESC' : 'ASC';  // Set sort order as desc if selected, asc otherwise
  // count books that match search
  $sql = "SELECT COUNT(*) FROM proj_books WHERE fk_userid = ? AND LOWER(title) LIKE ?";
  $stmt = $pdo->prepare($sql);
  $stmt->execute([$_SESSION['user'], $title]);
  // determine nuumber of pages based on result and display amount
  $numpages = ceil($stmt->fetchColumn() / $displaycount);
  // set numpages to 1 if 0
  if ($numpages == 0) {
    $numpages = 1;
  }
  // set page to max if exceeding max
  if ($page > $numpages) {
      $page = $numpages;
  }
  // get title/cover/id of books that match search/page/count parameters
  $sql = "SELECT id, title, cover FROM proj_books WHERE fk_userid = ? AND LOWER(title) LIKE ? ORDER BY LOWER(title) " . $order . " LIMIT ? OFFSET ?";
  $stmt = $pdo->prepare($sql);
  $stmt->execute([$_SESSION['user'], $title, $displaycount, $displaycount * ($page - 1)]);
  // remove widlcard from search parameter
  $title = substr($title, 0, -1);
?>

<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>My Library</title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/librarystyles.css" />
</head>

<body>
  <!-- include nav -->
  <?php include "includes/nav.php"; ?>


  <h1>My Library</h1>
  <!-- searchbar to search by title, contains hidden fields to preserve count/order -->
  <form id="search" action="library.php" method="get">
    <input type="text" name="title" value="<?= $title ?>">
    <input type="hidden" name="displaycount" value="<?= $displaycount ?>">
    <input type="hidden" name="order" value="<?= $order ?>">
    <input type="submit" name="search" value="Search">
  </form>
  <!-- optinons to filter results, currently selected options disabled -->
  <div class="filter">
    <!-- number of books to display per page -->
    <div>
      <h1>Books per page: </h1>
      <?php if ($displaycount == 10): ?>
      <span class="disabled">10</span>
      <?php else: ?>
      <a href="library.php?title=<?= $title ?>&order=<?= $order ?>&displaycount=10">10</a>
      <?php endif; ?>
      <?php if ($displaycount == 25): ?>
      <span class="disabled">25</span>
      <?php else: ?>
      <a href="library.php?title=<?= $title ?>&order=<?= $order ?>&displaycount=25">25</a>
      <?php endif; ?>
      <?php if ($displaycount == 50): ?>
      <span class="disabled">50</span>
      <?php else: ?>
      <a href="library.php?title=<?= $title ?>&order=<?= $order ?>&displaycount=50">50</a>
      <?php endif; ?>
    </div>
    <!-- page number -->
    <div>
      <h1>Page:</h1>
      <?php for ($i = 1; $i <= $numpages; $i++): ?>
      <?php if ($i == $page): ?>
      <span class="disabled"><?= $i ?></span>
      <?php else: ?>
      <a href="library.php?title=<?= $title ?>&order=<?= $order ?>&displaycount=<?= $displaycount?>&page=<?= $i ?>"><?= $i ?></a>
      <?php endif; ?>
      <?php endfor; ?>
    </div>
    <!-- order to display (ASC/DESC) -->
    <div>
      <h1>Order:</h1>
      <?php if ($order == 'ASC'): ?>
      <span class="disabled">ASC</span>
      <a href="library.php?title=<?= $title ?>&order=DESC&displaycount=<?= $displaycount?>">DESC</a>
      <?php else: ?>
      <a href="library.php?title=<?= $title ?>&order=ASC&displaycount=<?= $displaycount?>">ASC</a>
      <span class="disabled">DESC</span>
      <?php endif; ?>
    </div>
  </div>
  <!-- contains all books to display -->
  <div class="library">
    <!-- loop to display each cover and title as a link to book page -->
    <?php foreach ($stmt as $book): ?>
    <div class="thumbnail">
      <a href="book.php?book_id=<?= $book['id'] ?>">
        <img src="<?= $book['cover'] ? "/~rylandwhillans/www_data/" . $_SESSION['user'] . "/" . $book['id'] . "/cover" : "img/missing_cover.jpg" ?>" alt="<?= $book['title'] ?> Cover">
        <span><?= $book['title'] ?></span>
      </a>
    </div>
    <?php endforeach; ?>
  </div>
</body>

</html>
