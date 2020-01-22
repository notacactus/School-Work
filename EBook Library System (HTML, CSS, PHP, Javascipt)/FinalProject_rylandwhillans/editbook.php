<!--
COIS 3420 Project Edit Book Page
Author: Ryland Whillans

Page containing form to an existing book from library
to access page, must have come from link in book page as book page handles loading book details to session
Self-processing form handles validating input
-->
<?php
// check if logged in
include "includes/logincheck.php";
// check if book loaded (must have passed through book page) and redirect to library if not
if (!isset($_SESSION['book'])) {
    header('Location: library.php');
    exit();
}
// load values from POST/FILES array when form submited and strip whitespace, otherwise use empty valuesprevious book values
$errors = array();  // array for validation errors
$book = $_SESSION['book'];
$cover = $_FILES['cover'] ?? "";
$title = trim($_POST['title'] ?? $book['title'] ?? "");
// split on ',', use previous book values if not set
$authors = isset($_POST['authorlist']) ?
    (strlen($_POST['authorlist']) > 0 ?
      preg_split('~\s*,\s*~', $_POST['authorlist']) :
      array()) :
    $_SESSION['book']['authors'];
$status = trim($_POST['status'] ?? $book['status'] ?? "");
// split on ',', use previous book values if not set
$genres = isset($_POST['genrelist']) ?
    (strlen($_POST['genrelist']) > 0 ?
      preg_split('~\s*,\s*~', $_POST['genrelist']) :
      array()) :
    $_SESSION['book']['genres'];
$format = trim($_POST['format'] ??  $book['format'] ?? "");
$language = trim($_POST['language'] ?? $book['language'] ?? "");
$pub_date = trim($_POST['pub_date'] ?? $book['pub_date'] ?? "");
$publisher = trim($_POST['publisher'] ?? $book['publisher'] ?? "");
$isbn = trim($_POST['isbn'] ?? $book['isbn'] ?? "");
$extra = trim($_POST['extra'] ?? $book['extra'] ?? "");
$files = $_FILES['files'] ?? "";
$oldfiles = $_SESSION['book']['files'];
$deleted = $_POST['deletedfiles'] ?? array();

