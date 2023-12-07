$(function () {

    // ADD TRAINING
    var countPrerequisites = 1
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
    })

    $('#prerequisiteInputList').on("click", "li #deletePrerequisite", e => {
        $(e.target).closest('li').remove()
        countPrerequisites--
    });

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
            $('.prerequisite-input').each( function () {
                prerequisites.push($(this).val());
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

    // DELETE TRAINING
    $('#deleteTrainingForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('.deleteTraining').click(() => {
        var trainingId = $('.trainingId').val()

        $.ajax({
            type: "POST",
            url: "/Training/Delete",
            data: trainingId,
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
            }
        })
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

            if (department == null) {
                var TrainingModelObj = {
                    TrainingId: trainingId,
                    Title: title,
                    Description: description,
                    Deadline: deadline,
                    Capacity: capacity,
                }
            }
            else {
                var TrainingModelObj = {
                    TrainingId: trainingId,
                    Title: title,
                    Description: description,
                    Deadline: deadline,
                    Capacity: capacity,
                    DepartmentId: department
                }
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
})
