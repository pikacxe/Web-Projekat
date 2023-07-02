let products = [];
let filtered = [];
$(document).ready(() => {
    checkLogin().then(() => {
        $('#searchBtn').click(Search);
        $.ajax({
            url: api + "products/all",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                products = response;
                filtered = Array.from(products);
                populateItems(products);

            },
            error: function (xhr, status, error) {
                let result = JSON.parse(xhr.responseText);
                showApiMessage(result.Message, error);
            }
        });
    });
});

function populateItems(items) {
    if (items.length == 0) {
        let div = $(".items-container");
        div.empty();
        let message = $("<h2></h2>").text("No products found");
        div.append(message);
        return;
    }
    let max_price = 0;
    let div = $(".items-container");
    div.empty();
    $.each(items, function (index, product) {
        // Check if product is available
        if (product.isAvailable) {
            if (product.Price > max_price) {
                max_price = product.Price;
            }
            // Create the HTML elements for each product
            let a = $("<a></a>").addClass("product");
            a.attr('href', "details.html?ID=" + product.ID);
            let title = $("<h3></h3>").text(product.Name);
            let image = $("<img>").attr("src", imagesUrl + product.Image);
            let price = $("<p></p>").text(product.Price + "$");
            // Append the elements to the container div
            a.append(image, title, price);
            div.append(a);
        }
    });
    $("#max").val(max_price);
    $("#max").attr("max", max_price);
}

function Search() {
    if (products) {
        let result = products;
        let name = new RegExp($("#name").val().trim().toLowerCase());
        if (name) {
            result = result.filter(p => p.Name.toLowerCase().match(name));
        }
        let city = new RegExp($("#city").val().trim().toLowerCase());
        if (city) {
            result = result.filter(p => p.City.toLowerCase().match(city));
        }
        let min = $('#min').val();
        let max = $('#max').val();
        if (min && max) {
            result = result.filter(p => p.Price >= min && p.Price <= max);
        }
        filtered = result;
        populateItems(result);
    }
}
