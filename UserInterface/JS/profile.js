$(document).ready(() => {
    checkLogin();
    let ID = new URL(window.location.href).searchParams.get("ID");
    if (!ID) {
        windows.location.href = web + "index.html";
    }
    if (token) {
        $.ajax({
            url: api + "users/user/" + ID,
            method: 'GET',
            contentType: "application/json",
            success: function (res) {
                $("#usrname").text(res.Username);
                $("#fname").text(res.FirstName);
                $("#lname").text(res.LastName);
                $("#gender").text(res.Gender === "m" ? "Male" : "Female");
                $("#email").text(res.Email);
                $("#dob").text(new Date(res.DateOfBirth).toLocaleDateString(dateFormatOptions));
                $("#role").text(res.RoleName);
                $("#editBtn").click({ id: res.ID }, editProfile);
                if (res.RoleName == "Buyer") {
                    $("#product-title").text("My favourites 📦");
                    generateOrdersDisplay(res.ID);
                    generateReviewsDisplay(res.ID);
                }
                else {
                    $("#reviews").addClass("hide");
                    $("#orders").addClass("hide");
                }
                generateProductDisplay(res.ID);

            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        })
    }
});

function editProfile(event) {
    window.location.href = web + "user.html?ID=" + currentID;
}

function generateProductDisplay(id) {
    $.ajax({
        url: api + "products/user/" + id,
        method: 'GET',
        headers: { "Authorization": token },
        contentType: "application/json",
        success: function (res) {
            if (res.length > 0) {
                let div = $(".products");
                $.each(res, function (index, product) {
                    // Check if product is available
                    // Create the HTML elements for each product
                    let a = $("<a></a>").addClass("product");
                    if (!product.isAvailable) {
                        a.addClass("unavailable");
                    }
                    a.attr('href', "details.html?ID=" + product.ID);
                    let title = $("<h3></h3>").text(product.Title);
                    let image = $("<img>").attr("src", imagesUrl + product.Image);
                    let price = $("<p></p>").text(product.Price + "$");

                    // Append the elements to the container div
                    a.append(image, title, price);
                    div.append(a);
                });
            }
            else {
                let div = $(".products");
                let message = $("<h2></h2>").text("No products found");
                div.append(message);
            }
        },
        error: function (xhr, status, error) {
            let div = $(".products");
            let message = $("<h1></h1>").text(error);
            div.append(message);
        }
    })
}

function generateOrdersDisplay(id) {
    $.ajax({
        url: api + "orders/for-user/" + id,
        method: 'GET',
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            if (res.length > 0) {
                $.each(res, function (index, order) {
                    let table = $(".orders");
                    let tr = $("<tr></tr>");
                    let id = $("<td></td>").text(order.ID);
                    let product = $("<td></td>").text(order.ProductName);
                    let amount = $("<td></td>").text(order.Amount);
                    let date = $("<td></td>").text(new Date(order.OrderDate).toLocaleString(dateFormatOptions));
                    let status = $("<td></td>").text(order.StatusMessage);
                    let actionBtn = $('<button></button>');
                    actionBtn.addClass("green-btn");
                    status.attr("id", "order-status-" + order.ID);
                    actionBtn.attr("id", "order-btn-" + order.ID);
                    if (order.StatusMessage == "COMPLETED") {
                        actionBtn.text("Add review");
                        actionBtn.click({ id: order.ID }, LeaveReview);
                    }
                    else if (order.StatusMessage == "ACTIVE") {
                        actionBtn.text("Mark delivered");
                        actionBtn.click({ id: order.ID, product: order.Product }, OrderComplete);
                    }
                    let action = $("<td></td>");
                    action.append(actionBtn);
                    // posible image addition for product preview
                    tr.append(id, product, amount, date, status, action);
                    table.append(tr);
                });
            }
            else {
                let div = $(".orders");
                let message = $('<td colspan=6></td>').text("No orders found");
                div.append(message);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}

function OrderComplete(event) {
    console.log("Complete: " + event.data.id);
    $.ajax({
        url: api + "orders/delivered/" + event.data.id,
        method: 'PUT',
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            $("#order-status-" + event.data.id).text(res);
            let btn = $("#order-btn-" + event.data.id);
            btn.text("Add review");
            btn.click({ id: event.data.product }, LeaveReview);
            btn.off("click", OrderComplete);

        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}

function LeaveReview(event) {
    window.location.href = web + "review.html?ID=" + event.data.id;
}

function generateReviewsDisplay(id) {
    $.ajax({
        url: api + "/reviews/for-user/" + id,
        method: "GET",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            if (res.length > 0) {
                $.each(res, function (index, review) {
                    let div = $("<div></div>").addClass("review");
                    let title = $("<h3></h3>").text(review.Title);
                    let image = $("<img>").attr("src", imagesUrl + review.Image);
                    image.click({ param1: review.Image }, open_img);
                    let content = $("<p></p>").text(review.Content);
                    // Append the elements to the container div
                    div.append(title, content, image);
                    $(".reviews").append(div);
                });
            }
            else {
                let div = $(".reviews");
                let message = $("<h2></h2>").text("No reviews found");
                div.append(message);
            }
        },
        error: function (xhr, status, error) {
            console.log(status + "|" + error);
        }
    })
}