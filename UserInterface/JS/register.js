$(document).ready(() => {
    $('#signupBtn').click(Signup);


});


function Signup() {
    console.log("Test");
    user = ValidateUser();
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

            // Handle any errors that occur during the AJAX request
            $("#error").text(xhr.responseText + "|" + status + "|" + error);
            console.error(error);
        }
    });
    return false;
}

function ValidateUser() {
    let username = $("#usrname").val();
    let password = $("#pswd").val();
    let firstname = $("#fname").val();
    let lastname = $("#lname").val();
    let email = $("#email").val();
    let gender = $("input[name='Gender']:checked").val();
    let dateOfBirth = $("#dob").val();

    // Validate username, password, firstname, lastname, email, gender, and date of birth
    if (username === "" || password === "" || firstname === "" || lastname === "" || email === "" || !gender || dateOfBirth === "") {
        alert("Please fill in all fields");
        return null;
    }

    // Validate email format using a regular expression
    let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email.match(emailPattern)) {
        alert("Please enter a valid email address");
        return null;
    }

    // Create a user object
    let user = {
        Username: username,
        Password: password,
        FirstName: firstname,
        LastName: lastname,
        Email: email,
        Gender: gender,
        DateOfBirth: dateOfBirth
    };

    return user;
}