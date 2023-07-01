$(document).ready(function () {
    checkLogin().then(function(){
        let ID = new URL(window.location.href).searchParams.get("ID");
        if (ID != null) {
            $("#title").text("Edit user");
            $("#addBtn").text("Save");
            populateFields(ID);
            $("#usrname").remove();
            $("#usrlbl").remove();
            $("#pswd").remove();
            $("#pswdlbl").remove();
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

let old_usrname;
let old_pass = "placeholder";

function addUser(event) {
    if (role != "Administrator") {
        window.location.href = profileUrl;
    }
    event.preventDefault();
    user = validateUser();
    if (user == null) {
        return;
    }
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
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
    return false;
}

function updateUser(event) {
    event.preventDefault();
    user = validateUpdateUser();
    if (user == null) {
        return;
    }
    user.ID = event.data.id;
    user.Username = old_usrname;
    user.Password = old_pass;
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
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
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
            old_usrname = res.Username;
            $("#fname").val(res.FirstName);
            $("#lname").val(res.LastName);
            $("#gender").val(res.Gender === "m" ? "Male" : "Female");
            $("#email").val(res.Email);
            $("#dob").val(new Date(res.DateOfBirth).toISOString().split("T")[0]);
            $("#role").val(res.RoleName);

        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    })
}

function validateUser() {
    let username = $("#usrname").val().trim();
    let pass = $("#pswd").val().trim();
    let firstname = $("#fname").val().trim();
    let lastname = $("#lname").val().trim();
    let email = $("#email").val().trim();
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

    if (pass === "") {
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
    } else {
        $("#dob")[0].setCustomValidity("");
    }
    $("#dob")[0].reportValidity();

    if (!isValid) {

        return null;
    }

    let user = {
        Username: username,
        Password:pass,
        FirstName: firstname,
        LastName: lastname,
        Email: email,
        Gender: gender,
        DateOfBirth: dob,
        Role:1
    };
    console.log(user);
    return user;
}

function validateUpdateUser() {
    let firstname = $("#fname").val().trim();
    let lastname = $("#lname").val().trim();
    let email = $("#email").val().trim();
    let gender = $("input[name='Gender']:checked").val();
    let dob = new Date($("#dob").val());

    isValid = true;

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
    } else {
        $("#dob")[0].setCustomValidity("");
    }
    $("#dob")[0].reportValidity();

    if (!isValid) {

        return null;
    }

    let user = {
        Username: old_usrname,
        Password: old_pass,
        FirstName: firstname,
        LastName: lastname,
        Email: email,
        Gender: gender,
        DateOfBirth: dob,
    };
    return user;
}