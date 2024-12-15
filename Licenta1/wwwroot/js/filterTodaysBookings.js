$(document).ready(function () {
    // Click event for the filter button
    $('#filterButton').on('click', function () {
        const bookingIdFilter = $('#bookingIdFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const emailFilter = $('#emailFilter').val();
        const url = $('#bookingIdFilter').data('url');  

   
        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: bookingIdFilter,
                phoneFilter: phoneFilter,
                emailFilter: emailFilter
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

        const url = $('#bookingIdFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: '',
                phoneFilter: '',
                emailFilter: ''
            },
            success: function (result) {
                $('#bookingsTableBody').html(result);  // Reset the table
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset bookings. Please try again.', 'error');
            }
        });
    });
});
