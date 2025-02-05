$(document).on('click', '.delete-btn', function () {
    var userId = $(this).data('id');
    var actionUrl = $(this).data("url");

    Swal.fire({
        title: 'Are you sure?',
        text: "You are about to delete this user account!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: { id: userId },
                success: function (response) {
                    if (response.success) {
                        Swal.fire('Deleted!', 'The user account has been deleted.', 'success');

                        // Remove the row from the table
                        $('tr[data-user-id="' + userId + '"]').remove();
                    } else {
                        Swal.fire('Error', 'Something went wrong.', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Something went wrong.', 'error');
                }
            });
        }
    });
});
