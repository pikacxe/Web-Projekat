

$(document).ready(() => {
    let ID = new URL(window.location.href).searchParams.get("ID");
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
            let desc = $("<label></label>").text('Description:\n '+product.Description);
            desc.addClass("grid-desc");
            let img = $("<img>").attr("src", product.Image);
            img.addClass("grid-img");
            let date = $("<label></label>").text("Date published: " + product.PublishDate);
            date.addClass("grid-date");
            let city = $("<label></label>").text("City: " + product.City);
            city.addClass("grid-city");
            // Append the elements to the container div
            $("#product-display").append(title, img, desc, date, city,price,amount);
            

        },
        error: function (xhr, status, error) {

            // Handle any errors that occur during the AJAX request
            console.error(error);
        }
    });
    $.ajax({
        url: api + "reviews/for/" + ID,
        type: "GET",
        dataType: "json",
        headers: {
            "Authorization" : token
        },
        success: function (response) {
            if (response) {
                $.each(response, function (index, review) {
                    let a = $("<a></a>").addClass("review");
                    //a.attr('href', "Pages/details.html?ID=" + review.ID);
                    let title = $("<h3></h3>").text(review.Title);
                    let image = $("<img>").attr("src",review.Image);
                    let price = $("<p></p>").text(review.Content);

                    // Append the elements to the container div
                    a.append(title, image, price);
                    $(".reviews-container").append(a);
                });
            }

        },
        error: function (xhr, status, error) {

            // Handle any errors that occur during the AJAX request
            console.error(error);
        }
    });

});