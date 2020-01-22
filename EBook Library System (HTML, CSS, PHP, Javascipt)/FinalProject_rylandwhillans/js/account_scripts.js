/*
COIS 3420 Project Scripts for Accounts Page
Author: Ryland Whillans

Validadtes email and password and uses ajax call to check for duplicate emails
*/

// checks if changing email
if ($("input[type=email]").length > 0) {
  let emailvalid = false;
  $("input[type=email]").on("blur", function() {
    let $this = $(this);
    let $next = $this.next();
    let email = $this.val();
    emailvalid = false;
    // check if valid email
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
      $next.addClass("error").removeClass("noerror").text("Invalid Email");
    // check if too long
    else if (email.length > 254)
      $next.addClass("error").removeClass("noerror").text("Email must not exceed 254 characters");
    // use ajax to check if already in use
    else
      $.get("checkemail.php", {
        email: $this.val()
      })
      .done(function(data) {
        if (data == true) {
          $next.addClass("error").removeClass("noerror").text("Email already in use");
        } else {
          $next.addClass("noerror").removeClass("error");
          emailvalid = true;
        }
      });
  });

  // prevent submission if email invalid
  $("form>input:nth-of-type(1)").on("click", function(ev) {
    if (!emailvalid)
      ev.preventDefault();
  });

  // checks if changing password
} else if ($("input[type=password]").length > 0) {
  let passvalid = false;
  // checks if password valid
  $("input[type=password]:eq(1)").on("blur", function() {
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
  $("input[type=password]:eq(2)").on("blur", function() {
    let $this = $(this);
    let $next = $this.next();
    let pass = $("div:nth-of-type(2) input").val();
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
}
