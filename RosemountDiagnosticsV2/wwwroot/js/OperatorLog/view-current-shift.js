document.addEventListener("DOMContentLoaded", function () {
    var token = $('#requestVerificationToken').val();
    console.log("token : " + token);
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': token
        }
    });
});

function addGoodStock() {
    var data = $('#goodStockForm').serialize();

    $.post('/OperatorLog/AddGoodStockToWaste', data,
        function (returnedData) {
            $('#goodStockToWaste').modal('toggle');
            clearForm("#goodStockToWaste");
            showSuccessAlert(returnedData.amount + " kg of " + returnedData.recipe + " added to good stock to waste.")
        }).fail(function () {
            ShowFailedAlert("Something went wrong");
        });
}

function clearForm(formId) {

    $(formId + " :input").each(function () {
        console.log($(this).id);
        $(this).val("");
    });
}

function adjustReworkTote(id, labelToAdjust) {
    var totes = $(labelToAdjust).val();
    var toteamountElement = $(labelToAdjust)

    id == "addTote" ? totes++ : totes--;

    if (totes < 0) { return; }
    toteamountElement.val(totes);

    setTimeout(function () {
        var newToteCount = $(labelToAdjust).val();
        var shiftId = $('#shiftId').html();

        if (newToteCount == totes) {

            $.post('/OperatorLog/AddReworkTote', { toteCount: newToteCount, toteType: labelToAdjust, shiftId: shiftId },
                function (returnedData) {
                    showSuccessAlert(returnedData.totesAdded + " totes were succesfully " + returnedData.addOrRemove + ".")
                    $('#reworkCount').html(returnedData.totalRework);
                }).fail(function () {
                    ShowFailedAlert("Something went wrong");
                });
        }
    }, 1000);
}

function addOperator() {
    var oper = $('#newOperator').val();

    if (oper == "") { return; }
    var shiftId = $('#shiftId').html();

    $.post('/OperatorLog/AddOperatorToShift', { shiftId: shiftId, name: oper },
        function (returnData) {
            $('#operators').html(returnData.operators);
            $('#operatorsEdit').modal('toggle');
            showSuccessAlert("Operator Added To Shift");
            $('#newOperator').val("");
            refreshOperators();
        }).fail(function () {
            ShowFailedAlert("Something went wrong");
        });
}



function updateBatchCip(checkbox) {
    var vesselType = checkbox.getAttribute('data-vessel-type');
    var vesselName = checkbox.getAttribute('data-vessel-name');
    var batchId = checkbox.getAttribute('data-batchId');
    var shiftId = checkbox.getAttribute('data-shiftId');
    var checked = checkbox.checked;
    $.post('/OperatorLog/UpdateCip', { shiftId: shiftId, vesselType: vesselType, vesselName: vesselName, batchId: batchId, washed: checked },
        function (returnData) {
            var addRemove = checked ? "added" : "removed";
            showSuccessAlert("Wash to " + vesselName + " " + addRemove);
        }).fail(function () {
            ShowFailedAlert("Something went wrong");
        });

}

function refreshOperators() {
    var shiftId = $('#shiftId').html();
    $("#operatorList").load('../OperatorLog/ListOperators', { shiftid: shiftId });
}

function refreshToteChanges() {
    
    var shiftId = $('#shiftId').html();
    $("#toteList").load('../OperatorLog/ListToteChanges', { shiftid: shiftId });
}

function removeOperator(name) {
    var shiftId = $('#shiftId').html();
    $.post('/OperatorLog/RemoveOperatorFromShift', { shiftId: shiftId, name: name },
        function (returnData) {
            $('#operators').html(returnData.operators);
            $('#operatorsEdit').modal('toggle');
            showSuccessAlert("Operator Removed From Shift");
            refreshOperators();
        }).fail(function () {
            ShowFailedAlert("Something went wrong!");
        });
}

function removeTote(toteName) {
    var shiftId = $('#shiftId').html();
    $.post('/OperatorLog/RemoveToteFromShift', { shiftId: shiftId, toteName: toteName },
        function (returnData) {
            $('#toteCount').html(returnData.toteCount);
            $('#toteChanges').modal('toggle');
            showSuccessAlert(toteName + " Removed From Shift");
            refreshToteChanges();
        }).fail(function () {
            ShowFailedAlert("Something went wrong!");
        });

}

function updateEffluent() {
    var effluent = $("#effluent").val();
    var shiftId = $('#shiftId').html();

    $.post('/OperatorLog/EditEffluent', { shiftId: shiftId, effluent: effluent },
        function (returnData) {
            $('#effluentStart').html(returnData.effluentLevel);
            showSuccessAlert("Effluent Updated");
        }).fail(function () {
            ShowFailedAlert("Something went wrong");
        })

}

function addToteToShift() {
    var totename = $('#toteSelect option:selected').text();
    var shiftId = $('#shiftId').html();
    $.post('/OperatorLog/AddToteToShift', { shiftId: shiftId, toteName: totename },
        function (returnData) {
            $('#toteCount').html(returnData.toteCount);
            showSuccessAlert("Added " + totename + " to the list");
            refreshToteChanges();
        }).fail(function () {
            ShowFailedAlert("Something went wrong!")
        });
}

function showSuccessAlert(message) {
    $('#successMessage').html(message);
    $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
        $("#success-alert").slideUp(500);
    });
}

function ShowFailedAlert(message) {
    $('#successMessage').html(message);
    $("#fail-alert").fadeTo(2000, 500).slideUp(500, function () {
        $("#success-alert").slideUp(500);
    });
}