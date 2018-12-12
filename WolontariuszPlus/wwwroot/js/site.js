// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#organizer-table').DataTable({
        "paging": false,
        "info": false,
        "order": [[1, 'asc']],
        "columnDefs": [
            { "orderable": false, "targets": 5 }
        ],
        "language": {
            "search": "Szukaj",
            "zeroRecords": "Brak danych do wyświetlenia"
        }
    });

    $('#volunteer-table').DataTable({
        "paging": false,
        "info": false,
        "order": [[2, 'asc']],
        "columnDefs": [
            { "orderable": false, "targets": 7 }
        ],
        "language": {
            "search": "Szukaj",
            "zeroRecords": "Brak danych do wyświetlenia"
        }
    });
});