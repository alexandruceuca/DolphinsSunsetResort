$(document).ready(function () {

    function updateUI(response) {
        // Parse the response as an HTML document
        const parser = new DOMParser();
        const doc = parser.parseFromString(response, 'text/html');

        $('#newsTableBody').html(response);

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
        const titleFilter = $('#titleFilter').val();
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        const url = $('#titleFilter').data("url");


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
            url: url,
            type: 'GET',
            data: {
                titleFilter: titleFilter,
                startDate: startDate,
                endDate: endDate,
                page: 1 // Reset to the first page when applying filters
            },
            success: function (response) {
                updateUI(response);
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch news. Please try again.', 'error');
            }
        });
    });

    // Reset button click event
    $('#resetButton').on('click', function () {
        $('#titleFilter').val('');
        $('#startDate').val('');
        $('#endDate').val('');
        const url = $('#titleFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: { page: 1 },
            success: function (response) {
                updateUI(response);
            },
            error: function () {
                Swal.fire('Error', 'Unable to reset news. Please try again.', 'error');
            }
        });
    });


    $(document).on('click', '#pagination .page-link', function (e) {
        e.preventDefault();

        const page = $(this).attr('href').split('=')[1]; // Extract the page number
        const titleFilter = $('#titleFilter').val();
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        const url = $('#titleFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                titleFilter: titleFilter,
                startDate: startDate,
                endDate: endDate,
                page: page
            },
            success: function (response) {
                updateUI(response);
            },
            error: function () {
                Swal.fire('Error', 'Unable to fetch news. Please try again.', 'error');
            }
        });
    });

});
