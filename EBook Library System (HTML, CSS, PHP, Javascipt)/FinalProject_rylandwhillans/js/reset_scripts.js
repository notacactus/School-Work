/*
COIS 3420 Project Scripts for Password Reset Page
Author: Ryland Whillans

Validates email and password and uses ajax call to check for duplicate emails
*/

// checks if password valid
let passvalid = false;
$("input[type=password]:eq(0)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next().next();
  let pass = $this.val();
  passvalid = false;
  // check if too short
  if (pass.length < 8) {
    $next.addClass("error").removeClass("noerror").text("Password is not long enough");
    // check if contains required characters
  } else if (!/^(?=\D*\d)(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\w*\W)/.test(pass)) {
    $next.addClass("error").removeClass("noerror").text("Password does not contain required characters");
  } else {
    $next.addClass("noerror").removeClass("error");
    passvalid = true;
  }
});

//check if passwords match
let passmatch = false;
$("input[type=password]:eq(1)").on("blur", function() {
  let $this = $(this);
  let $next = $this.next();
  let pass = $("div:nth-of-type(1) input").val();
  let pass2 = $this.val();
  passmatch = false;
  // compare passwords
  if (pass != pass2) {
    $next.addClass("error").removeClass("noerror").text("Passwords do not match");
  } else {
    $next.addClass("noerror").removeClass("error");
    passmatch = true;
  }
});

//prevent submission if password invalid or passwords dont match
$("form>input:nth-of-type(1)").on("click", function(ev) {
  if (!(passvalid && passmatch))
    ev.preventDefault();
});
