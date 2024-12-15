$(document).ready(function () {
    // Click event for the filter button
    $('#filterButton').on('click', function () {
        const bookingIdFilter = $('#bookingIdFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const emailFilter = $('#emailFilter').val();
        const url = '@Url.Action("GetBookings", "Booking")'; // Get the URL for the GetBookings action

        // Send AJAX request with the filter parameters
        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: bookingIdFilter,
                phoneFilter: phoneFilter,
                emailFilter: emailFilter
            },
            success: function (result) {
                $('#bookingsTableBody').html($(result).find('#bookingsTableBody').html()); // Update the table body
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

        // Send AJAX request to reset filters
        const url = '@Url.Action("GetBookings", "Booking")'; // Get the URL for the GetBookings action

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: '',
                phoneFilter: '',
                emailFilter: ''
            },
            success: function (result) {
                $('#bookingsTableBody').html($(result).find('#bookingsTableBody').html()); // Reset the table
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset bookings. Please try again.', 'error');
            }
        });
    });
});
