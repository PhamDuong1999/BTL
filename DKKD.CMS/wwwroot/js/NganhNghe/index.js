var urlDomain = "/nganh-nghe";
$(document).ready(function () {
    getData("", "", -1);
    searching();
    
})
function searching() {
    $("#btnSearch").on('click', function () {
        var name = $("#txtName").val();
        var code = $('#txtCode').val();
        var status = $("#drStatus").val()
        getData(name, code, status);
    });
}
$('#btnCreate').on('click', function () {
    openCreate();
});
function getData(name, code, status) {
    showLoading();
    $.ajax({
        url: urlDomain + "/get-list/?name=" + name + "&code=" + code + "&status=" + status,
        method: "GET",
        success: function (response) {
            $('#dtTable').html(response);
            hideLoading();
        }
    });
}

function openCreate() {
    $.ajax({
        url: urlDomain + "/create",
        method: "Get",
        success: function (response) {
            showContentModal(response, "TẠO MỚI CHỨC VỤ")
        }
    });
}
