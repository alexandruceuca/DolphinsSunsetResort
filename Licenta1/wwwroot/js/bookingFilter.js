$(document).ready(function () {


    $('#filterForm').submit(function (e) {
        e.preventDefault(); // Prevent form submission and page refresh
        applyFilters();

    });

    function applyFilters() {

        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();
        var status = $('#statusFilter').val();

 
        // Validate that startDate and endDate are different
        if (startDate && endDate && startDate === endDate) {
            showPopup(false, "Start Date and End Date must be different.");
            return; // Stop execution if validation fails
        }

        // Validate that startDate and endDate cant be after endDate
        if (endDate != null && endDate != '' && startDate > endDate) {
            showPopup(false, "Start Date can't be after End Date .");
            return; // Stop execution if validation fails
        }


        $.ajax({
            url: filterUrl,
            type: 'GET',
            data: {
                checkInDate: startDate,
                checkOutDate: endDate,
                bookingStatus: status
            },
            success: function (response) {

                $('#bookingList').html(response);
            },
            error: function () {
                Swal.fire('Error', 'An error occurred while applying filters.', 'error');
            }
        });
    }

    // Click event for the reset button
    $('#resetFilters').on('click', function () {
        // Clear filter inputs
        $('#startDate').val('');
        $('#endDate').val('');
        $('#statusFilter').val('');

        $.ajax({
            url: filterUrl,
            type: 'GET',
            data: {
                startDate: '',
                endDate: '',
                statusFilter: ''
            },
            success: function (response) {
                $('#bookingList').html(response); 
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset accounts. Please try again.', 'error');
            }
        });
    });

});