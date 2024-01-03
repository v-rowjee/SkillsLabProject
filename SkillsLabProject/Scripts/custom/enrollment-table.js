$(document).ready(function () {
    var originalData = $('#enrollmentTable tbody tr').clone();

    var enrollmentTable = $('#enrollmentTable').DataTable({
        order: [[1, 'desc']],
        columnDefs: [
            { orderable: false, targets: [-1] },
        ],
        pageLength: 7,
        language: {
            paginate: {
                previous: '<span class="fa fa-chevron-left"></span>',
                next: '<span class="fa fa-chevron-right"></span>'
            },
            lengthMenu: 'Number of Rows: <select class="form-control input-sm mb-3">' +
                '<option value="7">7</option>' +
                '<option value="15">15</option>' +
                '<option value="25">25</option>' +
                '<option value="-1">All</option>' +
                '</select>',
        },
    });

    var currentDepartment = $('#departments');
    var enrollmentStatus = $('input[name="enrollmentStatus"]');
    var trainingStatus = $('input[name="trainingStatus"]');
    var currentTraining = $('#trainings');

    currentDepartment.change(function () {
        showTableData();
    });

    enrollmentStatus.change(function () {
        enrollmentStatus = $('input[name="enrollmentStatus"]:checked');
        showTableData();
    });

    trainingStatus.change(function () {
        trainingStatus = $('input[name="trainingStatus"]:checked');
        showTableData();
    });

    currentTraining.change(function () {
        $('#exportTrainingId').prop('value', currentTraining.val())
        showTableData();
    });

    currentDepartment.trigger('change');
    trainingStatus.trigger('change');
    enrollmentStatus.trigger('change');
    currentTraining.trigger('change');

    function showTableData() {
        var selectedDepartmentId = currentDepartment.val();
        var selectedTrainingStatus = trainingStatus.val();
        var selectedEnrollmentStatus = enrollmentStatus.val();
        var selectedTrainingId = currentTraining.val();

        var filteredRows = _.filter(originalData, function (row) {
            var rowTraining = $(row).data('trainingid');
            var rowDepartmentId = $(row).data('departmentid');
            var rowTrainingStatus = $(row).data('trainingstatus');
            var rowEnrollmentStatus = $(row).data('enrollmentstatus');

            return rowDepartmentId == selectedDepartmentId &&
                rowTrainingStatus == selectedTrainingStatus &&
                rowEnrollmentStatus == selectedEnrollmentStatus &&
                rowTraining == selectedTrainingId;
        });

        enrollmentTable.clear().rows.add(filteredRows).draw();
    }
});
