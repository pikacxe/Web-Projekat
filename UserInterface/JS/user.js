$(document).ready(function () {
    checkLogin().then(function () {
        let ID = new URL(window.location.href).searchParams.get("ID");
        if (ID != null) {
            $("#title").text("Edit user");
            $("#addBtn").text("Save");
            populateFields(ID);
            $("#usrname").addClass("hide");
            $("#usrlbl").addClass("hide");
            $("#pswd").addClass("hide");
            $("#pswdlbl").addClass("hide");
            $("#addBtn").click({ id: ID }, updateUser);
            $("#chgUsrname").removeClass("hide");
            $("#chgUsrname").click({ id: ID }, changeUsername);
            $("#chgPswd").removeClass("hide");
            $("#chgPswd").click({ id: ID }, changePassword);
        }
        else if (role == "Administrator") {
            $("#addBtn").click({}, addUser);
        }
        else {
            window.location.href = profileUrl;
        }
    });
});

let enable_usrname_change = false;
function changeUsername(event) {
    event.preventDefault();
    $("#usrname").removeClass("hide");
    $("#usrlbl").removeClass("hide").text("New username");
    $("#chgUsrname").remove();
    enable_usrname_change = true;
    return;

}

let enable_pass_change = false;
function changePassword(event) {
    event.preventDefault();
    $("#pswdlbl").removeClass("hide").text("New password");
    $("#pswd").removeClass("hide");
    $("#oldPswdlbl").removeClass("hide");
    $("#oldPswd").removeClass("hide");
    $("#chgPswd").remove();
    enable_pass_change = true;
    return;
}


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
            showApiMessage(result.Message, error);
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
    if (enable_usrname_change) {
        let usrname = $("#usrname").val().trim();
        if (username == "") {
            $("#usrname")[0].setCustomValidity("Username is required");
            $("#usrname")[0].reportValidity();
            return;
        }
        else {
            $("#usrname")[0].setCustomValidity("");
        }
        let req = {
            UserID: user.ID,
            NewUsername: usrname
        }
        $.ajax({
            url: api + "users/update-username/",
            type: 'PUT',
            contentType: "application/json",
            data: JSON.stringify(req),
            headers: { "Authorization": token },
            success: function (response) {
                enable_usrname_change = false;
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                let result = JSON.parse(xhr.responseText);
                showApiMessage(result.Message, error);
                return;
            }
        });
    }

    user.Username = old_usrname;

    if (enable_pass_change) {
        let oldPswd = $("#oldPswd").val().trim();
        let pswd = $("#pswd").val().trim();
        if (oldPswd == "") {
            $("#oldPswd")[0].setCustomValidity("Old password is required");
            $("#oldPswd")[0].reportValidity();
            return;
        }
        else {
            $("#oldPswd")[0].setCustomValidity("");
        }
        if (pswd == "") {
            $("#pswd")[0].setCustomValidity("New password is required");
            $("#pswd")[0].reportValidity();
            return;
        }
        else {
            $("#pswd")[0].setCustomValidity("");
        }
        let req = {
            UserID: user.ID,
            OldPassword: oldPswd,
            NewPassword: pswd
        }
        $.ajax({
            url: api + "users/update-password/",
            type: 'PUT',
            contentType: "application/json",
            data: JSON.stringify(req),
            headers: { "Authorization": token },
            success: function (response) {
                enable_pass_change = false;
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                let result = JSON.parse(xhr.responseText);
                showApiMessage(result.Message, error);
                return;
            }
        });
    }
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
            showApiMessage(result.Message, error);
        }
    });
    return;
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
            showApiMessage(result.Message, error);
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
        Password: pass,
        FirstName: firstname,
        LastName: lastname,
        Email: email,
        Gender: gender,
        DateOfBirth: dob,
        Role: 1
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