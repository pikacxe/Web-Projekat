const api = "http://192.168.0.13:60471/api/";
const web = "http://192.168.0.13:8888/Pages/";
const imagesUrl = "http://192.168.0.13:60471/Images/";

let token = sessionStorage.getItem("jwt_token");
let role;
let currentID;
let isLoggedIn = false;
let favourites = [];
let username;
let profileUrl;
const dateFormatOptions = {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
    hour12: false,
    hour: '2-digit',
    minute: '2-digit'
};
const dateLocale = "sr-RS";

async function checkLogin() {
    try {
        let response = await $.ajax({
            url: api + "users/current",
            type: "GET",
            headers: {
                "Authorization": token
            }
        });
        console.log(response);
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
        console.log(error);
        $("#links").removeClass("hide");
        Logout();
    }

    $('#logoutBtn').click(Logout);

}

function Logout() {
    if (token != null) {
        sessionStorage.removeItem("jwt_token");
        isLoggedIn = false;
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