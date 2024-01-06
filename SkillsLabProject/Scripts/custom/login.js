$(() => {
    $("#togglePassword").parent().click(() => {
        $("#togglePassword").toggleClass("fa-eye fa-eye-slash");
        var input = $("#password");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    })

    $('#loginForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#login').click(() => {
        var email = $('#email').val()
        var password = $('#password').val()
        var error = ''

        if (email === '') error += "Email Address required<br><br>"
        if (password === '') error += "Password required<br><br>"

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            showSnackbar(error);
        }
        else {
            var loginViewModelObj = { Email: email, Password: password }

            performAjaxRequest({
                method: "POST",
                url: "/Login/Authenticate",
                data: loginViewModelObj,
            })
        }
    })
})