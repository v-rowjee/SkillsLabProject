$(function () {

    $('#enrollmentForm').submit((e) => {
        $('#enroll').prop('disabled', true);
        $('#enroll').html(`
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Uploading files...</span>
        `)
        $('#overlay-spinner').show()

        e.preventDefault()
        return false;
    })

    $('#enroll').click(() => {
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
                $('#enroll').prop('disabled', false);
                $('#enroll').html('Enroll')
                $('#overlay-spinner').hide()
            }
        });
    })


})