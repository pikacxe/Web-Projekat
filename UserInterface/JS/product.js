$(document).ready(function () {
    checkLogin().then(() => {
        let ID = new URL(window.location.href).searchParams.get("ID");
        if (ID != null) {
            $("#title").text("Edit product");
            let addBtn = $("#addBtn");
            addBtn.text("Save");
            addBtn.click({ id: ID }, updateProduct);
            populateFields(ID);
        }
        else if (role == "Seller") {
            $("#title").text("Add product");
            let addBtn = $("#addBtn");
            addBtn.text("Add");
            addBtn.click({}, addProduct);
            addBtn.addClass("hide");
        }
        else {
            window.location.href = profileUrl;
        }
        $('#uploadBtn').click({ id: ID }, UploadImage);
    });
});

let imagePath;

function updateProduct(event) {
    event.preventDefault();
    let product = validateProduct();
    if (product == null) {
        return;
    }
    product.ID = event.data.id;
    product.SellerId = currentID;
    if (imagePath != "") {
        product.Image = imagePath;
    }
    $.ajax({
        url: api + "products/update/",
        method: "PUT",
        contentType: "application/json",
        data: JSON.stringify(product),
        headers: { "Authorization": token },
        success: function (res) {
            window.location.href = profileUrl;
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
}

function addProduct(event) {
    event.preventDefault();
    let product = validateProduct();
    if (product == null) {
        return;
    }
    product.ID = event.data.id;
    product.SellerId = currentID;
    if (imagePath != "") {
        product.Image = imagePath;
    }
    else {
        return;
    }
    $.ajax({
        url: api + "products/add/",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(product),
        headers: { "Authorization": token },
        success: function (res) {
            window.location.href = profileUrl;
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    });
}

function populateFields(productId) {
    $.ajax({
        url: api + "products/find/" + productId,
        method: "GET",
        contentType: "application/json",
        success: function (res) {
            $("#name").val(res.Name);
            $("#price").val(res.Price);
            $("#amount").val(res.Amount);
            $("#desc").val(res.Description);
            $("#city").val(res.City);
            imagePath = res.Image;
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }

    })
}

function UploadImage(event) {
    event.preventDefault();

    if ($('#imageUpload').val() === '') {
        $("#imageUpload")[0].setCustomValidity("Please upload an image");
        $("#imageUpload")[0].reportValidity();
        return;
    }
    else {
        $("#imageUpload")[0].setCustomValidity("");
        $("#imageUpload")[0].reportValidity();
    }

    let formData = new FormData();
    let file = $('#imageUpload')[0];

    formData.append('file', file.files[0]);

    $.ajax({
        url: api + "image/add/",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: { "Authorization": token },
        success: function (response) {
            console.log(response);
            imagePath = response;
            $("#uploadBtn").addClass("hide");
            $("#addBtn").removeClass("hide");
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiError(result.Message, error);
        }
    })
}

function validateProduct(){
    let name = $("#name").val().trim();
    let price = parseFloat($("#price").val().trim());
    let amount = parseInt($("#amount").val().trim());
    let desc = $("#desc").val().trim();
    let city = $("#city").val().trim();

    let isValid = true;

    if (name === "") {
        isValid = false;
        $("#name")[0].setCustomValidity("Product name is required.");
    } else {
        $("#name")[0].setCustomValidity("");
    }
    $("#name")[0].reportValidity();

    if (isNaN(price) || price <= 0) {
        isValid = false;
        $("#price")[0].setCustomValidity("Please enter a valid price.");
    } else {
        $("#price")[0].setCustomValidity("");
    }
    $("#price")[0].reportValidity();

    if (isNaN(amount) || amount < 0) {
        isValid = false;
        $("#amount")[0].setCustomValidity("Please enter a valid amount.");
    } else {
        $("#amount")[0].setCustomValidity("");
    }
    $("#amount")[0].reportValidity();

    if (desc === "") {
        isValid = false;
        $("#desc")[0].setCustomValidity("Product description is required.");
    } else {
        $("#desc")[0].setCustomValidity("");
    }
    $("#desc")[0].reportValidity();

    if (city === "") {
        isValid = false;
        $("#city")[0].setCustomValidity("Product city is required.");
    } else {
        $("#city")[0].setCustomValidity("");
    }
    $("#city")[0].reportValidity();
    if (!isValid) {
        return null;
    }

    let product = {
        Name: name,
        Price: price,
        Amount: amount,
        Description: desc,
        City: city
    };

    return product;
}