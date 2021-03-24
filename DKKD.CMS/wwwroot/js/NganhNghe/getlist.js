var listId = [];
$(document).ready(function () {
    setDataTable();
    checkAll();
})
$('#btnXoaNhieu').on('click', function () {
    openDeleteAll();
});
function checkAll() {
    $('#checkAll').each(function () {
        $(this).click(function () {
            $('input:checkbox').not(this).prop('checked', this.checked);
            $('input[type="checkbox"]').each(function () {
                var dataId = $(this).attr("data-id-box");
                if ($(this).is(":checked")) {
                    if (dataId != undefined) {
                        listId.push(dataId);
                    }
                }
                else {
                    var index = listId.indexOf(dataId);
                    if (index > -1) {
                        listId.splice(index, 1);
                    }
                }

            })
        })
    })
    $('input[type="checkbox"]').each(function () {
        var dataId = $(this).attr("data-id-box");
        if (dataId != undefined) {
            $(this).click(function () {
                if ($(this).is(":checked")) {
                    listId.push(dataId);
                }
                else {
                    var index = listId.indexOf(dataId);
                    if (index > -1) {
                        listId.splice(index, 1);
                    }
                }
            })
        }
    })
}
function openDeleteAll() {
    showDeleteModal("Bạn có muốn xóa ngành nghề");
    $('#modal-delete').find('#btnDelete').off("click").on('click', function (e) {
        var data = listId.toString();
        if (data.length != 0) {
            $.ajax({
                url: urlDomain + "/delete-all",
                method: "Post",
                data: {
                    inputModel: data
                },
                success: function (response) {
                    hideLoading()
                    if (response.result) {
                        showAlert(response.message, 2)
                        var name = $("#txtName").val();
                        var status = $("#drStatus").val()
                        var code = $('#txtCode').val();
                        getData(name, code, status);
                        hideDeleteModal();
                    }
                    else {
                        showAlert(response.message)
                    }
                }
            })
        }
        else {
            showAlert("Bạn chưa chọn ngành nghề để xóa")
        }
    });
}

function openUpdate(id) {
    $.ajax({
        url: urlDomain + "/update?id=" + id,
        method: "Get",
        success: function (response) {
            showContentModal(response, "cập nhập ngành nghề")
        }
    });
}
function openDelete(id) {
    showDeleteModal("Bạn có muốn xóa ngành nghề");
    $('#modal-delete').find('#btnDelete').off("click").on('click', function (e) {
        e.preventDefault();
        $.ajax({
            url: urlDomain + "/delete",
            method: "POST",
            data: {
                Id:id
            },
                success: function (response) {
                hideLoading()
                if (response.result) {
                    showAlert(response.message, 2)
                    var name = $("#txtName").val();
                    var status = $("#drStatus").val()
                    var code = $('#txtCode').val();
                    getData(name, code, status);
                    hideDeleteModal();
                }
                else {
                    showAlert(response.message)
                }
            }
        });
    });
}
function setDataTable() {
    $('#tblDisplay').DataTable({
        "paging": true,
        "lengthChange": false,
        "pagingType": "full_numbers",
        "searching": false,
        "ordering": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
        "sDom": 'lrtip',
        order: [],
        columnDefs: [
            { orderable: false, targets: [0] },
            { orderable: false, targets: [3] },
            { orderable: false, targets: [4] }
        ]
        ,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "paginate": {
                "first": "Trang đầu",
                "last": "Trang cuối",
                "next": "Trang tiếp",
                "previous": "Trang trước"
            }

        }
    });
}