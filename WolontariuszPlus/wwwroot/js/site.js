
$(document).ready(function () {
    $('#organizer-table').DataTable({
        "paging": false,
        "info": false,
        "order": [[1, 'asc']],
        "columnDefs": [
            { "orderable": false, "targets": 4 }
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

    $('#volunteer-table-archived').DataTable({
        "paging": false,
        "info": false,
        "order": [[2, 'asc']],
        "columnDefs": [
            { "orderable": false, "targets": 8 }
        ],
        "language": {
            "search": "Szukaj",
            "zeroRecords": "Brak danych do wyświetlenia"
        }
    });
});