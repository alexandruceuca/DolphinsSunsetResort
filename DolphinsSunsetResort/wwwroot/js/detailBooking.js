$(document).ready(function () {
    // Handle the Check-In button
    $(document).on('click', '.checkin-booking', function () {
        var bookingId = $(this).data('booking-id');
        var actionUrl = $(this).data('url');

        Swal.fire({
            title: 'Are you sure?',
            text: "Proceed with the check-in!",
            icon: 'info',
            showCancelButton: true,
            confirmButtonText: 'Yes, check-in!',
            cancelButtonText: 'No, cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // Make the AJAX POST request
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { bookingId: bookingId },
                    success: function (response) {
                        if (response.success) {
                            // Update the booking status
                            $('#bookingStatus').text('Checked In');

                            // Disable the Check-In button
                            $('.checkin-booking').prop('disabled', true).hide();
                            $('.cancel-booking').prop('disabled', true).hide();
                            $('.checkout-booking').show();
                            // Show success popup
                            showPopup(true, "Booking checked-in successfully!");

                            // Update all room statuses to 'Occupied'
                            $('.room-status').each(function () {
                                $(this).text('Occupied');
                            });
                        } else {
                            showPopup(false, response.message);
                        }
                    },
                    error: function () {
                        showPopup(false, "An error occurred while trying to check-in the booking.");
                    }
                });
            }
        });
    });

    // Handle the Cancel Booking button
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
                // Make the AJAX POST request
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { bookingId: bookingId },
                    success: function (response) {
                        if (response.success) {
                            // Update the booking status
                            $('#bookingStatus').text('Cancelled');

                            // Disable all action buttons
                            $('.cancel-booking').prop('disabled', true).hide();
                            $('.checkin-booking').prop('disabled', true).hide();
                            $('.checkout-booking').prop('disabled', true).hide();

                            // Show success popup
                            showPopup(true, "Booking cancelled successfully!");
                        } else {
                            showPopup(false, response.message);
                        }
                    },
                    error: function () {
                        showPopup(false, "An error occurred while trying to cancel the booking.");
                    }
                });
            }
        });
    });

    // Handle the Check-Out button
    $(document).on('click', '.checkout-booking', function () {
        var bookingId = $(this).data('booking-id');
        var actionUrl = $(this).data('url');

        Swal.fire({
            title: 'Are you sure?',
            text: "Proceed with the check-out!",
            icon: 'info',
            showCancelButton: true,
            confirmButtonText: 'Yes, check-out!',
            cancelButtonText: 'No, cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // Make the AJAX POST request
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { bookingId: bookingId },
                    success: function (response) {
                        if (response.success) {
                            // Update the booking status
                            $('#bookingStatus').text('Checked Out');

                            // Disable the Check-Out button
                            $('.checkout-booking').prop('disabled', true).hide();

                            // Show success popup
                            showPopup(true, "Booking checked-out successfully!");

                            // Update all room statuses to 'Available'
                            $('.room-status').each(function () {
                                $(this).text('NeedsCleaning');
                            });
                        } else {
                            showPopup(false, response.message);
                        }
                    },
                    error: function () {
                        showPopup(false, "An error occurred while trying to check-out the booking.");
                    }
                });
            }
        });
    });
});
