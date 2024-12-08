$(document).ready(function () {
    // Click event for the filter button
    $('#filterButton').on('click', function () {
        const emailFilter = $('#emailFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const roleFilter = $('#roleFilter').val();
        const url = $('#emailFilter').data('url'); // URL from the data attribute

        console.log(roleFilter);
        // Send AJAX request with the filter parameters
        $.ajax({
            url: url,
            type: 'GET',
            data: {
                emailFilter: emailFilter,
                phoneFilter: phoneFilter,
                roleFilter: roleFilter
            },
            success: function (result) {
                $('#accountsTable').html(result); // Update the table with the result
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch accounts. Please try again.', 'error');
            }
        });
    });

    // Click event for the reset button
    $('#resetButton').on('click', function () {
        // Clear filter inputs
        $('#emailFilter').val('');
        $('#phoneFilter').val('');
        $('#roleFilter').val('');

        // Send AJAX request to reset filters
        const url = $('#emailFilter').data('url'); // URL from the data attribute

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                emailFilter: '',
                phoneFilter: '',
                roleFilter: ''
            },
            success: function (result) {
                $('#accountsTable').html(result); // Reset the table with the full list
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset accounts. Please try again.', 'error');
            }
        });
    });
});
