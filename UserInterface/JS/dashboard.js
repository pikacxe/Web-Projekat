let filtered = [];
let products = [];
let filteredUsers = [];
let usersList = []
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
                $("#dob").text(new Date(res.DateOfBirth).toLocaleDateString(dateLocale));
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
            products = res;
            filtered = Array.from(products);
            populateItems(filtered);
        },
        error: function (xhr, status, error) {
            let div = $(".products");
            let message = $("<h1></h1>").text(error);
            div.append(message);
        }
    })
}

function populateItems(items) {
    let div = $(".products");
    div.empty();
    if (items.length > 0) {
        $.each(items, function (index, product) {
            // Create the HTML elements for each product
            let productDiv = $("<div></div>").addClass("product");
            if (!product.isAvailable) {
                productDiv.addClass("unavailable");
            }
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
            let editBtn = $("<button>📝</button>");
            editBtn.addClass("icon-btn");
            editBtn.click({ id: product.ID }, editProduct);
            let deleteBtn = $("<button>❌</button>");
            deleteBtn.addClass("icon-btn");
            deleteBtn.click({ id: product.ID }, deleteProduct);
            action.append(editBtn, text, deleteBtn);

            // Append the elements to the container div
            productDiv.append(image, action);
            div.append(productDiv);
        });
    }
    else {
        let message = $("<h2></h2>").text("No products found");
        div.append(message);
    }
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
        }
    });
}
function editProduct(event) {
    window.location.href = web + "product.html?ID=" + event.data.id;
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
                    let date = $("<td></td>").text(new Date(order.OrderDate).toLocaleDateString(dateLocale));
                    let status = $("<td></td>").text(order.StatusMessage);
                    status.attr("id", "order-status-" + order.ID);
                    let deliverBtn = $('<button></button>');
                    deliverBtn.addClass("green-btn");
                    deliverBtn.attr("id", "order-btn-" + order.ID);
                    let cancelBtn = $('<button></button>');
                    cancelBtn.addClass("green-btn");
                    cancelBtn.attr("id", "order-btn-" + order.ID);
                    let action = $('<td></td>');
                    action.attr("id", "actionBtns-" + order.ID);
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
            $("#actionBtns-" + event.data.id).empty();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}

function generateReviewsDisplay() {
    $.ajax({
        url: api + "/reviews/all",
        method: "GET",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            if (res.length > 0) {
                $.each(res, function (index, review) {
                    let div = $("<div></div>");
                    div.attr("id", "review-" + review.ID);
                    div.addClass("review");
                    let title = $("<h3></h3>").text(review.Title);
                    let image = $("<img>").attr("src", imagesUrl + review.Image);
                    image.click({ param1: review.Image }, open_img);
                    let content = $("<p></p>").text(review.Content);
                    let actions = $("<div></div>");
                    if (!review.isApproved && !review.isDenied) {
                        actions.attr("id", "reviewBtn-" + review.ID);
                        let approveBtn = $("<button>Approve</button>");
                        let denyBtn = $("<button>Deny</button>");
                        approveBtn.addClass("green-btn");
                        denyBtn.addClass("red-btn");
                        denyBtn.click({ id: review.ID }, denyReview);
                        approveBtn.click({ id: review.ID }, approveReview);
                        actions.append(approveBtn, denyBtn);
                        // Append the elements to the container div
                        div.append(title, content, image, actions);
                        $("#approved").append(div);
                    }
                    else if (review.isDenied) {
                        let message = $("<h2></h2>");
                        message.text("Denied");
                        message.addClass("red-text");
                        actions.append(message);
                        div.append(title, content, image, actions);
                        $("#denied").append(div);
                    }

                });
            }
            else {
                let div = $(".reviews");
                let message = $("<h2></h2>").text("No reviews available");
                div.append(message);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }
    })
}

function denyReview(event) {
    event.preventDefault();
    $.ajax({
        url: api + "reviews/deny/" + event.data.id,
        method: "PUT",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            let temp = $("#reviewBtn-" + event.data.id).empty();
            let message = $("<h2></h2>");
            message.text("Denied");
            message.addClass("red-text");
            temp.append(message);
            let denied = $("#review-" + event.data.id);
            denied.remove();
            $("#denied").append(denied);
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }
    });
}

function approveReview(event) {
    event.preventDefault();
    $.ajax({
        url: api + "reviews/approve/" + event.data.id,
        method: "PUT",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            $("#review-" + event.data.id).remove();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }
    });
}

function generateUsersDisplay() {
    $.ajax({
        url: api + "users/all/",
        method: 'GET',
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            if (res.length > 0) {
                usersList = res;
                filteredUsers = Array.from(usersList);
                populateUserTable(usersList);
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
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
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
            location.reload();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }
    })
}

function populateUserTable(users) {
    let table = $(".users tbody");
    table.empty();
    if (users.length == 0) {
        let tr = $("<tr></tr>");
        let message = $('<td colspan=6></td>').text("No users found");
        tr.append(message);
        table.append(tr);
    }
    else {
        $.each(users, function (index, user) {
            let tr = $("<tr></tr>");
            let usrname = $("<td></td>").text(user.Username);
            let fname = $("<td></td>").text(user.FirstName);
            let lname = $("<td></td>").text(user.LastName);
            let email = $("<td></td>").text(user.Email);
            let role = $("<td></td>").text(user.RoleName);
            let gender = $("<td></td>").text(user.Gender === "m" ? "Male" : "Female");
            let dob = $("<td></td>").text(new Date(user.DateOfBirth).toLocaleDateString(dateLocale));
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
}

function searchUsers() {
    let fname = new RegExp($("#searchFName").val().trim().toLowerCase());
    let lname = new RegExp($("#searchLName").val().trim().toLowerCase());
    let dobMin = new Date($("#minDate").val());
    let dobMax = new Date($("#maxDate").val());
    let role = $("#searchRole").val();
    filteredUsers = usersList;
    if (fname) {
        filteredUsers = filteredUsers.filter(x => x.FirstName.toLowerCase().match(fname));
    }
    if (lname) {
        filteredUsers = filteredUsers.filter(x => x.LastName.toLowerCase().match(lname));
    }
    if (!isNaN(dobMin) && !isNaN(dobMax)) {
        filteredUsers = filteredUsers.filter(x => new Date(x.DateOfBirth) >= dobMin && new Date(x.DateOfBirth) <= dobMax);
    }
    if (role != -1) {
        filteredUsers = filteredUsers.filter(x => x.Role == role);
    }
    populateUserTable(filteredUsers);
}