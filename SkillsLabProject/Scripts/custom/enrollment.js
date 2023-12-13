$(function () {

    $('#enrollmentForm').submit((e) => {
        e.preventDefault()
        return false;
    })

    var inputFiles = $('input[name="files"')
    inputFiles.change(() => {
        if (inputFiles[0].files > 0) {
            inputFiles.removeClass("is-invalid")
        }
    })

    $('#enroll').click(() => {

        $('#enroll').html(`
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Uploading files...</span>
        `)
        setLoading(true)

        var formData = new FormData();
        var trainingId = $('#trainingId').val();
        var inputFiles = $('input[name="files"]')[0].files;

        formData.append("trainingId", trainingId);
        for (i = 0; i < inputFiles.length; i++) {
            formData.append("files", $('input[name="files"]')[0].files[i]);
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
                    window.location.replace(response.url);
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

})

// DELETE ENROLLMENT
function deleteEnrollment(deleteBtn) {
    $(deleteBtn).html(`
        <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
        <span role="status">Removing Enrollment...</span>
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
            $(deleteBtn).html('Cancel Enrollment')
            setLoading(false)
        }
    });

}