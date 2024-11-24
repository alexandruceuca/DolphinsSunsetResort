$(document).ready(function () {
    // Handle the filter form submission using AJAX
    $("#filterForm").submit(function (e) {
        e.preventDefault(); // Prevent form from submitting the traditional way

        // Get the form data
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();

        // Validate that startDate and endDate are different
        if (startDate === endDate) {
            alert("Start Date and End Date must be different.");
            return; // Stop execution if validation fails
        }

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

    $(document).ready(function () {
        $(".add-to-cart-button").click(function (e) {
            e.preventDefault();  // Prevent the form from submitting the traditional way

            var roomId = $(this).data("room-id");
            var checkInDate = $(this).data("checkin-date");
            var checkOutDate = $(this).data("checkout-date");
            var actionUrl = $(this).data("url"); // Get the URL for the action

            // Optional: Add validation for dates if necessary
            if (!checkInDate || !checkOutDate) {
                showPopup(false, "Please select both check-in and check-out dates.");
                return;
            }

            $.ajax({
                url: actionUrl,  // Use the URL for the action method
                type: 'POST',
                data: {
                    roomId: roomId,
                    checkInDate: checkInDate,
                    checkOutDate: checkOutDate
                },
                success: function (response) {
                    showPopup(response.success, response.message);
                },
                error: function (jqXHR) {
                    let message = jqXHR.responseJSON ? jqXHR.responseJSON.message : "An error occurred while adding the room to the cart.";
                    showPopup(false, message);
                }
            });
        });

        function showPopup(success, message) {
            Swal.fire({
                title: success ? 'Success' : 'Error',
                text: message,
                icon: success ? 'success' : 'error',
                confirmButtonText: 'OK'
            });
        }
    });


});
