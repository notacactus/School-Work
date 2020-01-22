/*
COIS 3420 Project Scripts for Add and Edit Book Pages
Author: Ryland Whillans

checks that each entry is valid and prevents submission if so
*/

// check if cover valid
let covervalid = true;
$("input[type=file]:eq(0)").on("change", function() {
  let $this = $(this);
  let $next = $this.next();
  // check if cover uploaded
  if (this.files[0] != undefined) {
    let name = this.files[0].name;
    let validext = ["apng", "bmp", "gif", "ico", "cur", "ico", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "tif", "tiff", "webp", "xbm"];
    // check if filetype valid
    if ($.inArray(name.substring(name.lastIndexOf('.') + 1), validext) < 0) {
      covervalid = false;
      $next.addClass("error").removeClass("noerror").text("Invalid File Type");
      // check if too large
    } else if (this.files[0].size > 2000000) {
      covervalid = false;
      $next.addClass("error").removeClass("noerror").text("Files must be less than 2MB");
      // otherwise valid
    } else {
      $next.addClass("noerror").removeClass("error");
      covervalid = true;
    }
  } else {
    $next.addClass("noerror").removeClass("error");
    covervalid = true;
  }
});

// check if title valid
let titlevalid = false;
$("input[type=text]:eq(0)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let text = $this.val();
  titlevalid = false;
  // check if title not set
  if (text.length == 0) {
    $next.addClass("error").removeClass("noerror").text("Title is required");
    // check if too long
  } else if (text.length > 100) {
    $next.addClass("error").removeClass("noerror").text("Title must not exceed 100 characters");
  } else {
    $next.addClass("noerror").removeClass("error");
    titlevalid = true;
  }
});

//check if authors valid
let authorsvalid = true;
$("input[type=text]:eq(1)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next().next();
  // split author input into array
  let list = $this.val().split(/\s*,\s*/);
  authorsvalid = true;
  // loop and check if any are too long
  for (let text of list)
    if (text.length > 100) {
      $next.addClass("error").removeClass("noerror").text("Author names must not exceed 100 characters");
      authorsvalid = false;
    }
  if (authorsvalid)
    $next.addClass("noerror").removeClass("error");
});

// check if genres valid
let genresvalid = true;
$("input[type=text]:eq(2)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next().next();
  // split genre input into array
  let list = $this.val().split(/\s*,\s*/);
  genresvalid = true;
  // loop and check if any are too long
  for (let text of list)
    if (text.length > 50) {
      $next.addClass("error").removeClass("noerror").text("Genres must not exceed 50 characters");
      genresvalid = false;
    }
  if (genresvalid)
    $next.addClass("noerror").removeClass("error");
});

// check if language valid
let langvalid = true;
$("input[type=text]:eq(3)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let text = $this.val();
  // check if too long
  if (text.length > 50) {
    $next.addClass("error").removeClass("noerror").text("Language must not exceed 50 characters");
    langvalid = false;
  } else {
    $next.addClass("noerror").removeClass("error");
    langvalid = true;
  }
});

// check pub date valid
let datevalid = true;
$("input[type=date]").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let text = $this.val();
  // check if correct date format
  if (text.length > 0 && !/^\d{4}-\d\d-\d\d$/.test(text)) {
    $next.addClass("error").removeClass("noerror").text("Dates must be in format YYYY/MM/DD");
    datevalid = false;
  } else {
    $next.addClass("noerror").removeClass("error");
    datevalid = true;
  }
});

// check if publisher valid
let pubvalid = true;
$("input[type=text]:eq(4)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let text = $this.val();
  // check if too long
  if (text.length > 100) {
    $next.addClass("error").removeClass("noerror").text("Publisher name must not exceed 100 characters");
    pubvalid = false;
  } else {
    $next.addClass("noerror").removeClass("error");
    pubvalid = true;
  }
});

// check if isbn valid
let isbnvalid = true;
$("input[type=text]:eq(5)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let text = $this.val();
  // check if too long
  if (text.length > 50) {
    $next.addClass("error").removeClass("noerror").text("ISBN must not exceed 50 characters");
    isbnvalid = false;
  } else {
    $next.addClass("noerror").removeClass("error");
    isbnvalid = true;
  }
});

// check if extra info valid
let extravalid = true;
$("textarea").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let text = $this.val();
  // check if too long
  if (text.length > 1000) {
    $next.addClass("error").removeClass("noerror").text("Additional information is limited to 1000 characters");
    extravalid = false;
  } else {
    $next.addClass("noerror").removeClass("error");
    extravalid = true;
  }
});

// check if uploaded files valid
let filesvalid = true;
$("input[type=file]:eq(1)").on("change", function() {
  let $this = $(this);
  let $next = $this.next();
  // check if any files uploaded
  if (this.files[0] != undefined) {
    let name = "";
    let validext = ["opus", "flac", "webm", "weba", "wav", "ogg", "m4a", "mp3", "oga", "mid", "amr", "aiff", "wma", "au", "acc", "epub", "mobi", "pdf", "azw", "azw3"];
    filesvalid = true;
    //loop through files
    for (let file of this.files) {
      name = file.name;
      // check if extension valid
      if ($.inArray(name.substring(name.lastIndexOf('.') + 1), validext) < 0) {
        filesvalid = false;
        $next.addClass("error").removeClass("noerror").text("Invalid File Type");
        // check if too large
      } else if (file.size > 2000000) {
        filesvalid = false;
        $next.addClass("error").removeClass("noerror").text("Files must be less than 2MB");
      }
    }
    if (filevalid)
      $next.addClass("noerror").removeClass("error");
  } else {
    $next.addClass("noerror").removeClass("error");
    filesvalid = true;
  }
});

// if anything invalid prevent submission
$("form>input:nth-of-type(1)").on("click", function(ev) {
  if (!(covervalid && titlevalid && authorsvalid && genresvalid && langvalid && datevalid && pubvalid && isbnvalid && extravalid && filesvalid))
    ev.preventDefault();
});
