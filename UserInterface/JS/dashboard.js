$(document).ready(() => {
    let ID = new URL(window.location.href).searchParams.get("ID");
    if (!ID) {
        window.location.href = web + "index.html";
    }
    checkLogin();
    $("#userAddBtn").click({}, addUser);
    if (token) {
        $.ajax({
            url: api + "users/user/" + ID,
            method: 'GET',
            contentType: "application/json",
            success: function (res) {
                if (res.RoleName != "Administrator") {
                    window.location.href = web + "index.html";
                }
                $("#usrname").text(res.Username);
                $("#fname").text(res.FirstName);
                $("#lname").text(res.LastName);
                $("#gender").text(res.Gender === "m" ? "Male" : "Female");
                $("#email").text(res.Email);
                $("#dob").text(new Date(res.DateOfBirth).toLocaleDateString(dateFormatOptions));
                $("#role").text(res.RoleName);
                $("#editBtn").click({ id: res.ID }, editProfile);
                generateProductDisplay();
                generateOrdersDisplay();
                generateReviewsDisplay();
                generateUsersDisplay();

            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        })
    }
});

function addUser(event) {
    window.location.href = web + 'user.html';
}

function editProfile(event) {
    window.location.href = web + "user.html?ID=" + event.data.id;
}

function generateProductDisplay() {
    $.ajax({
        url: api + "products/all/",
        method: 'GET',
        headers: { "Authorization": token },
        contentType: "application/json",
        success: function (res) {
            if (res.length > 0) {
                let div = $(".products");
                $.each(res, function (index, product) {
                    // Create the HTML elements for each product
                    let a = $("<a></a>").addClass("product");
                    // Check if product is available
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

function generateOrdersDisplay() {
    $.ajax({
        url: api + "orders/all/",
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
                    status.attr("id", "order-status-" + order.ID);
                    let deliverBtn = $('<button></button>');
                    deliverBtn.addClass("green-btn");
                    deliverBtn.attr("id", "order-btn-" + order.ID);
                    let cancelBtn = $('<button></button>');
                    cancelBtn.addClass("green-btn");
                    cancelBtn.attr("id", "order-btn-" + order.ID);
                    let action = $('<td id="actionBtns"></td>');
                    if (order.StatusMessage == "ACTIVE") {
                        deliverBtn.text("Mark delivered");
                        deliverBtn.click({ id: order.ID, status: 1 }, OrderStatus);
                        cancelBtn.text("Cancel");
                        cancelBtn.click({ id: order.ID, status: 2 }, OrderStatus);
                        action.append(deliverBtn, cancelBtn);
                    }
                    // posible image addition for product preview
                    tr.append(id, product, amount, date, status, action);
                    table.append(tr);
                });
            }
            else {
                let table = $(".orders");
                let tr = $("<tr></tr>");
                let message = $('<td colspan=6></td>').text("No orders found");
                tr.append(message);
                table.append(tr);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}

function OrderStatus(event) {
    console.log("Complete: " + event.data.id);
    let req = {
        orderId: event.data.id,
        status: event.data.status
    }
    $.ajax({
        url: api + "orders/change-status/",
        method: 'POST',
        contentType: "application/json",
        headers: { "Authorization": token },
        data: JSON.stringify(req),
        success: function (res) {
            console.log(res);
            $("#order-status-" + event.data.id).text(res);
            $("#actionBtns").empty();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}

function generateReviewsDisplay() {
    $.ajax({
        url: api + "/reviews/need-approval",
        method: "GET",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            if (res.length > 0) {
                $.each(res, function (index, review) {
                    let div = $("<div></div>");
                    div.addClass("review");
                    let title = $("<h3></h3>").text(review.Title);
                    let image = $("<img>").attr("src", imagesUrl + review.Image);
                    image.click({ param1: review.Image }, open_img);
                    let content = $("<p></p>").text(review.Content);
                    let actions = $("<div></div>");
                    actions.attr("id", "review-" + review.ID);
                    let approveBtn = $("<button>Approve</button>");
                    let denyBtn = $("<button>Deny</button>");
                    approveBtn.addClass("green-btn");
                    denyBtn.addClass("red-btn");
                    denyBtn.click({ id: review.ID }, denyReview);
                    approveBtn.click({ id: review.ID }, approveReview);
                    actions.append(approveBtn, denyBtn);
                    // Append the elements to the container div
                    div.append(title, content, image, actions);
                    $(".reviews").append(div);
                });
            }
            else {
                let div = $(".reviews");
                let message = $("<h2></h2>").text("No reviews need approval");
                div.append(message);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    })
}

function denyReview(event) {
    event.preventDefault();
    let temp = $("#review-" + event.data.id).empty();
    let message = $("<h2></h2>");
    message.text("Denied");
    message.addClass("red-text");
    temp.append(message);
}

function approveReview(event) {
    event.preventDefault();
    $.ajax({
        url: api + "reviews/approve/" + event.data.id,
        method: "PUT",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            let temp = $("#review-" + event.data.id).empty();
            let message = $("<h2></h2>");
            message.text(res);
            message.addClass("green-text");
            temp.append(message);
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}

let users;
function generateUsersDisplay() {
    $.ajax({
        url: api + "users/all/",
        method: 'GET',
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            if (res.length > 0) {
                users = res;
                populateUserTable(users);
            }
            else {
                let table = $(".users");
                let tr = $("<tr></tr>");
                let message = $('<td colspan=6></td>').text("No users found");
                tr.append(message);
                table.append(tr);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });


}

function deleteUser(event) {
    event.preventDefault();
    $.ajax({
        url: api + "users/delete/" + event.data.id,
        method: "DELETE",
        headers: { "Authorization": token },
        success: function (response) {
            console.log(response);
            $("#user-" + event.data.id).remove();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    })
}

function populateUserTable(users) {
    $.each(users, function (index, user) {
        let table = $(".users");
        let tr = $("<tr></tr>");
        let usrname = $("<td></td>").text(user.Username);
        let fname = $("<td></td>").text(user.FirstName);
        let lname = $("<td></td>").text(user.LastName);
        let email = $("<td></td>").text(user.Email);
        let role = $("<td></td>").text(user.RoleName);
        let gender = $("<td></td>").text(user.Gender === "m" ? "Male" : "Female");
        let dob = $("<td></td>").text(new Date(user.DateOfBirth).toLocaleDateString(dateFormatOptions));
        let action = $('<td></td>');
        tr.attr("id", "user-" + user.ID);
        if (user.RoleName != "Administrator") {
            let editBtn = $('<button>Edit</button>');
            let deleteBtn = $('<button>Delete</button>');
            editBtn.addClass("green-btn");
            deleteBtn.addClass("red-btn");
            editBtn.click({ id: user.ID }, editProfile);
            deleteBtn.click({ id: user.ID }, deleteUser);
            action.append(editBtn, deleteBtn);
        }
        tr.append(usrname, fname, lname, email, role, gender, dob, action);
        table.append(tr);
    });
}