
$(function () {
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
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {
            var LoginViewModelObj = { Email: email, Password: password }

            $.ajax({
                type: "POST",
                url: "/Login/Authenticate",
                data: LoginViewModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result) {
                        Snackbar.show({
                            text: "Authentication successful!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to authenticate",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }

    })

})