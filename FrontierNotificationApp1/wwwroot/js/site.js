// Write your JavaScript code.
$(".table td").each(function () {
    var thisCell = $(this);
    var cellStatus = thisCell.text();

    if (cellStatis == "Open") {
        thisCell.css("background-color", "red");
    }
}
);
