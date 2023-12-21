$(function () {

    // DataTable
    $('#enrollmentTable').DataTable({
        order: [[1, 'desc']],
        "columnDefs": [
            { "orderable": false, "targets": [-1] }
        ],
        language: {
            'paginate': {
                'previous': '<span class="fa fa-chevron-left"></span>',
                'next': '<span class="fa fa-chevron-right"></span>'
            },
            "lengthMenu": 'Number of Results: <select class="form-control input-sm mb-3">' +
                '<option value="10">10</option>' +
                '<option value="25">25</option>' +
                '<option value="50">50</option>' +
                '<option value="-1">All</option>' +
                '</select>'
        },
    })

    $('#enrollmentForm').submit((e) => {
        e.preventDefault()
        return false;
    })

    var inputFiles = $('input[name="files"]')
    inputFiles.change(() => {
        if (inputFiles[0].files > 0) {
            inputFiles.removeClass("is-invalid")
        }
        else {
            inputFiles.removeClass("is-valid")
        }
    })

    $('#enroll').click(() => {

        $('#enroll').html(`
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Enrolling Training...</span>
        `)
        setLoading(true)

        var formData = new FormData();
        var trainingId = $('#trainingId').val();
        var inputFiles = $('input[name="files"]');

        formData.append("trainingId", trainingId);
        for (i = 0; i < inputFiles.length; i++) {
            for (j = 0; j < inputFiles[i].files.length; j++) {
                formData.append("files", $('input[name="files"]')[i].files[j]);
            }
        }

        $.ajax({
            type: "POST",
            url: '/Enrollment/Enroll',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            async: true,
            success: (response) => {
                if (response.result == "Success") {
                    Snackbar.show({
                        text: "Training enrolled successfully!",
                        actionTextColor: "#CFE2FF"
                    });
                    window.location.reload()
                }
                else if (response.result == "FileMissing") {
                    $('#enrollmentForm input[name="files"]').addClass("is-invalid")
                    Snackbar.show({
                        text: "Please upload all pre-requisite files.",
                        actionTextColor: "#CFE2FF"
                    });
                }
                else {
                    Snackbar.show({
                        text: "Unable to enroll in training.",
                        actionTextColor: "#CFE2FF"
                    });
                }
            },
            error: () => {
                Snackbar.show({
                    text: "Unable to enroll in training.",
                    actionTextColor: "#CFE2FF"
                });
            },
            complete: () => {
                $('#enroll').html('Enroll')
                setLoading(false)
            }
        });
    })

    $('#deleteEnrollmentForm').submit((e) => {
        e.preventDefault()
        return false;
    })

    $('#status').change(() => {
        var status = $('#status').val()
        if (status == "Declined") {
            $('#reasonCard').removeClass('d-none')
        }
        else {
            $('#reasonCard').addClass('d-none')
        }
    })

    $('#editEnrollmentForm').submit((e) => {
        e.preventDefault()
        return false
    })

    $('#approveEnrollment').click(() => {
        var id = $('#enrollmentId').val()
        var status = "Approved"

        var enrollmentObj = {
            EnrollmentId: id,
            Status: status,
        }

        $.ajax({
            type: "POST",
            url: '/Enrollment/Approve',
            data: enrollmentObj,
            dataType: 'json',
            success: (response) => {
                if (response.result == "Success") {
                    Snackbar.show({
                        text: "Enrollment approved!",
                        actionTextColor: "#CFE2FF"
                    });
                    window.location.reload()
                }
                else {
                    Snackbar.show({
                        text: "Unable to change enrollment status.",
                        actionTextColor: "#CFE2FF"
                    });
                }
            },
            error: () => {
                Snackbar.show({
                    text: "Unable to change enrollment status.",
                    actionTextColor: "#CFE2FF"
                });
            }
        });

    })

    $('#declineEnrollment').click(() => {
        var id = $('#enrollmentId').val()
        var status = "Declined"
        var reason = $('#reason').val()

        var enrollmentObj = {
            EnrollmentId: id,
            Status: status,
            DeclinedReason: reason
        }

        $.ajax({
            type: "POST",
            url: '/Enrollment/Decline',
            data: enrollmentObj,
            dataType: 'json',
            success: (response) => {
                if (response.result == "Success") {
                    Snackbar.show({
                        text: "Enrollment Declined!",
                        actionTextColor: "#CFE2FF"
                    });
                    window.location.reload()
                }
                else {
                    Snackbar.show({
                        text: "Unable to change enrollment status.",
                        actionTextColor: "#CFE2FF"
                    });
                }
            },
            error: () => {
                Snackbar.show({
                    text: "Unable to change enrollment status.",
                    actionTextColor: "#CFE2FF"
                });
            }
        });

    })

})

// DELETE ENROLLMENT
function deleteEnrollment(deleteBtn) {
    $(deleteBtn).html(`
        <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
    `)

    var enrollmentId = $(deleteBtn).siblings('input[type="hidden"]').val();

    $.ajax({
        type: "POST",
        url: '/Enrollment/Delete',
        data: { id: enrollmentId },
        dataType: 'json',
        success: (response) => {
            if (response.result) {
                Snackbar.show({
                    text: "Enrollemnt has been removed!",
                    actionTextColor: "#CFE2FF"
                });
                window.location.replace(response.url);
            }
            else {
                Snackbar.show({
                    text: "Unable to remove enrollment of this training.",
                    actionTextColor: "#CFE2FF"
                });
            }
        },
        error: () => {
            Snackbar.show({
                text: "Unable to remove enrollment of this training.",
                actionTextColor: "#CFE2FF"
            });
        },
        complete: () => {
            $(deleteBtn).html('Cancel')
        }
    });

}

// CLOSE TRAINING
function closeTraining(closeBtn) {
    $(closeBtn).html(`
        <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
        <span role="status">Closing Training...</span>
    `)

    var trainingId = $(closeBtn).siblings('input[type="hidden"]').val();

    $.ajax({
        type: "POST",
        url: '/Training/Close',
        data: { id: trainingId },
        dataType: 'json',
        success: (response) => {
            if (response.result) {
                Snackbar.show({
                    text: "Training has been closed!",
                    actionTextColor: "#CFE2FF"
                });
                window.location.replace(response.url);
            }
            else {
                Snackbar.show({
                    text: "Unable to close training.",
                    actionTextColor: "#CFE2FF"
                });
            }
        },
        error: () => {
            Snackbar.show({
                text: "Unable to close training.",
                actionTextColor: "#CFE2FF"
            });
        },
        complete: () => {
            $(closeBtn).html('Close')
        }
    });
}