<!--
COIS 3420 Nav Bar
Author: Ryland Whillans

Nav bar to include on all pages when logged in
Includes links to library, add book, account
displays current user and option to logout
-->
<nav>
  <!-- list with logout/add book/account, current page link disabled -->
  <ul>
    <li>
      <?php if (basename($_SERVER['PHP_SELF']) == 'library.php'): ?>
      <span class="disabled">Library</span>
      <?php else: ?>
      <a href="library.php">Library</a>
      <?php endif; ?>
    </li>
    <li>
      <?php if (basename($_SERVER['PHP_SELF']) == 'addbook.php'): ?>
      <span class="disabled">Add a Book</span>
      <?php else: ?>
      <a href="addbook.php">Add a Book</a>
      <?php endif; ?>
    </li>
    <li>
      <?php if (basename($_SERVER['PHP_SELF']) == 'account.php'): ?>
      <span class="disabled">My Account</span>
      <?php else: ?>
      <a href="account.php">My Account</a>
      <?php endif; ?>
    </li>
  </ul>
  <!-- current user and logout button -->
  <div class="">
    <span>Logged in as <?= $_SESSION['email'] ?></span>
    <a href="logout.php">Logout</a>
  </div>
</nav>
