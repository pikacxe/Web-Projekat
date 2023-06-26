$(document).ready(() => {
    let ID = new URL(window.location.href).searchParams.get("ID");
    $.ajax({
        url: api + "orders/for-user/" + ID,
        method: 'GET',
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            if (res.length > 0) {
                $.each(res, function (index, order) {
                    console.log(order)
                    let table = $(".orders-container");
                    let tr = $("<tr></tr>");
                    let id = $("<td></td>").text(order.ID);
                    let product = $("<td></td>").text(order.ProductName);
                    let amount = $("<td></td>").text(order.Amount);
                    let date = $("<td></td>").text(new Date(order.OrderDate).toLocaleString());
                    let status = $("<td></td>").text(order.Status);
                    let aproveBtn = $("<button>Completed</button>");
                    aproveBtn.addClass("green-btn");
                    status.attr("id", "order-" + order.ID);
                    aproveBtn.click({ id: order.ID }, OrderComplete);
                    let action = $("<td></td>");
                    action.append(aproveBtn);
                    // posible image addition for product preview
                    tr.append(id, product, amount, date, status,action);
                    table.append(tr);
                });
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });

});

function OrderComplete(event) {
    console.log("Complete: " + event.data.id);
    $.ajax({
        url: api + "orders/delivered/" + event.data.id,
        method: 'PUT',
        contentType: "application/json",
        headers: { "Authorization": token },
        success: function (res) {
            console.log(res);
            $("#order-"+event.data.id).text("DELIVERED");
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}