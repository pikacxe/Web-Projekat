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
            let price = $("<label></label>").text("Price:" + product.Price + " $");
            price.addClass("grid-price");
            let amount = $("<label></label>").text("Available: " + product.Amount + " units");
            amount.addClass("grid-amount");
            let desc = $("<label></label>").text('Description:\n ' + product.Description);
            desc.addClass("grid-desc");
            let img = $("<img>").attr("src", product.Image);
            img.addClass("grid-img");
            let date = $("<label></label>").text("Date published: " + product.PublishDate);
            date.addClass("grid-date");
            let city = $("<label></label>").text("City: " + product.City);
            city.addClass("grid-city");
            // Append the elements to the container div
            $("#product-display").append(title, img, desc, date, city, price, amount);
            $("#btnBuy").click({ id: product.ID }, Order);

        },
        error: function (xhr, status, error) {
            history.back();
            // Handle any errors that occur during the AJAX request
            console.error(error);
        }
    });
    $.ajax({
        url: api + "reviews/for/" + ID,
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        success: function (response) {
            if (response) {
                $.each(response, function (index, review) {
                    let div = $("<div></div>").addClass("review");
                    let title = $("<h3></h3>").text(review.Title);
                    let image = $("<img>").attr("src", review.Image);
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
                    window.location.href = web + "orders.html?ID=" + currentID;
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