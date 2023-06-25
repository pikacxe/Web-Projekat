const api = "http://192.168.0.13:60471/api/";
const web = "http://192.168.0.13:8888/";

let token = sessionStorage.getItem("jwt_token");
let logged_in = false;
let role;

$(document).ready(function () {
    if (token) {
        $.ajax({
            url: api + "users/current",
            type: "GET",
            headers: {
                "Authorization": token
            },
            success: function (response) {
                $(".link-list").addClass("hide");
                $(".dropdown").removeClass("hide");
                $("#profile-name").text(response.name + "👨‍💼");
                $("#profile-name").attr("href", web + "Pages/profile.html?ID="+response.id);
                $("#my-orders").attr("href", web + "Pages/orders.html?ID=" + response.id);
                $("#my-favourites").attr("href", web + "Pages/favourites.html?ID=" + response.id);
                $("#my-reviews").attr("href", web + "Pages/reviews.html?ID=" + response.id);
                logged_in = true;
                role = response.role;
                console.log(role);
            },
            error: function (xhr, status, error) {
                $(".link-list").removeClass("hide");
                $(".dropdown").addClass("hide");
                logged_in = false;
            }
        });
    }
    $('#logoutBtn').click(Logout);
});



function Logout() {
    if (token != null) {
        $(".link-list").removeClass("hide");
        $(".dropdown").addClass("hide");
        sessionStorage.setItem("jwt_token", null);
    }

}