"use strict"

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/batchhub")
    .build();

connection.on("LastBatch", function (message) {
    //var currentdate = new Date();
    //var timeNow = currentdate.getDay() + "/" + currentdate.getMonth() + "/" + currentdate.getFullYear() + " @ " + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
    
    var json = JSON.parse(message);
    
    document.getElementById("lastbatch-recipe").innerHTML = json["Recipe"];
    document.getElementById("lastbatch-batchno").innerHTML = json["Campaign"] + "-" + json["BatchNo"];
    document.getElementById("lastbatch-visco-hidden").innerHTML = json["Visco"];
    document.getElementById("lastbatch-maketime").innerHTML = json["MakingTime"];
    document.getElementById("lastbatch-stoppage").innerHTML = json["Stoppages"];
    document.getElementById("lastbatch-quality").innerHTML = json["QualityIssues"];
    document.getElementById("lastbatch-matvar").innerHTML = json["MatVarCost"];
    document.getElementById("IsSyncronising").innerHTML = json["Sync"];
    document.getElementById("lastbatch-recipetype-hidden").innerHTML = json["RecipeType"];
    document.getElementById("lastbatch-batchId-hidden").innerHTML = json["BatchReportId"];
    document.getElementById("batches-made-weekNo").innerHTML = json["WeekNo"];
    
    
    
});

connection.on("BatchesMadeByCategory", function (message) {
    var counts = JSON.parse(message);
    document.getElementById("totalThisWeek").innerHTML = counts["BatchesMade"]
    document.getElementById("concBatchCount").innerHTML = counts["ConcBatchesCount"]
    document.getElementById("bigBangbatchCount").innerHTML = counts["BigBangBatchesCount"]
    document.getElementById("regBatchCount").innerHTML = counts["RegBatchesCount"]

});

connection.start().catch(function (err) {
    return console.error(err.toString());
});


