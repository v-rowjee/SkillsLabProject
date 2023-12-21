$(function () {

    // DataTable
    $('#trainingTable').DataTable({
        order: [[1, 'desc']],
        "columnDefs": [
            { "orderable": false, "targets": [] }
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

    var countPrerequisites = 0

    function togglePrerequisiteNone() {
        if (countPrerequisites > 0) {
            $('#prerequisiteNone').addClass("d-none");
        } else {
            $('#prerequisiteNone').removeClass("d-none");
        }
    }
    
    $('#addPrerequisite').click(() => {
        if (countPrerequisites < 5) {
            countPrerequisites++
            $('#prerequisiteInputList').append(`
                <li class="list-group-item d-inline-flex rounded">
                    <input list="prerequisiteList" class="form-control my-2 me-3 prerequisite-input" placeholder="Detail" />
                    <button id="deletePrerequisite" class="btn btn-sm btn-outline-danger my-2"><i class="fa fa-trash my-2"></i></button>
                </li>
            `)
        }
        else {
            Snackbar.show({
                text: "List number of pre-requisite reached.",
                actionTextColor: "#CFE2FF"
            });
        }
        togglePrerequisiteNone()
    })

    $('#prerequisiteInputList').on("click", "li #deletePrerequisite", e => {
        $(e.target).closest('li').remove()
        countPrerequisites--
        togglePrerequisiteNone()
    });

    // ADD TRAINING

    $('#createTrainingForm').submit((e) => {
        e.preventDefault();
        return false;
    })
    $('#createTraining').click((e) => {
        e.preventDefault()
        var title = $('#title').val()
        var description = $('#description').val()
        var deadline = $('#deadline').val()
        var capacity = $('#capacity').val()
        var department = $('#department').val()

        var error = ''

        if (!title) error += "Title required<br><br>"
        if (!description) error += "Description required<br><br>"
        if (!deadline) error += "Deadline required<br><br>"
        if (!capacity) error += "Capacity required<br><br>"
        if (!department) error += "Department required<br><br>"


        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {
            var prerequisites = [];
            $('.prerequisite-input').each(function () {
                if ($(this).val() != "") {
                    prerequisites.push($(this).val());
                }
            });

            var TrainingViewModelObj = {
                Title: title,
                Description: description,
                Deadline: deadline,
                Capacity: capacity,
                DepartmentId: department,
                PreRequisites: prerequisites
            }

            $.ajax({
                type: "POST",
                url: "/Training/Create",
                data: TrainingViewModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result) {
                        Snackbar.show({
                            text: "Training added successfully!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to add training.",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }
    })

    // EDIT TRAINING
    $('#editTrainingForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#editTraining').click(() => {
        var trainingId = $('#trainingId').val()
        var title = $('#title').val()
        var description = $('#description').val()
        var deadline = $('#deadline').val()
        var capacity = $('#capacity').val()
        var department = $('#department').val()

        var error = ''

        if (!title) error += "Title required<br><br>"
        if (!description) error += "Description required<br><br>"
        if (!deadline) error += "Deadline required<br><br>"
        if (!capacity) error += "Capacity required<br><br>"
        if (!department) error += "Department required<br><br>"

        if (error != '') {
            error = error.slice(0, -8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF"
            });
        }
        else {
            var TrainingModelObj = {
                TrainingId: trainingId,
                Title: title,
                Description: description,
                Deadline: deadline,
                Capacity: capacity,
            }
            if (department != null) {
                TrainingModelObj.DepartmentId = department
            }
            

            var prerequisites = [];
            $('.prerequisite-input').each(function () {
                if ($(this).val() != "") {
                    prerequisites.push($(this).val());
                }
            });

            if (prerequisites != []) {
                TrainingModelObj.PreRequisites = prerequisites
            }

            $.ajax({
                type: "POST",
                url: "/Training/Edit",
                data: TrainingModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result) {
                        Snackbar.show({
                            text: "Training details updated successfully!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else {
                        Snackbar.show({
                            text: "Unable to edit training details",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }

    })

    $('#trainingForm').submit((e) => {
        e.preventDefault()
        return false;
    })
})

// DELETE TRAINING
function deleteTraining(deleteBtn) {
    $(deleteBtn).html(`
        <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
    `)

    var trainingId = $(deleteBtn).siblings('input[type="hidden"]').val();

    $.ajax({
        type: "POST",
        url: "/Training/Delete",
        data: { id: trainingId },
        dataType: "json",
        success: (response) => {
            if (response.result == "Success") {
                Snackbar.show({
                    text: "Training removed!",
                    actionTextColor: "#CFE2FF"
                });
                window.location.replace(response.url);
            }
            else {
                Snackbar.show({
                    text: "Unable to remove training.",
                    actionTextColor: "#CFE2FF"
                });
            }
        },
        complete: () => {
            $(deleteBtn).html(`<i class="fa-solid fa-trash"></i>`)
        }
    })
}