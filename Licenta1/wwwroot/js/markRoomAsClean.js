$(document).ready(function () {

    function showPopup(success, message) {
        Swal.fire({
            title: success ? 'Success' : 'Error',
            text: message,
            icon: success ? 'success' : 'error',
            confirmButtonText: 'OK'
        });
    }

    // Function to mark room as ready for check-in
    function markAsReadyForCheckIn(button) {
        var roomId = $(button).data('room-id');
        var actionUrl = $(button).data("url");

        Swal.fire({
            title: 'Are you sure?',
            text: "You are about to mark this room as ready for check-in!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, mark it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { roomId: roomId },
                    success: function (response) {
                        if (response.success) {
                            showPopup(true, 'Room status updated!');
                            // Remove the room row from the DOM
                            $('div[data-room-id="' + roomId + '"]').fadeOut(500, function () {
                                $(this).remove();  // Remove after the fade-out effect
                            });
                        } else {
                            showPopup(false, 'Failed to update room status');
                        }
                    },
                    error: function () {
                        showPopup(false, 'An error occurred while updating the room status.');
                    }
                });
            }
        });
    }

    // Bind the markAsReadyForCheckIn function to the buttons dynamically
    $(document).on('click', '.btn-mark-ready', function () {
        markAsReadyForCheckIn(this);
    });
});
