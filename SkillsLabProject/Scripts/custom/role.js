﻿$(function () {

    $('#roleForm').submit((e) => {
        e.preventDefault()
        return false
    })

    $('#roleBtn').click(() => {
        var role = $('input[name="role"]').val()

        $.ajax({
            type: "POST",
            url: "/Home/Role",
            data: {role : role},
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    Snackbar.show({
                        text: "Logged in as " + role,
                        actionTextColor: "#CFE2FF"
                    });
                    window.location.replace(response.url);
                }
                else {
                    Snackbar.show({
                        text: "Unable to select role.",
                        actionTextColor: "#CFE2FF"
                    });
                }
            }
        })
    })

})