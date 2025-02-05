$(document).ready(function () {

    function updateUI(response) {
        // Parse the response as an HTML document
        const parser = new DOMParser();
        const doc = parser.parseFromString(response, 'text/html');

        $('#filteredList').html(response);

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
        const activeYN = $('#ActiveYn').val();
        const categoryId = $('#categoryId').val();
        const url = $('#titleFilter').data("url");


        $.ajax({
            url: url,
            type: 'GET',
            data: {
                titleFilter: titleFilter,
                activeYN: activeYN,
                categoryId: categoryId,
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
        $('#ActiveYn').val('');
        $('#categoryId').val('');
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

        const page = $(this).attr('href').split('=')[1]; 
        const titleFilter = $('#titleFilter').val();
        const ActiveYn = $('#ActiveYn').val();
        const categoryId = $('#categoryId').val();
        const url = $('#titleFilter').data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                titleFilter: titleFilter,
                ActiveYn: ActiveYn,
                categoryId: categoryId,
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
