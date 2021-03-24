var frmUpdate = $('#frmUpdate');
var trangThai;
$(document).ready(function () {
    addRequired(frmUpdate);
    if (frmUpdate.find('#ckbStatus').val() == 2) {
        frmUpdate.find('#ckbKhoa').prop('checked', true);
    }
});
$('#modal-form').find('#btnSave').off("click").on('click', function (e) {
    e.preventDefault();
    Update();

});
function Update() {
    if (ValidateForm(frmUpdate)) {
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
            Id: frmUpdate.find('#id').val(),
            TenNganhNghe: frmUpdate.find('#txtName').val(),
            MaNganhNghe: frmUpdate.find('#txtCode').val(),
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