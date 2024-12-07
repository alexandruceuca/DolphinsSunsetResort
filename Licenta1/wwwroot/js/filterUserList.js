
$(document).ready(function () {
    $('#emailFilter').on('keypress', function (e) {
        // Check if the Enter key is pressed
        if (e.key === "Enter") {
            const emailFilter = $(this).val();
            const url = $('#emailFilter').data('url');

            $.ajax({
                url: url,
                type: 'GET',
                data: { emailFilter: emailFilter },
                success: function (result) {
                    $('#accountsTable').html(result);
                },
                error: function () {
                    Swal.fire('Error', 'Unable to fetch accounts. Please try again.', 'error');
                }
            });
        }
    });
});
