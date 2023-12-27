$(function () {
    var currentDepartment = $('#departments');
    var trainingStatus = $('#trainingStatus');
    var enrollmentHeading = $('#enrollmentHeading');

    showTableData(currentDepartment.val(), enrollmentHeading.text() === "Closed Enrollments");
    

    currentDepartment.change(function () {
        showTableData($(this).val(), enrollmentHeading.text() === "Closed Enrollments");
    });

    trainingStatus.click(function () {
        enrollmentHeading.text(enrollmentHeading.text() === "Open Enrollments" ? "Closed Enrollments" : "Open Enrollments");
        showTableData(currentDepartment.val(), enrollmentHeading.text() === "Closed Enrollments");
    });
});

function showTableData(selectedDepartmentId, isClosed) {
    $('#enrollmentTable tbody tr').hide();

    var filteredRows = _.filter($('#enrollmentTable tbody tr'), function (row) {
        var rowDepartmentId = $(row).data('departmentid');
        var trainingIsClosed = $(row).data('trainingclosed').toString() === 'True';

        return rowDepartmentId == selectedDepartmentId && trainingIsClosed == isClosed;
    });

    $(filteredRows).show();
}
