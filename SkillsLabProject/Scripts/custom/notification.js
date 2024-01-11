$(() => {
    $('#notificationTable').DataTable({
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: [1,2] },
        ],
        pageLength: 7,
        language: {
            paginate: {
                previous: '<span class="fa fa-chevron-left"></span>',
                next: '<span class="fa fa-chevron-right"></span>'
            },
            lengthMenu: 'Max rows: <select class="form-control input-sm mb-3">' +
                '<option value="7">7</option>' +
                '<option value="15">15</option>' +
                '<option value="25">25</option>' +
                '<option value="-1">All</option>' +
                '</select>',
            emptyTable: '<img src="Content/Images/no-data.png" class="w-25" />'
        },
    });

    $('#notificationTable').on('click', 'tr', function () {
        var isRead = $(this).find('.notificationIsRead').val();
        console.log(isRead);

        if (isRead === "0") {
            var notificationId = $(this).find('.notificationId').val();

            performAjaxRequest({
                method: "POST",
                url: "/Notification/MarkAsRead",
                data: { id: notificationId },
            })
        }
    });

    $('#notificationTable').on('click', '.deleteNotif', function () {
        var notificationId = $(this).closest('tr').find('.notificationId').val();

        performAjaxRequest({
            method: "POST",
            url: "/Notification/Delete",
            data: { id: notificationId },
        })
    });

})