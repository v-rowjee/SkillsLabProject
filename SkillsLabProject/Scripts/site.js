//Page Loader
//$(window).on('load', function () {
//    $('#loading').css("width", "100%")
//    setTimeout(() => {
//        $('#loading').fadeOut()
//    }, 750);
//})

// Bootstrap Form Validations
//(() => {
//    'use strict'

//    const forms = document.querySelectorAll('.needs-validation')

//    Array.from(forms).forEach(form => {
//        form.addEventListener('submit', event => {
//            if (!form.checkValidity()) {
//                event.preventDefault()
//                event.stopPropagation()
//            }

//            form.classList.add('was-validated')
//        }, false)
//    })
//})()

// Bootstrap Tooltip
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))