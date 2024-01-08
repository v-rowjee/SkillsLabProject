//Page Loader
$(window).on('load', function () {
    $('#loading').css("width", "100%")
    setTimeout(() => {
        $('#loading').fadeOut()
    }, 750)
})

// Prevent Form Resubmission
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


    // Bootstrap Tooltip
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

})()


// Spinner
$('#overlay-spinner').hide()

// Dark Mode
var currentTheme = localStorage.getItem("theme") || getDefaultTheme()
updateTheme(currentTheme)

$(() => {
    $("#darkModeToggle").click(function () {
        currentTheme = currentTheme === "dark" ? "light" : "dark"
        updateTheme(currentTheme)
    })
})




// FUNCTIONS
function showSnackbar(message) {
    Snackbar.show({
        text: message,
        actionTextColor: "#CFE2FF"
    });
}

function performAjaxRequest(requestParams) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: requestParams.method,
            url: requestParams.url,
            data: requestParams.data,
            dataType: "json",
            success: (response) => {
                if (response.IsSuccess) {
                    showSnackbar(response.Message);
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
            showSnackbar(response.Message);

            if (response.RedirectUrl) {
                setTimeout(() => {
                    window.location.replace(response.RedirectUrl);
                }, 1000);
            }
            return response;
        })
        .catch((error) => {
            showSnackbar(error.Message);
            throw error;
        });
}
    function getDefaultTheme() {
    //return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    return 'light';
}

function updateTheme(theme) {
    localStorage.setItem("theme", theme);

    $("html").attr("data-bs-theme", theme);

    if (theme === "dark") {
        $(".bg-light").toggleClass("bg-dark-subtle bg-light");
        $(".btn-light").toggleClass("btn-dark btn-light");
        $(".text-black").toggleClass("text-light text-black");
    }
    if (theme === "light") {
        $(".bg-dark-subtle").toggleClass("bg-light bg-dark-subtle");
        $(".btn-dark").toggleClass("btn-light btn-dark");
        $(".text-light").toggleClass("text-black text-light");
    }

    var darkModeToggleText = theme === "dark" ? "Light Mode" : "Dark Mode";
    $("#darkModeToggle").text(darkModeToggleText);
}