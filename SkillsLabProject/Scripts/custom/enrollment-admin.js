$(function () {
    var currentDepartment = $('#departments').val()
    showTableData(currentDepartment);

    $('#departments').change(function () {
        showTableData($(this).val());
    });
});

function showTableData(selectedDepartmentId) {
    $('#enrollmentTable tbody tr').hide();
    var filteredRows = _.filter($('#enrollmentTable tbody tr'), function (row) {
        var rowDepartmentId = $(row).data('departmentid');
        return rowDepartmentId == selectedDepartmentId;
    });
    $(filteredRows).show();

    if (filteredRows.length > 0) {
        $(filteredRows).show();
    }
    else {
        $('#enrollmentTable .dataTables_empty').html('<p>No Enrollments</p><img src="~/Content/Images/no-enrollment.png" class="w-50" alt="No Data">');
    }
}