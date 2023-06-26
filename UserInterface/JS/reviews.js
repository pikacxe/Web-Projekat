$(document).ready(() => {
    let ID = new URL(window.location.href).searchParams.get("ID");
    if (!ID) {
        window.location.href = web + "index.html";
    }
    $.ajax({
        url: api + "/reviews/all",
        method: "GET",
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (response) {
            console.log(response)
        },
        error: function (xhr, status, error) {
            console.log(status + "|" + error);
        }
    })
});