function endStatus(Id, NodeId) {
    var confirmstop = $("#txtConfirmStoped").val();
    if (confirm(confirmstop)) {
        $.ajax({
            url: "/NodeOnline/UpdateStatus",
            data: {
                Id: Id
            },
            type: "POST",
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (result) {
                $('#loading').hide();
                window.location.replace("/NodeOnline/Index?NodeId=" + NodeId);
            },
            error: function (errormessage) {
                console.log("error", errormessage);
                alert("Có lỗi");
            }
        }).done(function () {
            $('#loading').hide();
        });
    }
}
$(function () {
    function addCommas(x) {
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    }
    function addColor(col,val1, val2, val3, val4,val5, Id, NodeId) {
        var color = 'color' + col;
        var html = '<div class="d-row">' +
                        '<div class="b2-group-chil1 " >' +
                            '   <div class="t1 ' + color + '">' + val1 + '</div>' +
                            '   <div class="t2 ' + color + '">' + val2 + '</div>' +
                            '  <div class="t2 ' + color + '">' + val3 + '</div>' +
                            ' <div class="t2 ' + color + '">' + val4 + '</div>' +
                             ' <div class="t2 ' + color + '">' + val5 + '</div>' +
                        '</div >' +
                        '<div class="b2-group-chil2"> ';
                            if (col == 1) {
                                var txtPause = $("#txtStoped").val();
                                html += '<div class="t3 colorbtn">' +
                                    '           <div class="btnend" onclick="endStatus(' + Id + ',' + NodeId + ')">' + txtPause +'</div>' +
                                    '     </div>';
                            } else {
                                html += '<div class="t3"></div> ';
                            }
       
            html +=  '</div> '+
            '       </div> ' +
            '<div class="clear"></div>';
        return html;
    }
    function updateMonitoring(message) {

        var data = JSON.parse(message);
        var _Id =Number($("#Id").val());
        var dNodes = data.Nodes;
        var dLines = data.Lines;
        //node
        var LineId = 0;
        for (var i = 0; i < dNodes.length; i++) {
            var row = dNodes[i];
            var Id = row.Id;
            if (Id == _Id) {
                LineId = row.LineId;
                var NodeId = '#Node-' + Id + ' ';
                var NewEventDefId = row.EventDefId;
                var ClassEvent = '#Event-' + NewEventDefId;

                $(NodeId + "#EventStatus").html($(ClassEvent).attr('data-name'));
                $(NodeId + "#EventStatus").attr('style', 'background:' + $(ClassEvent).attr('data-color'));
            }
        }
        for (var i = 0; i < dLines.length; i++) {
            var row = dLines[i];
            if (row.Id == LineId) {
                var Plan = row.Plan;
                var PlanStart = Plan.PlanStart;
                var PlanFinish = Plan.PlanFinish;
                var strTime = "";
                if (PlanStart.length > 14 && PlanFinish.length>14) {
                    strTime += PlanStart.substring(11, 16) + ' - ';
                    strTime += PlanFinish.substring(11, 16);
                    $(NodeId + "#TimeRange").html(strTime);
                }
                if (typeof Plan.OrderPlans == "undefined") {
                    continue;
                }
                var OrderPlans = Plan.OrderPlans;
                var TotalQuantity = 0;
                var TotalQuantityProduced = 0;
                var totalTarget = 0;
                var html = '';
                for (var j = 0; j < OrderPlans.length; j++) {
                    var r = OrderPlans[j];
                    if (r.NodeId == _Id) {
                        var Quantity = r.Quantity;
                        if (Number(r.QuantityProduced) < 0) {
                            r.QuantityProduced = 0;
                        }
                        var QuantityProduced = r.QuantityProduced;
                        //var ProductName = $("#WorkOrder-" + r.WorkOrderId).attr('data-productname');
                        var ProductName = r.ProductName;
                        TotalQuantity += Quantity;
                        TotalQuantityProduced += QuantityProduced;
                        var target = 0;
                        if (r.Target != "undefined") {
                            target = r.Target;
                        }
                        var speed = 0
                        if (r.ActualTaktTime != "undefined" && Number(r.ActualTaktTime)!=0) {
                            speed = 60 / Number(r.ActualTaktTime);
                        }
                        html += addColor(r.Status, ProductName, addCommas(Number(Quantity)), addCommas(Number(target)), addCommas(Number(QuantityProduced)), speed.toFixed(2), r.Id, _Id);

                    }
                }
                $(NodeId + '.alldata').html(html);

                $(NodeId + "#Unit").html(addCommas(TotalQuantity));
                $(NodeId + "#Target").html(addCommas(TotalQuantity));
                $(NodeId + "#Actual").html(addCommas(TotalQuantityProduced));
                var per = TotalQuantityProduced * 100 / TotalQuantity;
                      $(".knob").val(parseFloat(per).toFixed(0) + "%").trigger('change');

            }
        }
    }

    var uri = "ws://192.168.5.14:5000/ws";
    var socket = new WebSocket(uri);
    function connect() {

        socket.onopen = function (e) {
            console.log("connection open");
        };
        socket.onclose = function (e) {
            console.log("connection closed");
        }
        socket.onmessage = function (e) {
            updateMonitoring(e.data);
        }
        socket.onerror = function (e) {
            console.log(e);
        }
    }
    connect();
});
