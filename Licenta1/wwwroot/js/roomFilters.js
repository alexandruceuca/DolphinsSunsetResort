$(document).ready(function () {
    // Handle the filter form submission using AJAX
    $("#filterForm").submit(function (e) {
        e.preventDefault(); // Prevent form from submitting the traditional way

        // Get the form data
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();

        console.log("Start Date:", startDate);
        console.log("End Date:", endDate);

        $.ajax({
            url: filterUrl,
            type: 'GET',
            data: {
                startDate: startDate,
                endDate: endDate
            },
            success: function (response) {
                // Update the room list with the filtered results
                $("#roomListContainer").html(response);
            },
            error: function () {
                alert("Error while filtering rooms.");
            }
        });
    });

    // Reset filters when "Reset Filters" is clicked
    $("#resetFilters").click(function () {
        $("#startDate").val('');
        $("#endDate").val('');

        // Fetch the full list of rooms again (no filters)
        $.ajax({
            url: filterUrl,
            type: 'GET',
            success: function (response) {
                // Reset the room list container to the full list
                $("#roomListContainer").html(response);
            },
            error: function () {
                alert("Error while resetting filters.");
            }
        });
    });
});
