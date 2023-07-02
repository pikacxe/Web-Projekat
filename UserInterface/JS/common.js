const api = "http://192.168.0.13:60471/api/";
const web = "http://192.168.0.13:8888/Pages/";
const imagesUrl = "http://192.168.0.13:60471/Images/";

let token = localStorage.getItem("jwt_token");
let role;
let currentID;
let isLoggedIn = false;
let favourites = [];
let username;
let profileUrl;
const dateLocale = "en-GB";

async function checkLogin() {
    try {
        let response = await $.ajax({
            url: api + "users/current",
            type: "GET",
            headers: {
                "Authorization": token
            }        
        });
        role = response.Role;
        currentID = response.ID;
        favourites = Array.from(response.Favourites);
        username = response.Username;
        $("#dropdown").removeClass("hide");
        let profile = $("#profile-name");
        profile.text(username + "🤵");
        isLoggedIn = true;
        if (role == "Administrator") {
            profileUrl = web + "dashboard.html?ID=" + response.ID;
            profile.attr("href",profileUrl);
        }
        else {
           profileUrl = web + "profile.html?ID=" + response.ID;
           profile.attr("href", profileUrl);
        }

    }
    catch (error) {
        $("#links").removeClass("hide");
        Logout();
    }

    $('#logoutBtn').click(Logout);

}

function Logout() {
    if (token != null) {
        localStorage.removeItem("jwt_token");
        
        if (window.location.href != web + "index.html") {
            window.location.href = web + "index.html";
        }
        else {
            location.reload();
        }
    }

}

function open_img(event) {
    window.open(imagesUrl + event.data.param1, "_blank");
}


function toProfile() {
    if (profileUrl) {
        window.location.href = profileUrl;
    }
    else {
        window.location.href = web + "index.html";
    }
}

function showApiMessage(message, error) {
    let div = $("<div></div>");
    let popup = $("<div></div>");
    popup.addClass("error-popup");
    div.addClass("api-error");
    let title = $("<h1></h1>").text(error);
    let text = $("<p></p>").text(message);
    let btn = $("<button>Ok</button>");
    btn.addClass("green-btn");
    btn.click(() => {
        div.remove();
    });
    text.addClass("red-text");
    popup.append(title, text,btn);
    div.append(popup);
    $("main").append(div);
}