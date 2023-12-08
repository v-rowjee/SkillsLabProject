$(function () {
    if ($('#roleEmployee').click(() => $('#userrole').text('Employee') ))
    if ($('#roleManager').click(() => $('#userrole').text('Manager') ))
    if ($('#roleAdmin').click(() => $('#userrole').text('Admin') ))

    $('#registerForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#register').click(() => {
        var role = $('.btn-check:checked').val()
        var email = $('#email').val().trim()
        var password = $('#password').val().trim()
        var fname = $('#fname').val().trim()
        var lname = $('#lname').val().trim()
        var nic = $('#nic').val().trim()
        var phone = $('#phone').val().toString()
        var department = $('#department').val()

        var error = ''

        if (!email) error += "Email Address required<br><br>"
        if (!password) error += "Password required<br><br>"
        if (!fname) error += "First Name required<br><br>"
        if (!lname) error += "Last Name required<br><br>"
        if (!nic) error += "NIC required<br><br>"
        if (!phone) error += "Phone number required<br><br>"
        if (!department) error += "Department required<br><br>"



        $("#email-feedback").html("Please provide a valid email.");

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