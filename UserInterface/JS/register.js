$(document).ready(() => {
    checkLogin().then(() => {
        $('#signupBtn').click({}, Signup);
    });
});


function Signup(event) {
    event.preventDefault();
    user = ValidateUser();
    if (user == null) {
        return;
    }
    console.log(JSON.stringify(user));
    $.ajax({
        url: api + "users/register",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(user),
        success: function (response) {
            window.location.href = web + "login.html";
        },
        error: function (xhr, status, error) {
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
}

function ValidateUser() {
    let username = $("#usrname").val().trim();
    let password = $("#pswd").val().trim();
    let firstname = $("#fname").val().trim();
    let lastname = $("#lname").val().trim();
    let email = $("#email").val().trim();
    console.log(email);
    let gender = $("input[name='Gender']:checked").val();
    let dob = new Date($("#dob").val());

    isValid = true;

    if (username === "") {
        isValid = false;
        $("#usrname")[0].setCustomValidity("Username is required.");
    } else {
        $("#usrname")[0].setCustomValidity("");
    }
    $("#usrname")[0].reportValidity();

    if (password === "") {
        isValid = false;
        $("#pswd")[0].setCustomValidity("Password is required.");
    } else {
        $("#pswd")[0].setCustomValidity("");
    }
    $("#pswd")[0].reportValidity();

    if (firstname === "") {
        isValid = false;
        $("#fname")[0].setCustomValidity("Firstname is required.");
    } else {
        $("#fname")[0].setCustomValidity("");
    }
    $("#fname")[0].reportValidity();

    if (lastname === "") {
        isValid = false;
        $("#lname")[0].setCustomValidity("Lastname is required.");
    } else {
        $("#lname")[0].setCustomValidity("");
    }
    $("#lname")[0].reportValidity();

    // Validate email format using a regular expression
    let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email.match(emailPattern)) {
        isValid = false;
        $("#email")[0].setCustomValidity("Please enter a valid email address");
    } else {
        $("#email")[0].setCustomValidity("");
    }
    $("#email")[0].reportValidity();

    // Validate date of birth
    if (dob === null && dob >= Date.now()) {
        isValid = false;
        $("#dob")[0].setCustomValidity("Date of birth is required.");
    } else{
        $("#dob")[0].setCustomValidity("");
    }
    $("#dob")[0].reportValidity();

    if (!isValid) {

        return null;
    }

    let user = {
        Username: username,
        Password: password,
        FirstName: firstname,
        LastName: lastname,
        Email: email,
        Gender: gender,
        DateOfBirth: dob,
        Role: 0
    };
    console.log(user);
    return user;
}