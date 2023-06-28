const api = "http://192.168.0.13:60471/api/";
const web = "http://192.168.0.13:8888/Pages/";

let token = sessionStorage.getItem("jwt_token");
let role;
let currentID;
let isLoggedIn = false;

$(document).ready(function () {
    if (token != null) {
        $.ajax({
            url: api + "users/current",
            type: "GET",
            headers: {
                "Authorization": token
            },
            success: function (response) {
                role = response.role;
                currentID = response.id;
                isLoggedIn = true;
                $(".link-list").addClass("hide");
                $(".dropdown").removeClass("hide");
                $("#profile-name").text(response.name + "👨‍💼");
                if (role == "Administrator") {
                    $("#profile-name").attr("href", web + "dashboard.html?ID=" + response.id);
                }
                else {
                    $("#profile-name").attr("href", web + "profile.html?ID=" + response.id);
                }
                $("#my-orders").attr("href", web + "orders.html?ID=" + response.id);
                $("#my-favourites").attr("href", web + "favourites.html?ID=" + response.id);
                $("#my-reviews").attr("href", web + "reviews.html?ID=" + response.id);
            },
            error: function (xhr, status, error) {
                $(".link-list").removeClass("hide");
                $(".dropdown").addClass("hide");
                isLoggedIn = false;
                sessionStorage.removeItem("jwt_token");
            }
        });
    }
    $('#logoutBtn').click(Logout);
});

function Logout() {
    if (token != null) {
        $(".link-list").removeClass("hide");
        $(".dropdown").addClass("hide");
        sessionStorage.removeItem("jwt_token");
        window.location.href = web + "index.html";
    }

}

function open_img(event) {
    window.open(web + event.data.param1,"_blank");
}