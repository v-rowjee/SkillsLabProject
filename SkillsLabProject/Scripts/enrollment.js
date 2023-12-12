$(function () {

    $('#enrollmentForm').submit((e) => {
        e.preventDefault()
        return false;
    })

    $('#enroll').click(() => {

        $('#enroll').html(`
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Uploading files...</span>
        `)
        setLoading(true)

        var formData = new FormData()
        var trainingId = $('#trainingId').val()
        var files = $('input[name="file"]');

        formData.append("trainingId", trainingId);
        formData.append("httpPostedFileBase", files.each(function (i, file) {
            formData.append("httpPostedFileBase", file);
        }));

        $.ajax({
            type: "POST",
            url: '/Enrollment/Enroll',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            async: true,
            success: (response) => {
                if (response.result) {
                    Snackbar.show({
                        text: "Training enrolled successfully!",
                        actionTextColor: "#CFE2FF"
                    });
                    window.location.replace(response.url);
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