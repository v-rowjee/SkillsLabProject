$(() => {
    $('img').on('load', function () {
        $('#viewProofs').html("View")
        $('#viewProofs').attr('disabled', false);
    });
    $('#downloadProofs').click(function () {
        var proofImages = $('.proof-img');
        proofImages.each(function (index) {
            var link = document.createElement('a');
            link.href = $(this).attr('src');
            link.download = 'proof' + (index + 1) + '.jpg';
            link.target = '_blank';
            link.style.display = 'none';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
    });

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

        var approveButton = $('#approveEnrollment');
        var originalButtonText = approveButton.text()
        approveButton.html(`<div class="spinner-border spinner-border-sm" role="status"></div>`)
        $(':button').prop('disabled', true);

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
            },
            complete: () => {
                approveButton.html(originalButtonText)
                $(':button').prop('disabled', false);
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

        var declineButton = $('#declineEnrollment')
        var originalButtonText = declineButton.text()
        declineButton.html(`<div class="spinner-border spinner-border-sm" role="status"></div>`)
        $(':button').prop('disabled', true);

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
            },
            complete: () => {
                declineButton.html(originalButtonText)
                $(':button').prop('disabled', false);
            }
        });

    })

})

// DELETE ENROLLMENT
function deleteEnrollment(deleteBtn) {
    var enrollmentId = $('#enrollmentId').val();

    var deleteBtnOriginalText = $(deleteBtn).text()
    $(deleteBtn).html(`
        <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
    `)

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
        error: (e) => {
            console.log(e)
            Snackbar.show({
                text: "An error occured while removing enrollment.",
                actionTextColor: "#CFE2FF"
            });
        },
        complete: () => {
            $(deleteBtn).html(deleteBtnOriginalText)
        }
    });

} 


// Loading
function setLoading(flag) {
    if (flag) {
        $('#enroll').prop('disabled', true);
        $('#overlay-spinner').show()
    } else {
        $('#enroll').prop('disabled', false);
        $('#overlay-spinner').hide()
    }
}