$(function () {

    $('#enrollmentForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#enroll').click(() => {
        if ($('input[type="file"]').length > 0) {
            alert($('input[type="file"]').length)
        } else {
            alert("No Prerequisite")
        }
    })

})