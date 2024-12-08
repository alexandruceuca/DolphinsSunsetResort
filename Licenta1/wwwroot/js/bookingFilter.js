$(document).ready(function () {

    $('#resetFilters').click(function () {
        $('#startDate').val('');
        $('#endDate').val('');
        $('#statusFilter').val('');
        applyFilters(); // Apply filters after reset
    });


    $('#filterForm').submit(function (e) {
        e.preventDefault(); // Prevent form submission and page refresh
        applyFilters();
    });

    function applyFilters() {

        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();
        var status = $('#statusFilter').val();

        console.log(status);

        $.ajax({
            url: filterUrl,
            type: 'GET',
            data: {
                checkInDate: startDate,
                checkOutDate: endDate,
                bookingStatus: status
            },
            success: function (response) {

                $('#bookingList').html(response);
            },
            error: function () {
                Swal.fire('Error', 'An error occurred while applying filters.', 'error');
            }
        });
    }
});