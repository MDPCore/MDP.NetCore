$(function () {

    // table th-select-all
    $(".table .th-select-all input:checkbox").click(function (e) {
        $(this).closest("table").find("tr").find("td:first input:checkbox").prop("checked", this.checked);
    });
    $(".table .th-select-all input:checkbox").closest("table").find("tr").find("td:first input:checkbox").click(function (e) {
        $(this).closest("table").find("th:first input:checkbox").prop("checked", false);
    });
});