// process form if submitted
if (isset($_POST['submit'])) {
    // if cover uploaded check extension is valid type and if size too large(size will be 0 if upload failed due to file size)
    if ($cover['tmp_name'] != "") {
        $ext = pathinfo($cover['name'], PATHINFO_EXTENSION);
        $validtypes = array("apng", "bmp", "gif", "ico", "cur", "ico", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "tif", "tiff", "webp", "xbm");
        if (! in_array($ext, $validtypes)) {
            $errors['cover_valid'] = "Invalid File Type";
        }
        if ($cover['size'] == 0) {
            $errors['cover_valid'] = "Files must be less than 2MB";
        }
    }
    // check if title entered and not too long
    if (strlen($title) == 0) {
        $errors['title_valid'] = "Title is required";
    } elseif (strlen($title) > 100) {
        $errors['title_valid'] = "Title must not exceed 100 characters ";
    }
    //check if not too long
    foreach ($authors as $author) {
        if (strlen($author) > 100) {
            $errors['authors_valid'] = "Author names must not exceed 100 characters";
        }
    }
    //check if not too long
    foreach ($genres as $genre) {
        if (strlen($genre) > 50) {
            $errors['genres_valid'] =  "Genres must not exceed 50 charactes";
        }
    }
    // check if correct date format
    if (strlen($pub_date) > 0 && !preg_match("~^\d{4}-\d\d-\d\d$~", $pub_date)) {
        $errors['date_valid'] = "Dates must be in format YYYY/MM/DD";
    }
    //check if not too long
    if (strlen($language) > 50) {
        $errors['lang_valid'] = "Language must not exceed 50 characters";
    }
    //check if not too long
    if (strlen($publisher) > 100) {
        $errors['pub_valid'] = "Publisher name must not exceed 100 charaters";
    }
    //check if not too long
    if (strlen($isbn) > 50) {
        $errors['isbn_valid'] = "ISBN must not exceed 50 characters";
    }
    //check if not too long
    if (strlen($extra) > 1000) {
        $errors['extra_valid'] = "Additional information is limited to 1000 characters";
    }
    // if files uploaded check extensions are valid types and if sizes too large(size will be 0 if upload failed due to file size)
    if ($files['tmp_name'][0] != "") {
        $validtypes = array("opus", "flac", "webm", "weba", "wav", "ogg", "m4a", "mp3", "oga", "mid", "amr", "aiff", "wma", "au", "acc", "epub", "mobi", "pdf", "azw", "azw3");
        foreach ($files['name'] as $file) {
            $ext = pathinfo($file, PATHINFO_EXTENSION);
            if (! in_array($ext, $validtypes)) {
                $errors['files_valid'] = "Invalid File Type";
            }
        }
        foreach ($files['size'] as $size) {
            if ($size == 0) {
                $errors['files_valid'] = "Files must be less than 2MB";
            }
        }
    }

    // if no errors update book date in library
    if (empty($errors)) {
        // update values in books table, use null for any empty strings
        $sql = "UPDATE proj_books SET title = ?, cover = ?, format = ?, status = ?, language = ?, pub_date = ?, publisher = ?, isbn = ?, extra = ? WHERE id = ?";
        $pdo->prepare($sql)->execute(
            [
                $title,
                $book['cover'] || $cover != "",
                $format != "" ? $format : null,
                $status != "" ? $status : null,
                $language != "" ? $language : null,
                $pub_date != "" ? $pub_date : null,
                $publisher != "" ? $publisher : null,
                $isbn != "" ? $isbn : null,
                $extra != "" ? $extra : null,
                $book['id']
              ]
        );
        // delete existing authors/genres for book from tables
        // then insert new authors/genres into respective tables
        $sql = "DELETE FROM proj_authors WHERE fk_bookid = ?";
        $pdo->prepare($sql)->execute([$book['id']]);
        $sql = "INSERT INTO proj_authors (fk_bookid, author) VALUES (?, ?)";
        foreach ($authors as $author) {
            $pdo->prepare($sql)->execute([$book['id'], $author]);
        }
        $sql = "DELETE FROM proj_genres WHERE fk_bookid = ?";
        $pdo->prepare($sql)->execute([$book['id']]);
        $sql = "INSERT INTO proj_genres (fk_bookid, genre) VALUES (?, ?)";
        foreach ($genres as $genre) {
            $pdo->prepare($sql)->execute([$book['id'], $genre]);
        }

        //construct file path for storing cover/files and create directories if needed
        $path = "/home/rylandwhillans/public_html/www_data/" . $_SESSION['user'] . "/" ;
        if (!is_dir($path)) {
            mkdir($path);
            chmod($path, 0755);
        }
        $path .= $book['id'] . "/";
        if (!is_dir($path)) {
            mkdir($path);
            chmod($path, 0755);
        }
        // if cover already exists delete it, then save cover to directory with old extension
        if (file_exists($cover['tmp_name'])) {
            $coverpath = $path . "cover.";
            $oldcover = glob($coverpath . "*");
            if (!empty($oldcover)) {
                unlink($oldcover[0]);
            }
            $coverpath .= pathinfo($cover['name'], PATHINFO_EXTENSION);
            move_uploaded_file($cover['tmp_name'], $coverpath);
            chmod($coverpath, 0644);
        }
        // add files to table then save each to directory as id from table with extension
        if (file_exists($files['tmp_name'][0])) {
            for ($i = 0; $i < count($files['tmp_name']); $i++) {
                $pdo->prepare("INSERT INTO proj_bookfiles (fk_bookid, filename) VALUES (?, ?)")->execute([$book['id'], $files['name'][$i]]);
                $filepath = $path . $pdo->lastInsertId() . "." . pathinfo($files['name'][$i], PATHINFO_EXTENSION);
                move_uploaded_file($files['tmp_name'][$i], $filepath);
                chmod($filepath, 0644);
            }
        }
        // remove selected files from database then delete from directory
        foreach ($deleted as $fileid) {
            $pdo->prepare("DELETE FROM proj_bookfiles WHERE id = ?")->execute([$fileid]);
            $oldfile = glob($path . $fileid . ".*");
            if (!empty($oldfile)) {
                unlink($oldfile[0]);
            }
        }
        // redirect to book page
        header('Location: book.php?book_id=' . $book['id']);
        exit();
    }
}
 ?>
