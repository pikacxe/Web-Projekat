$(document).ready(() => {
    let ID = new URL(window.location.href).searchParams.get("ID");
    if (!ID) {
        windows.location.href = web + "index.html";
    }
    $.ajax({
        url: api + "products/find/" + ID,
        type: "GET",
        dataType: "json",
        success: function (product) {
            // Create the HTML elements for each product
            let title = $("<h1></h1>").text(product.Title);
            title.addClass("grid-title");
            let price = $("<p></p>").text("Price:\t" + product.Price + " $");
            price.addClass("grid-price");
            $("#product-amount").text("(Available:\t" + product.Amount + " units)");
            let desc = $("<p></p>").text('Description\n ' + product.Description);
            desc.addClass("grid-desc");
            let img = $("<img>").attr("src",imagesUrl + product.Image);
            img.click({ param1: product.Image }, open_img);
            img.addClass("grid-img");
            let date = $("<p></p>").text("Date published\n " + new Date(product.PublishDate).toLocaleDateString(dateLocale));
            date.addClass("grid-date");
            let city = $("<p></p>").text("City: " + product.City);
            city.addClass("grid-city");
            // Append the elements to the container div
            $("#product-display").append(title, img, desc, date, city, price);
            $("#btnBuy").click({ id: product.ID }, Order);
            $("#btnFav").click({ id: product.ID }, AddToFavourites);
            if (token) {
                $("#btnFav").removeClass("hide");
                if (favourites.includes(product.ID)) {
                    $("#btnFav").addClass("favourite");
                }
            }

        },
        error: function (xhr, status, error) {
            history.back();
            // Handle any errors that occur during the AJAX request
            console.error(error);
        }
    });
    $.ajax({
        url: api + "reviews/for-product/" + ID,
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        success: function (response) {
            if (response.length > 0) {
                $.each(response, function (index, review) {
                    let div = $("<div></div>").addClass("review");
                    let title = $("<h3></h3>").text(review.Title);
                    let image = $("<img>").attr("src",imagesUrl +  review.Image);
                    image.click({ param1: review.Image }, open_img);
                    let content = $("<p></p>").text(review.Content);
                    // Append the elements to the container div
                    div.append(title, content, image);
                    $(".reviews-container").append(div);
                });
            }

        },
        error: function (xhr, status, error) {

            // Handle any errors that occur during the AJAX request
            console.error(error);
        }
    });

});

function Order(event) {
    if (token && isLoggedIn) {
        let product = event.data.id;
        let amount = $("#buy-amount").val();
        let buyer = currentID;
        let order = {
            Product: product,
            Amount: amount,
            Buyer: buyer
        };
        if (order.Amount > 0) {
            $.ajax({
                url: api + "orders/add",
                method: 'POST',
                contentType: "application/json",
                headers: { "Authorization": token },
                data: JSON.stringify(order),
                success: function (res) {
                    console.log(res);
                    window.location.href = web + "profile.html?ID=" + currentID;
                },
                error(xhr, status, error) {
                    console.log(error);
                }

            });
        }
        else {
            // validation error
            alert("Invalid amount!");
        }
    } else {
        window.location.href = web + "login.html";
    }
}

function AddToFavourites(event) {
    if (token) {
        let product = event.data.id;
        let user = currentID;
        let req = {
            UserId: user,
            ProductId: product
        };
        $.ajax({
            url: api + "products/add-to-favourites/",
            method: 'PUT',
            contentType: "application/json",
            headers: { "Authorization": token },
            data: JSON.stringify(req),
            success: function (res) {
                favourites = Array.from(res);
                if (favourites.includes(product)) {
                    window.location.href = web + "profile.html?ID=" + currentID;
                }
                else {
                    $("btnFav").removeClass("favourite");
                }
            },
            error(xhr, status, error) {
                console.log(error);
            }
        });
    } else {
        window.location.href = web + "login.html";
    }
}