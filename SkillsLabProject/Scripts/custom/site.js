//Page Loader
$(window).on('load', function () {
    $('#loading').css("width", "100%")
    setTimeout(() => {
        $('#loading').fadeOut()
    }, 750);
})

// Snackbar
function showSnackbar(message) {
    Snackbar.show({
        text: message,
        actionTextColor: "#CFE2FF"
    });
}

// Ajax
function performAjaxRequest(requestParams) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: requestParams.method,
            url: requestParams.url,
            data: requestParams.data,
            dataType: "json",
            success: (response) => {
                if (response.IsSuccess) {
                    showSnackbar(response.Message)
                    resolve(response);
                } else {
                    reject(response);
                }
            },
            error: () => {
                reject({ IsSuccess: false, Message: "An error occurred while making the request." });
            }
        });
    })
        .then((response) => {
            showSnackbar(response.Message)

            if (response.RedirectUrl) {
                setTimeout(() => {
                    window.location.replace(response.RedirectUrl);
                }, 1000)
            }
            return response;
        })
        .catch((error) => {
            showSnackbar(error.Message)
            throw error;
        });
}



// prevent form resubmission
if (window.history.replaceState) {
    window.history.replaceState(null, null, window.location.href);
}

// Bootstrap Form Validations
(() => {
    'use strict'

    const forms = document.querySelectorAll('.needs-validation')

    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()

// Bootstrap Tooltip
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

// Spinner
$('#overlay-spinner').hide()

function setLoading(flag) {
    if (flag) {
        $('#enroll').prop('disabled', true);
        $('#overlay-spinner').show()
    } else {
        $('#enroll').prop('disabled', false);
        $('#overlay-spinner').hide()
    }
}

// Dark Mode
var currentTheme = localStorage.getItem("theme") || getDefaultTheme();
updateTheme(currentTheme);

$(document).ready(function () {
    $("#darkModeToggle").click(function () {
        currentTheme = theme === "dark" ? "light" : "dark";
        updateTheme(currentTheme);
    });
});

function getDefaultTheme() {
    //return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    return 'light';
}

function toggleTheme(theme) {
    return theme === "dark" ? "light" : "dark";
}

function updateTheme(theme) {
    localStorage.setItem("theme", theme);

    $("html").attr("data-bs-theme", theme)

    var classBefore = theme === "dark" ? "bg-light btn-light" : "bg-dark-subtle btn-dark";
    var classAfter = theme === "dark" ? "bg-dark-subtle btn-dark" : "bg-light btn-light";
    $(".bg-light, .btn-light").removeClass(classBefore).addClass(classAfter);

    var darkModeToggleText = theme === "dark" ? "Light Mode" : "Dark Mode";
    $("#darkModeToggle").text(darkModeToggleText);
}