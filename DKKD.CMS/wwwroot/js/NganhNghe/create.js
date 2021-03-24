var frmCreate = $('#frmCreate');
var trangThai;
$(document).ready(function () {
    addRequired(frmCreate);
});
$('#modal-form').find('#btnSave').off("click").on('click', function (e) {
    e.preventDefault();
    create();
});
function create() {
    if (ValidateForm(frmCreate)) {
        return;
    }
    $.each($("input[name='on_offd']:checked"), function () {
        trangThai = $(this).val();
    });
    showLoading();
    $.ajax({
        url: urlDomain + "/create-or-update",
        method: "POST",
        data: {
            TenNganhNghe: frmCreate.find('#txtName').val(),
            MaNganhNghe: frmCreate.find('#txtCode').val(),
            trangThai: trangThai
        }
        , success: function (response) {
            hideLoading()
            if (response.result) {
                showAlert(response.message, 2)
                var name = $("#txtName").val();
                var status = $("#drStatus").val()
                var code = $('#txtCode').val();
                getData(name, code, status);
                hideContentModal();
            }
            else {
                showAlert(response.message)
            }
        }
    });
}