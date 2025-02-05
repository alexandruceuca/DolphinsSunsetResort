$(document).ready(function () {
    $('.btn-mark-ready').on('click', function () {
        var button = $(this);
        var roomId = button.data('room-id');
        var newStatus = button.data('status');
        var url = button.data('url');

   
        Swal.fire({
            title: 'Are you sure?',
            text: "You are about to change the status",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // Send AJAX request to update room status
                $.ajax({
                    type: "POST",
                    url: url,
                    data: { roomId: roomId, status: newStatus },
                    success: function (response) {
                     
                        showPopup(true, 'Room status updated successfully.');
                        location.reload();  
                    },
                    error: function (xhr, status, error) {
                        // Show error popup
                        showPopup(false, 'An error occurred while updating the status. Please try again.');
                    }
                });
            }
        });
    });
});