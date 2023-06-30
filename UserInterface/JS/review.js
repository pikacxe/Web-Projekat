
$(document).ready(function () {
    checkLogin().then(function () {
        if (role != "Buyer") {
            let ID = new URL(window.location.href).searchParams.get("ID");
            if (ID != null) {
                $("#title").text("Edit user");
                $("#addBtn").text("Save");
                populateFields(ID);
                $("#addBtn").click({ id: ID }, updateReview);
            }
            else if (role == "Buyer") {
                $("#addBtn").click({}, createReview);
            }
            $('#uploadBtn').click({ id: ID }, UploadImage);
        }
        else {
            window.location.href = profileUrl;
        }
    });
});

function UploadImage(event) {
    event.preventDefault();

    if ($('#imageUpload').val() == '') {
        alert('Plase select file');
        return;
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
            $("#addBtn").removeClass("hide");
            $("#uploadBtn").addClass("hide");
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    })
}

function updateReview(event) {
    event.preventDefault();
    let imagePath = event.data.image;
    let title = $("#title").val().trim();
    let content = $("#content").val().trim();

    let req = {
        Title: title,
        Content: content,
        Image: imagePath,
        Product: parseInt(event.data.product),
        Reviewer: currentID
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
        }

    })
}

function createReview(event) {
    event.preventDefault();
    let imagePath = event.data.image;
    let title = $("#title").val().trim();
    let content = $("#content").val().trim();

    let req = {
        Title: title,
        Content: content,
        Image: imagePath,
        Product: parseInt(event.data.product),
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
        }

    })
}