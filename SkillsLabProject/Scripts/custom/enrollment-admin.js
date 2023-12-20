$(function () {
    showTableData;
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

    // Check if there are any filtered rows
    if (filteredRows.length > 0) {
        $(filteredRows).show();
        $('#noDataImage').remove();
    } else {
        $('#enrollmentTable tbody').append('<tr id="noDataImage"><td colspan="9" class="text-center"><p>No Data</p></td></tr>');
    }
}