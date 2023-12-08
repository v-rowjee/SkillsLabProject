//Page Loader
//$(window).on('load', function () {
//    $('#loading').css("width", "100%")
//    setTimeout(() => {
//        $('#loading').fadeOut()
//    }, 750);
//})

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

$('#trainingTable').DataTable({
    order: [[1, 'desc']],
    "columnDefs": [
        { "orderable": false, "targets": [3,4] }
    ],
    language: {
    'paginate': {
        'previous': '<span class="fa fa-chevron-left"></span>',
        'next': '<span class="fa fa-chevron-right"></span>'
    },
    "lengthMenu": 'Number of Results: <select class="form-control input-sm mb-3">' +
        '<option value="10">10</option>' +
        '<option value="25">25</option>' +
        '<option value="50">50</option>' +
        '<option value="-1">All</option>' +
        '</select>'
    },
})  