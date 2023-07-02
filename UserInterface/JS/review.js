
$(document).ready(function () {
    checkLogin().then(() => {
        if (role != "Buyer") {
            window.location.href = profileUrl;
        }
        let params = new URL(window.location.href).searchParams;
        let reviewId = params.get("reviewId");
        let productId = params.get("productId");
        if (reviewId != null) {
            $("#title").text("Edit review");
            $("#addBtn").text("Save");
            populateFields(reviewId);
            $("#addBtn").click({ id: reviewId, product: productId }, updateReview);
        }
        else {
            $("#addBtn").click({ id: productId }, createReview);
            $("#addBtn").addClass("hide");
        }
        $('#uploadBtn').click({}, UploadImage);
    });
});
let imagePath = "";
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
        }
    })
}
let reviewer;
function populateFields(reviewId) {
    $.ajax({
        url: api + "reviews/find/" + reviewId,
        method: "GET",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            $("#name").val(res.Title);
            $("#content").text(res.Content);
            reviewer = res.Reviewer;
            imagePath = res.Image
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }
    })
}

function updateReview(event) {
    event.preventDefault();
    let title = $("#name").val().trim();
    let content = $("#content").val().trim();

    if (!validateReview()) {
        return;
    }

    let req = {
        ID:parseInt(event.data.id),
        Title: title,
        Content: content,
        Product: parseInt(event.data.product),
        Reviewer: reviewer
    }
    if (imagePath != "") {
        req.Image = imagePath;
    }
    console.log(req);
    $.ajax({
        url: api + "reviews/update/",
        type: 'PUT',
        data: JSON.stringify(req),
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (response) {
            window.location.href = profileUrl;
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }

    })
}

function createReview(event) {
    event.preventDefault();
    let title = $("#name").val().trim();
    let content = $("#content").val().trim();

    if (!validateReview()) {
        return;
    }
    let req = {
        Title: title,
        Content: content,
        Image: imagePath,
        Product: parseInt(event.data.id),
        Reviewer: currentID
    }
    console.log(req);
    $.ajax({
        url: api + "reviews/add/",
        type: 'POST',
        data: JSON.stringify(req),
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (response) {
            window.location.href = profileUrl;
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            let result = JSON.parse(xhr.responseText);
            showApiMessage(result.Message, error);
        }
    })
}

function validateReview() {
    let isValid = true;
    if ($("#name").val().trim() === "") {
        $("#name")[0].setCustomValidity("Title is required!");
        isValid = false;
    } else {
        $("#name")[0].setCustomValidity("");
    }
    $("#name")[0].reportValidity();

    if ($("#content").val().trim() === "") {
        $("#content")[0].setCustomValidity("Content is required!");
        isValid = false;
    } else {
        $("#content")[0].setCustomValidity("");
    }
    $("#content")[0].reportValidity();
    return isValid;
}