<!DOCTYPE html>
<html lang="en" dir="ltr">

<head>
  <meta charset="utf-8">
  <title>Edit: <?= $book['title'] ?></title>
  <link rel="stylesheet" href="css/reset.css" />
  <link rel="stylesheet" href="css/basestyles.css" />
  <link rel="stylesheet" href="css/formstyles.css" />
</head>

<body>
  <!-- include nav -->
  <?php include "includes/nav.php"; ?>

  <h1>Editing <em><?= $book['title'] ?></em></h1>
  <!-- form to display book details and allow editing (each input displays error in span based on validation results)-->
  <form id="edit" action="editbook.php" method="post" enctype="multipart/form-data">
    <!-- currently uploaded cover -->
    <div>
      <img src="<?= $book['cover'] ? "/~rylandwhillans/www_data/" . $_SESSION['user'] . "/" . $book['id'] . "/cover" : "img/missing_cover.jpg" ?>" alt="Book Cover">
    </div>
    <!-- cover upload - file -->
    <div>
      <label for="cover">Upload Cover:</label>
      <input type="file" name="cover" id="cover" accept="image/*">
      <span class="<?= isset($errors['cover_valid']) ? "" : "no" ?>error"><?= $errors['cover_valid'] ?? "" ?></span>
    </div>
    <!-- title - text (only required field) -->
    <div>
      <label for="title">Title:</label>
      <input type="text" name="title" id="title" value="<?= $title ?>" required>
      <span class="<?= isset($errors['title_valid']) ? "" : "no" ?>error"><?= $errors['title_valid'] ?? "" ?></span>
    </div>
    <!-- author - text, comma seperated -->
    <div>
      <label for="authorlist">Author(s):</label>
      <input type="text" name="authorlist" id="authorlist" value="<?= implode(', ', $authors) ?>">
      <span>Seperate with commas to enter multiple authors</span>
      <span class="<?= isset($errors['authors_valid']) ? "" : "no" ?>error"><?= $errors['authors_valid'] ?? "" ?></span>
    </div>
    <!-- reading status - radio -->
    <div>
      <h1>Reading Status:</h1>
      <div>
        <div>
          <input type="radio" name="status" id="status-none" value="" <?= $status == "" ? "checked" : "" ?>>
          <label for="status-none">None</label>
        </div>
        <div>
          <input type="radio" name="status" id="status-nostart" value="Not Started" <?= $status == "Not Started" ? "checked" : "" ?>>
          <label for="status-nostart">Not Started</label>
        </div>
        <div>
          <input type="radio" name="status" id="status-progress" value="In Progress" <?= $status == "In Progress" ? "checked" : "" ?>>
          <label for="status-progress">In Progress</label>
        </div>
        <div>
          <input type="radio" name="status" id="status-done" value="Finished" <?= $status == "Finished" ? "checked" : "" ?>>
          <label for="status-done">Finished</label>
        </div>
      </div>
    </div>
    <!-- genre - text, comma seperated -->
    <div>
      <label for="genrelist">Genre(s):</label>
      <input type="text" name="genrelist" id="genrelist" value="<?= implode(', ', $genres) ?>">
      <span>Seperate with commas to enter multiple genres</span>
      <span class="<?= isset($errors['genres_valid']) ? "" : "no" ?>error"><?= $errors['genres_valid'] ?? "" ?></span>
    </div>
    <!-- format - radio -->
    <div>
      <h1>Format:</h1>
      <div>
        <div>
          <input type="radio" name="format" id="format-none" value="" <?= $format == "" ? "checked" : "" ?>>
          <label for="format-none">None</label>
        </div>
        <div>
          <input type="radio" name="format" id="format-physical" value="Physical" <?= $format == "Physical" ? "checked" : "" ?>>
          <label for="format-physical">Physical</label>
        </div>
        <div>
          <input type="radio" name="format" id="format-ebook" value="Ebook" <?= $format == "Ebook" ? "checked" : "" ?>>
          <label for="format-ebook">Ebook</label>
        </div>
        <div>
          <input type="radio" name="format" id="format-audio" value="Audio" <?=  $format == "Audio" ? "checked" : "" ?>>
          <label for="format-audio">Audio</label>
        </div>
      </div>
    </div>
    <!-- language - text -->
    <div>
      <label for="language">Language:</label>
      <input type="text" name="language" id="language" value="<?= $language ?>">
      <span class="<?= isset($errors['lang_valid']) ? "" : "no" ?>error"><?= $errors['lang_valid'] ?? "" ?></span>
    </div>
    <!-- publish date - date -->
    <div>
      <label for="pub_date">Date Published:</label>
      <input type="date" name="pub_date" id="pub_date" value="<?= $pub_date ?>">
      <span class="<?= isset($errors['date_valid']) ? "" : "no" ?>error"><?= $errors['date_valid'] ?? "" ?></span>
    </div>
    <!-- publisher - text -->
    <div>
      <label for="publisher">Publisher:</label>
      <input type="text" name="publisher" id="publisher" value="<?= $publisher ?>">
      <span class="<?= isset($errors['pub_valid']) ? "" : "no" ?>error"><?= $errors['pub_valid'] ?? "" ?></span>
    </div>
    <!-- isbn - text -->
    <div>
      <label for="isbn">ISBN:</label>
      <input type="text" name="isbn" id="isbn" value="<?= $isbn ?>">
      <span class="<?= isset($errors['isbn_valid']) ? "" : "no" ?>error"><?= $errors['isbn_valid'] ?? "" ?></span>
    </div>
    <!-- extra info - textarea -->
    <div>
      <label for="extra">Additional Information:</label>
      <textarea name="extra" id="extra"><?= $extra ?></textarea>
      <span class="<?= isset($errors['extra_valid']) ? "" : "no" ?>error"><?= $errors['extra_valid'] ?? "" ?></span>
    </div>
    <!-- list of uploaded ebooks with option to delete - checkbox -->
    <div>
      <h1>Uploaded Ebooks:</h1>
      <?php if (empty($oldfiles)): ?>
      <span class="file">No ebooks Uploaded</span>
      <?php else: ?>
      <div>
        <?php foreach ($oldfiles as $file): ?>
        <div>
          <span class="file"><?= $file['filename'] ?></span>
          <input type="checkbox" name="deletedfiles[]" id="<?= $file['id'] ?>" value="<?= $file['id'] ?>">
          <label for="<?= $file['id'] ?>">Delete File</label>
        </div>
        <?php endforeach; ?>
      </div>
      <?php endif; ?>
    </div>
    <!-- upload ebooks - file (multiple) -->
    <div>
      <label for="files">Upload Ebooks</label>
      <input type="file" name="files[]" id="files" multiple accept="audio/*,.epub,.mobi,.pdf,.azw,.azw3">
      <span class="<?= isset($errors['files_valid']) ? "" : "no" ?>error"><?= $errors['files_valid'] ?? "" ?></span>
    </div>
    <input type="submit" name="submit" value="Save Changes">
  </form>
  
  <!-- include js files -->
  <?php include 'includes/js_includes.php';?>
  <script src="js/book_scripts.js"></script>
</body>

</html>
