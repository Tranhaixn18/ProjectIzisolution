$(document).ready(function () {
    $(function () {
        var conn = $.connection.workOrderPlanHub;
        conn.client.getData = function (listWOP) {
            clientUpdate(listWOP);

        };
    });
    $.connection.hub.start().done(function () {
        clientUpdate();
    });
});
function clientUpdate(dataList) {
    if (dataList != undefined) {
        updateAttribute(dataList);
    } else {
        var url = "NodeOnline/Index?Id=20";
        $.post(url, function (rData) {
            updateAttribute(rData);
        });
    }
}
function updateAttribute(rData) {
    $('#planquantity').html("");
    if (rData != undefined && rData != "") {
        var s1 = 1;
        rData.forEach(c => {
            var planQuantity = "<span>"+ c.PlanQuantity +"</span>";
            $('#planquantity').append(planQuantity)
        });
    }
}