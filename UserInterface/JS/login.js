$(document).ready(() => {
    $('#loginBtn').click(Login);
});

// headers: { "Authorization": localStorage.getItem('token') }
function Login() {
    let loginReq = JSON.stringify(ValidateLogin());

    console.log(loginReq);
    $.ajax({
        url: api + "users/login",
        type: 'POST',
        data: loginReq,
        contentType: "application/json",
        success: function (response) {
            sessionStorage.setItem("jwt_token", response);
            window.location.href = web + "Pages/index.html";
        },
        error: function (xrh, status, error) {
            console.log(xrh.responseText);
        }
    });
    return false;
}


function ValidateLogin() {
    let usrname = $("#usrname").val();
    let pass = $("#pswd").val();

    if (usrname === "" || pass === "") {
        alert("Please fill in all fields");
        return false;
    }

    let req = {
        username: usrname,
        password: pass
    };
    return req;
}