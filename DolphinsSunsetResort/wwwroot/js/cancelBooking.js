﻿$(document).ready(function () {
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

                            // Find the booking row using the bookingId
                            var bookingRow = $('[data-booking-id="' + bookingId + '"]').closest('.list-group-item');

                            // Update the status text directly
                            bookingRow.find('.status-text').text('Status: Cancelled');
                            // Disable the cancel button
                            $('[data-booking-id="' + bookingId + '"]').prop('disabled', true).hide();
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
});
