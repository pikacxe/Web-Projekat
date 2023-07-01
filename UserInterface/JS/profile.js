$(document).ready(() => {
    checkLogin().then(() => {
        let ID = new URL(window.location.href).searchParams.get("ID");
        if (!ID) {
            windows.location.href = web + "index.html";
        }
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
                $("#dob").text(new Date(res.DateOfBirth).toLocaleDateString(dateLocale));
                $("#role").text(res.RoleName);
                $("#editBtn").click({ id: res.ID }, editProfile);
                if (res.RoleName == "Buyer") {
                    $("#product-title").text("My favourites 📦");
                    generateOrdersDisplay(res.ID);
                    generateReviewsDisplay(res.ID);
                    $("#productAddBtn").remove();
                }
                else {
                    $("#reviews").addClass("hide");
                    $("#orders").addClass("hide");
                    $("#productAddBtn").click({}, addProduct);
                }
                generateProductDisplay(res.ID);

            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
                let result = JSON.parse(xhr.responseText);
                showApiError(result.Message, error);
            }
        })

    });
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
                    let productDiv = $("<div></div>").addClass("product");
                    productDiv.attr("id", "product-" + product.ID);
                    let text = $("<div></div>");
                    let title = $("<h3></h3>").text(product.Name);
                    let image = $("<img>").attr("src", imagesUrl + product.Image);
                    image.click({ param1: product.Image }, open_img);
                    let price = $("<p></p>").text(product.Price + "$");
                    let action = $("<div></div>");
                    action.addClass("product-action");
                    text.append(title, price);
                    text.click({ id: product.ID }, productDetails);
                    if (role == "Seller") {
                        let editBtn = $("<button>📝</button>");
                        editBtn.addClass("icon-btn");
                        editBtn.click({ id: product.ID }, editProduct);
                        let deleteBtn = $("<button>❌</button>");
                        deleteBtn.addClass("icon-btn");
                        deleteBtn.click({ id: product.ID }, deleteProduct);
                        action.append(editBtn, text, deleteBtn);
                    }
                    else {
                        let placeholder = $("<span>&nbsp</span>");
                        let placeholder1 = $("<span>&nbsp</span>");
                        action.append(placeholder, text, placeholder1);
                    }

                    if (!product.isAvailable) {
                        productDiv.addClass("unavailable");
                    }

                    // Append the elements to the container div
                    productDiv.append(image, action);
                    div.append(productDiv);
                });
            }
            else {
                let div = $(".products");
                let message = $("<h2></h2>").text("No products found");
                div.append(message);
            }
        },
        error: function (xhr, status, error) {
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    })
}

function productDetails(event) {
    window.location.href = web + "details.html?ID=" + event.data.id;
}

function deleteProduct(event) {
    event.preventDefault();
    $.ajax({
        url: api + "products/delete/" + event.data.id,
        method: "DELETE",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            $("#product-" + event.data.id).remove();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
}
function editProduct(event) {
    window.location.href = web + "product.html?ID=" + event.data.id;
}

function addProduct(event) {
    window.location.href = web + "product.html";
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
                    let date = $("<td></td>").text(new Date(order.OrderDate).toLocaleString(dateLocale, dateFormatOptions));
                    let status = $("<td></td>").text(order.StatusMessage);
                    let actionBtn = $('<button></button>');
                    actionBtn.addClass("green-btn");
                    status.attr("id", "order-status-" + order.ID);
                    actionBtn.attr("id", "order-btn-" + order.ID);
                    if (order.StatusMessage == "COMPLETED") {
                        actionBtn.text("Add review");
                        actionBtn.click({ id: order.Product }, LeaveReview);
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
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
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
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
}

function LeaveReview(event) {
    window.location.href = web + "review.html?productId=" + event.data.id;
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
                    let actions = $("<div></div>");
                    div.attr("id", "review-" + review.ID);
                    let editBtn = $("<button>Edit 📝</button>");
                    let deleteBtn = $("<button>Delete ❌</button>");
                    editBtn.addClass("green-btn");
                    deleteBtn.addClass("red-btn");
                    deleteBtn.click({ id: review.ID }, deleteReview);
                    editBtn.click({ id: review.ID, product: review.Product }, editReview);
                    actions.append(editBtn, deleteBtn);
                    // Append the elements to the container div
                    div.append(title, content, image, actions);
                    if (review.isApproved) {

                    }
                    else {
                        div.addClass("unavailable");
                    }
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
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    })
}

function editReview(event) {
    window.location.href = web + "review.html?reviewId=" + event.data.id + "&productId=" + event.data.product;
}

function deleteReview(event) {
    event.preventDefault();
    $.ajax({
        url: api + "reviews/delete/" + event.data.id,
        method: "DELETE",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            $("#review-" + event.data.id).remove();
        },
        error: function (xhr, statis, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    })
}