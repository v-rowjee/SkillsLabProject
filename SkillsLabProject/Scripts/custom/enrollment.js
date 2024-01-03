$(function () {

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
                else if (response.result == "InvalidType") {
                    $('#enrollmentForm').removeClass("was-validated")
                    $('#enrollmentForm input[name="files"]').addClass("is-invalid")
                    Snackbar.show({
                        text: "Some files are not of the correct type. (jpg, jpeg or png)",
                        actionTextColor: "#CFE2FF"
                    });
                }
                else if (response.result == "FileMissing") {
                    $('#enrollmentForm').removeClass("was-validated")
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
            $(deleteBtn).html('<i class="fa-solid fa-pen-to-square"></i>')
        }
    });

} 