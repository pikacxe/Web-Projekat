const api = "http://192.168.0.13:60471/api/";
const web = "http://192.168.0.13:8888/Pages/";
const imagesUrl = "http://192.168.0.13:60471/Images/";

let token = sessionStorage.getItem("jwt_token");
let role;
let currentID;
let isLoggedIn = false;
let favourites = [];
const dateFormatOptions = {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
    hour12: false,
    hour: '2-digit',
    minute: '2-digit'
};
const dateLocale = "sr-RS";

$(document).ready(function () {
    $.ajax({
        url: api + "users/current",
        type: "GET",
        headers: {
            "Authorization": token
        },
        success: function (response) {
            role = response.RoleName;
            currentID = response.ID;
            favourites = Array.from(response.Favourites);
            isLoggedIn = true;
            $(".link-list").addClass("hide");
            $(".dropdown").removeClass("hide");
            $("#profile-name").text(response.Username + "👨‍💼");
            if (role == "Administrator") {
                $("#profile-name").attr("href", web + "dashboard.html?ID=" + response.ID);
            }
            else {
                $("#profile-name").attr("href", web + "profile.html?ID=" + response.ID);
            }
        },
        error: function (xhr, status, error) {
            $(".link-list").removeClass("hide");
            $(".dropdown").addClass("hide");
            isLoggedIn = false;
            sessionStorage.removeItem("jwt_token");
        }
    });
    $('#logoutBtn').click(Logout);

});

function Logout() {
    if (token != null) {
        $(".link-list").removeClass("hide");
        $(".dropdown").addClass("hide");
        sessionStorage.removeItem("jwt_token");
        window.location.href = last_url;
    }

}

function open_img(event) {
    window.open(imagesUrl + event.data.param1, "_blank");
}