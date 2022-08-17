$(function () {
    function addCommas(x) {
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    }
    function updateMonitoring(message) {

        var data = JSON.parse(message);
        var LineId = Number($("#Id").val());
        var dNodes = data.Nodes;

        for (var i = 0; i < dNodes.length; i++) {
            var row = dNodes[i];
            var Id = row.Id;
            $(".view-node").each(function () {
                var NodeId = Number($(this).attr('data-id'));
                if (Id == NodeId) {
                    var NodeId = '#Node_' + Id;
                    var NewEventDefId = row.EventDefId;
                    var ClassEvent = '#Event-' + NewEventDefId;

                    $(NodeId + " .node-title").attr('style', 'background:' + $(ClassEvent).attr('data-color'));
                }
            });
        }



        var dLines = data.Lines;

        var color1 = $("#Color1").val();
        var color2 = $("#Color2").val();
        var color3 = $("#Color3").val();
        var Num1 = Number($("#Num1").val());
        var Num2 = Number($("#Num2").val());
        var Num3 = Number($("#Num3").val());
        for (var i = 0; i < dLines.length; i++) {
            var row = dLines[i];
            if (row.Id == LineId) {
                var Plan = row.Plan;
                if (typeof Plan.OrderPlans == "undefined") {
                    continue;
                }
                var OrderPlans = Plan.OrderPlans;
                $(".view-node").each(function () {
                    var NodeId = $(this).attr('data-id');
                   
                    var iNodeId = Number(NodeId);
                    var totalPlan = 0;
                    var totalActual = 0;
                    var totalTarget = 0;
                    for (var j = 0; j < OrderPlans.length; j++) {
                        var r = OrderPlans[j];
                        if (r.NodeId == iNodeId) {
                            if (Number(r.QuantityProduced) < 0) {
                                r.QuantityProduced = 0;
                            }
                            totalPlan += Number(r.Quantity);
                            totalTarget += Number(r.Target);
                            totalActual += Number(r.QuantityProduced);
                        }
                    }
                    var colorNode = "";
                    var persent = 0;
                    if (totalTarget == 0) {
                        colorNode = color1;
                        persent = 0;
                    } else {
                        var per = (totalActual * 100) / totalTarget;
                        persent = parseFloat(per);
                        persent = persent > 100 ? 100 : persent;
                        if (persent < Num1) {
                            colorNode = color1;
                        } else if (persent < Num2) {
                            colorNode = color2;
                        } else {
                            colorNode = color3;
                        }
                    }
                   
                    $("#Node_" + iNodeId + " #Plan").html(addCommas(totalPlan));
                    $("#Node_" + iNodeId + " #Actual").html(addCommas(totalActual));
                    $("#Node_" + iNodeId + " #Target").html(addCommas(totalTarget));
                    $("#Node_" + iNodeId + " .knob").val(persent.toFixed(0) + "%").trigger('change');
                    $("#Node_" + iNodeId + " .knob").val(persent.toFixed(0) + "%").trigger(
                        'configure',
                        {
                            min: '0',
                            max: '100',
                            "fgColor": colorNode
                        }
                    );

                });
            }
        }
    }

    var uri = "ws://192.168.1.254:4000/ws";
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
