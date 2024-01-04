//Page Loader
$(window).on('load', function () {
    $('#loading').css("width", "100%")
    setTimeout(() => {
        $('#loading').fadeOut()
    }, 750);
})

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
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

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
        currentTheme = toggleTheme(currentTheme);
        localStorage.setItem("theme", currentTheme);
        updateTheme(currentTheme);
    });
});

function getDefaultTheme() {
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
}

function toggleTheme(theme) {
    return theme === "dark" ? "light" : "dark";
}

function updateTheme(theme) {
    $("body").attr("data-bs-theme", theme)
    var classBefore = theme === "dark" ? "bg-light btn-light" : "bg-dark-subtle btn-dark";
    var classAfter = theme === "dark" ? "bg-dark-subtle btn-dark" : "bg-light btn-light";

    $(".bg-light, .btn-light").removeClass(classBefore).addClass(classAfter);
}