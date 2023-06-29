
$(document).ready(function () {
    let ID = new URL(window.location.href).searchParams.get("ID");
    if (!ID) {
        windows.location.href = web + "index.html";
    }
    $('#uploadBtn').click({ id:  ID}, UploadImage);
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
            let button = $('#uploadBtn');
            button.text("Create review");
            button.click({ image: response, product: event.data.id }, CreateReview);
            button.off("click", UploadImage);

        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
}

function CreateReview(event) {
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
    // TODO ajax req to create new review

    $.ajax({
        url: api + "reviews/add/",
        type: 'POST',
        data: JSON.stringify(req),
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (response) {
            window.location.href = web + "profile.html?ID=" + currentID;
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }

    })
    console.log("Radim");

}