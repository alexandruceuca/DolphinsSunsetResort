$(document).ready(function () {

    function updateUI(response) {
        // Parse the response as an HTML document
        const parser = new DOMParser();
        const doc = parser.parseFromString(response, 'text/html');

        $('#bookingsTableBody').html(response);

        // Extract pagination controls
        const pagination = doc.querySelector('#pagination').parentNode.innerHTML;
        if (pagination) {
            $('#pagination').parent().html(pagination);
        } else {
            console.error("No pagination controls found in the response.");
            $('#pagination').parent().html('');
        }
    }

    // Filter button click event
    $('#filterButton').on('click', function () {
        const bookingIdFilter = $('#bookingIdFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const emailFilter = $('#emailFilter').val();
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        const url = $('#bookingIdFilter').data("url");
        console.log(bookingIdFilter);

        // Validate that startDate and endDate are different
        if (startDate && endDate && startDate === endDate) {
            showPopup(false, "Start Date and End Date must be different.");
            return; // Stop execution if validation fails
        }

        // Validate that startDate and endDate cant be after endDate
        if (startDate > endDate) {
            showPopup(false, "Start Date can't be after End Date .");
            return; // Stop execution if validation fails
        }
        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: bookingIdFilter,
                phoneFilter: phoneFilter,
                emailFilter: emailFilter,
                checkInDate: startDate,
                checkOutDate: endDate,
                page: 1 // Reset to the first page when applying filters
            },
            success: function (response) {
                updateUI(response);
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch bookings. Please try again.', 'error');
            }
        });
    });

    // Reset button click event
    $('#resetButton').on('click', function () {
        $('#bookingIdFilter').val('');
        $('#phoneFilter').val('');
        $('#emailFilter').val('');
        $('#startDate').val('');
        $('#endDate').val('');
        const url = $('#bookingIdFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: { page: 1 },
            success: function (response) {
                updateUI(response);
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset bookings. Please try again.', 'error');
            }
        });
    });


    $(document).on('click', '#pagination .page-link', function (e) {
        e.preventDefault();

        const page = $(this).attr('href').split('=')[1]; // Extract the page number
        const bookingIdFilter = $('#bookingIdFilter').val();
        const phoneFilter = $('#phoneFilter').val();
        const emailFilter = $('#emailFilter').val();
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        const url = $('#bookingIdFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                bookingIdFilter: bookingIdFilter,
                phoneFilter: phoneFilter,
                emailFilter: emailFilter,
                checkInDate: startDate,
                checkOutDate: endDate,
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
