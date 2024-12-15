$(document).ready(function () {
    // Function to show popup with success or error messages
    function showPopup(success, message) {
        Swal.fire({
            title: success ? 'Success' : 'Error',
            text: message,
            icon: success ? 'success' : 'error',
            confirmButtonText: 'OK'
        });
    }

    // Use event delegation to handle the cancel booking button click
    $(document).on('click', '.cancel-booking', function () {
        var bookingId = $(this).data('booking-id');
        var actionUrl = $(this).data("url");

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, cancel it!',
            cancelButtonText: 'No, keep it'
        }).then((result) => {
            if (result.isConfirmed) {
                // Make the AJAX POST request if the user confirms the cancellation
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { bookingId: bookingId },
                    success: function (response) {
                        if (response.success) {
                            // Show success popup and update the UI
                            showPopup(true, "Booking cancelled successfully!");

                            // Update the status directly on the page
                            $('#bookingStatus').text('Cancelled');

                            // Disable the cancel and check-in buttons
                            $('.cancel-booking').prop('disabled', true).hide();
                            $('.checkin-booking').prop('disabled', true).hide();
                        } else {
                            // Show error message if cancellation fails
                            showPopup(false, response.message);
                        }
                    },
                    error: function () {
                        // Handle any errors that occur during the AJAX request
                        showPopup(false, "An error occurred while trying to cancel the booking.");
                    }
                });
            }
        });
    });

    // Use event delegation to handle the check-in button click
    $(document).on('click', '.checkin-booking', function () {
        var bookingId = $(this).data('booking-id');
        var actionUrl = $(this).data("url");

        Swal.fire({
            title: 'Are you sure?',
            text: "Proceed with the check-in!",
            icon: 'info',
            showCancelButton: true,
            confirmButtonText: 'Yes, check-in!',
            cancelButtonText: 'No, cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // Make the AJAX POST request if the user confirms the check-in
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { bookingId: bookingId },
                    success: function (response) {
                        if (response.success) {
                            // Show success popup and update the UI
                            showPopup(true, "Booking checked-in successfully!");

                            // Update the status directly on the page
                            $('#bookingStatus').text('Checked In');

                            // Disable the cancel and check-in buttons
                            $('.cancel-booking').prop('disabled', true).hide();
                            $('.checkin-booking').prop('disabled', true).hide();
                        } else {
                            // Show error message if check-in fails
                            showPopup(false, response.message);
                        }
                    },
                    error: function () {
                        // Handle any errors that occur during the AJAX request
                        showPopup(false, "An error occurred while trying to check-in the booking.");
                    }
                });
            }
        });
    });
});
