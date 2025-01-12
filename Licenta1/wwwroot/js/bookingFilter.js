$(document).ready(function () {


    $('#filterForm').submit(function (e) {
        e.preventDefault(); // Prevent form submission and page refresh
        applyFilters();

    });

    function updateUI(response) {
        // Parse the response as an HTML document
        const parser = new DOMParser();
        const doc = parser.parseFromString(response, 'text/html');

        $('#bookingList').html(response);

        // Extract pagination controls
        const pagination = doc.querySelector('#pagination').parentNode.innerHTML;
        if (pagination) {
            $('#pagination').parent().html(pagination);
        } else {
            console.error("No pagination controls found in the response.");
            $('#pagination').parent().html('');
        }
    }

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
                bookingStatus: status,
                page: 1
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
                Swal.fire('Error', 'Unable to reset filters. Please try again.', 'error');
            }
        });
    });

    $(document).on('click', '#pagination .page-link', function (e) {
        e.preventDefault();

        const page = $(this).attr('href').split('=')[1]; // Extract the page number
        const statusFilter = $('#statusFilter').val();
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();

        $.ajax({
            url: filterUrl,
            type: 'GET',
            data: {
                statusFilter: statusFilter,
                startDate: startDate,
                endDate: endDate,
                page: page
            },
            success: function (response) {
                updateUI(response);
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch bookings. Please try again.', 'error');
            }
        });
    });

});