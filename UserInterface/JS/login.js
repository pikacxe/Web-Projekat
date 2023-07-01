$(document).ready(() => {
    checkLogin();
    $('#loginBtn').click({},Login);
});

function Login(event) {
    event.preventDefault();
    let loginReq = ValidateLogin();
    if (loginReq == null) {
        return;
    }
    console.log(loginReq);
    $.ajax({
        url: api + "users/login",
        type: 'POST',
        data: JSON.stringify(loginReq),
        contentType: "application/json",
        success: function (response) {
            localStorage.setItem("jwt_token", response);
            window.location.href = web + "index.html";
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
}


function ValidateLogin() {
    let username = $("#usrname").val();
    let pass = $("#pswd").val();

    let isValid = true;
    // Validate username
    if (username === "") {
        isValid = false;
        $("#usrname")[0].setCustomValidity("Username is required.");
    } else {
        $("#usrname")[0].setCustomValidity("");
    }
    $("#usrname")[0].reportValidity();

    // Validate password
    if (pass === "") {
        isValid = false;
        $("#pswd")[0].setCustomValidity("Password is required.");
    } else {
        $("#pswd")[0].setCustomValidity("");
    }
    $("#pswd")[0].reportValidity();

    if (!isValid) {
        return null;
    }
    let req = {
        username: username,
        password: pass
    };
    return req;
}