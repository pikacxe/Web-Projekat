$(document).ready(function () {
    checkLogin().then(function(){
        let ID = new URL(window.location.href).searchParams.get("ID");
        if (ID != null) {
            $("#title").text("Edit user");
            $("#addBtn").text("Save");
            populateFields(ID);
            $("#addBtn").click({ id: ID }, updateUser);
        }
        else if (role == "Administrator") {
            $("#addBtn").click({}, addUser);
        }
        else {
            window.location.href = profileUrl;
        }
    });
});


function addUser(event) {
    if (role != "Administrator") {
        window.location.href = profileUrl;
    }
    event.preventDefault();
    user = ValidateUser();
    console.log(JSON.stringify(user));
    $.ajax({
        url: api + "users/add/",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(user),
        headers: { "Authorization": token },
        success: function (response) {
            window.location.href = profileUrl;
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
    return false;
}

function updateUser(event) {
    event.preventDefault();
    user = ValidateUser();
    user.ID = event.data.id;
    console.log(JSON.stringify(user));
    $.ajax({
        url: api + "users/update/",
        type: 'PUT',
        contentType: "application/json",
        data: JSON.stringify(user),
        headers: { "Authorization": token },
        success: function (response) {
            window.location.href = profileUrl;
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
    return false;
}
function populateFields(id) {
    $.ajax({
        url: api + "users/user/" + id,
        method: 'GET',
        contentType: "application/json",
        success: function (res) {
            $("#usrname").val(res.Username);
            $("#fname").val(res.FirstName);
            $("#lname").val(res.LastName);
            $("#gender").val(res.Gender === "m" ? "Male" : "Female");
            $("#email").val(res.Email);
            $("#dob").val(new Date(res.DateOfBirth).toISOString().split("T")[0]);
            $("#role").val(res.RoleName);

        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    })
}

function ValidateUser() {
    let username = $("#usrname").val();
    let password = $("#pswd").val();
    let firstname = $("#fname").val();
    let lastname = $("#lname").val();
    let email = $("#email").val();
    let gender = $("input[name='Gender']:checked").val();
    let dateOfBirth = new Date($("#dob").val());

    // Validate username, password, firstname, lastname, email, gender, and date of birth
    if (username === "" || firstname === "" || lastname === "" || email === "" || !gender || dateOfBirth >= Date.now()) {
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
        DateOfBirth: dateOfBirth,
        Role: 1
    };
    console.log(user);
    return user;
}