$(document).ready(function () {
    var options = {
        lengthChange: false,
        pageLength: 50,
        select: {
            style: 'os'
        },
        stateSave: true,
        order: [[0, "desc"]]
    };

    var table = $("#dynamicTable").DataTable(options);

    $('#dynamicTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#dynamicTable tbody').on('click', 'tr', function () {
        var customerId = $("#dynamicTable").find('.selected').children('td:first').text();
        var page = $('#dynamicTable').attr('custom-submitpage');

        if (page != null && page != "") {
            window.location.href = page + "/" + customerId;
        }
    });
});
