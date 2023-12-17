﻿$(function () {
    $('#roleEmployee').click(() => $('#userrole').text('Employee'))
    $('#roleManager').click(() => $('#userrole').text('Manager'))
    $('#roleAdmin').click(() => $('#userrole').text('Admin'))

    $("#togglePassword").parent().click(() => {
        $("#togglePassword").toggleClass("fa-eye fa-eye-slash");
        var input = $("#password");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    })

    $('#registerForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#phone').on('keypress', function (e) {
        // allow numbers, plus sign and space
        if ((e.charCode < 48 || e.charCode > 57) && e.charCode !== 43 && e.charCode !== 32) return false;
    });

    $('#register').click(() => {
        var role = $('.btn-check:checked').val()
        var email = $('#email').val().trim()
        var password = $('#password').val().trim()
        var fname = $('#fname').val().trim()
        var lname = $('#lname').val().trim()
        var nic = $('#nic').val().trim()
        var phone = $('#phone').val().trim()
        var department = $('#department').val()

        var error = ''

        if (!email) error += "Email Address required<br><br>"
        if (!password) error += "Password required<br><br>"
        if (!fname) error += "First Name required<br><br>"
        if (!lname) error += "Last Name required<br><br>"
        if (!nic) error += "NIC required<br><br>"
        if (!phone) error += "Phone number required<br><br>"
        if (!department) error += "Department required<br><br>"


        if (password && password.length < 4) {
            error += "Password should be at least 4 characters."
            $("#password-feedback").html("Password should be at least four characters.");
        }

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {
            var RegisterViewModelObj = {
                Role: role,
                Email: email,
                Password: password,
                FirstName: fname,
                LastName: lname,
                NIC: nic,
                PhoneNumber: phone,
                DepartmentId: department
            }

            $.ajax({
                type: "POST",
                url: "/Register/Register",
                data: RegisterViewModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result == "Success") {
                        Snackbar.show({
                            text: "Registration successful!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else if (response.result == "DuplicatedEmail") {

                        $("#email").addClass("is-invalid")
                        $("#email-feedback").html("Email already exists.");

                        Snackbar.show({
                            text: "An account with this email already exist.",
                            actionText: "LOGIN",
                            actionTextColor: "#CFE2FF",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to register",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }




    })

})