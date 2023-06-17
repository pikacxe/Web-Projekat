let products;

$(document).ready(() => {
    $.ajax({
        url: "http://localhost:60471/api/products/all",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response);
            products = response;
            $.each(products, function (index, product) {
                // Create the HTML elements for each product
                var div = $("<div></div>").addClass("product");;
                var title = $("<p></p>").text(product.Title);
                var image = $("<img>").attr("src", product.Image);
                var price = $("<p></p>").text(product.Price + "$");

                // Append the elements to the container div
                div.append(title, image, price);
                $(".items-container").append(div);
            });

        },
        error: function (xhr, status, error) {

            // Handle any errors that occur during the AJAX request
            console.error(error);
        }
    });


});


function ProductDetails(index) {

}