$(document).ready(function () {
    // Store the original data
    var originalData = $('#enrollmentTable tbody tr').clone();

    // DataTable initialization
    var enrollmentTable = $('#enrollmentTable').DataTable({
        order: [[1, 'desc']],
        columnDefs: [
            { orderable: false, targets: [-1] }
        ],
        language: {
            paginate: {
                previous: '<span class="fa fa-chevron-left"></span>',
                next: '<span class="fa fa-chevron-right"></span>'
            },
            lengthMenu: 'Number of Rows: <select class="form-control input-sm mb-3">' +
                '<option value="10">10</option>' +
                '<option value="25">25</option>' +
                '<option value="50">50</option>' +
                '<option value="-1">All</option>' +
                '</select>'
        },
    });

    var currentDepartment = $('#departments');
    var trainingStatus = $('#trainingStatus');
    var enrollmentHeading = $('#enrollmentHeading');

    showTableData(originalData, enrollmentTable, currentDepartment.val(), enrollmentHeading.text() === "Closed Enrollments");

    currentDepartment.change(function () {
        showTableData(originalData, enrollmentTable, $(this).val(), enrollmentHeading.text() === "Closed Enrollments");
    });

    trainingStatus.click(function () {
        enrollmentHeading.text(enrollmentHeading.text() === "Open Enrollments" ? "Closed Enrollments" : "Open Enrollments");
        trainingStatus.text(trainingStatus.text() == "Open" ? "Closed" : "Open");
        showTableData(originalData, enrollmentTable, currentDepartment.val(), enrollmentHeading.text() === "Closed Enrollments");
    });
});

function showTableData(originalData, enrollmentTable, selectedDepartmentId, isClosed) {
    var filteredRows = _.cloneDeep(originalData);

    filteredRows = _.filter(filteredRows, function (row) {
        var rowDepartmentId = $(row).data('departmentid');
        var trainingIsClosed = $(row).data('trainingclosed') === 'closed';

        return rowDepartmentId == selectedDepartmentId && trainingIsClosed == isClosed;
    });

    enrollmentTable.clear().rows.add(filteredRows).draw();
}
