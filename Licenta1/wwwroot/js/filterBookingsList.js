$(document).ready(function () {
    // Click event for the filter button
    $('#filterButton').on('click', function () {
        const bookingIdFilter = $('#bookingIdFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const emailFilter = $('#emailFilter').val();
        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();
        const url = $('#bookingIdFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: bookingIdFilter,
                phoneFilter: phoneFilter,
                emailFilter: emailFilter,
                checkInDate: startDate,
                checkOutDate: endDate,
                page: 1  // Reset to the first page when applying filters
            },
            success: function (result) {
                $('#bookingsTableBody').html(result);
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch bookings. Please try again.', 'error');
            }
        });
    });

    // Click event for the reset button
    $('#resetButton').on('click', function () {
        // Clear filter inputs
        $('#bookingIdFilter').val('');
        $('#phoneFilter').val('');
        $('#emailFilter').val('');
        $('#startDate').val('');
        $('#endDate').val('');

        const url = $('#bookingIdFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: '',
                phoneFilter: '',
                emailFilter: '',
                checkInDate: '',
                checkOutDate: '',
                page: 1  // Reset to the first page when clearing filters
            },
            success: function (result) {
                $('#bookingsTableBody').html(result);  // Reset the table
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset bookings. Please try again.', 'error');
            }
        });
    });

    // Pagination - Handling page clicks
    $('.pagination').on('click', '.page-link', function (e) {
        e.preventDefault();

        var page = $(this).attr('href').split('=')[1];  // Extract the page number from the href attribute
        const bookingIdFilter = $('#bookingIdFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const emailFilter = $('#emailFilter').val();
        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();
        const url = $('#bookingIdFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: bookingIdFilter,
                phoneFilter: phoneFilter,
                emailFilter: emailFilter,
                checkInDate: startDate,
                checkOutDate: endDate,
                page: page  
            },
            success: function (result) {
                $('#bookingsTableBody').html(result);  // Update the bookings table
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch bookings. Please try again.', 'error');
            }
        });
    });
});
