$(document).ready(() => {
    if (token) {
        let ID = new URL(window.location.href).searchParams.get("ID");
        if (!ID) {
            windows.location.href = web + "index.html";
        }
        $.ajax({
            url: api + "users/user/" + ID,
            method: 'GET',
            contentType: "application/json",
            success: function (res) {
                let profile = $(".profile-data");
                let fname = $("<h3></h3>").text("First name: " + res.FirstName);
                let lname = $("<h3></h3>").text("Last name: " + res.LastName);
                let usrname = $("<h3></h3>").text("Username: " + res.Username);
                let email = $("<h3></h3>").text("Email: " + res.Email);
                let dob = $("<h3></h3>").text("Date of birth: " + new Date(res.DateOfBirth).toLocaleDateString());

                profile.append(fname, lname, usrname, email, dob);
                if (res.RoleName = "Seller") {
                    generateProductDisplay(res.ID);
                }
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        })
    }
});


function generateProductDisplay(id) {
    $.ajax({
        url: api + "products/seller/" + id,
        method: 'GET',
        headers: { "Authorization": token },
        contentType: "application/json",
        success: function (res) {
            if (res.length > 0) {
                let div = $(".items-container");
                div.removeClass("hide");
                $.each(res, function (index, product) {
                    // Check if product is available
                    // Create the HTML elements for each product
                    let a = $("<a></a>").addClass("product");
                    if (!product.isAvailable) {
                        a.addClass("unavailable");
                    }
                    a.attr('href', "details.html?ID=" + product.ID);
                    let title = $("<h3></h3>").text(product.Title);
                    let image = $("<img>").attr("src", product.Image);
                    let price = $("<p></p>").text(product.Price + "$");

                    // Append the elements to the container div
                    a.append(image, title, price);
                    div.append(a);

                });
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
}