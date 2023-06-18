let products;

$(document).ready(() => {
    $('#searchBtn').click(Search);
    $.ajax({
        url: "http://localhost:60471/api/products/all",
        type: "GET",
        dataType: "json",
        success: function (response) {
            products = response;
            populateItems(products);

        },
        error: function (xhr, status, error) {

            // Handle any errors that occur during the AJAX request
            alert("Error occured while proccesing request: " + error);
            console.error(error);
        }
    });


});

function populateItems(items) {
    let div = $(".items-container");
    div.empty();
    $.each(items, function (index, product) {
        // Check if product is available
        if (product.isAvailable) {
            // Create the HTML elements for each product
            let a = $("<a></a>").addClass("product");
            a.attr('href', "Pages/details.html?ID=" + product.ID);
            let title = $("<p></p>").text(product.Title);
            let image = $("<img>").attr("src", product.Image);
            let price = $("<p></p>").text(product.Price + "$");

            // Append the elements to the container div
            a.append(title, image, price);
            div.append(a);
        }
    });
}

function Search() {
    if (products) {
        let result = products;
        let name = $("#name").val();
        if (name) {
            result = result.filter(p => p.Title.startsWith(name));
        }
        let city = $("#city").val();
        if (city) {
            result = result.filter(p => p.City.startsWith(city));
        }
        let min = $('#min').val();
        let max = $('#max').val();
        if (min && max) {
            result = result.filter(p => p.Price >= min && p.Price <= max);
        }
        populateItems(result);
    }
}