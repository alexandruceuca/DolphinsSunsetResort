$(document).ready(function () {
    const now = new Date();



        // Function to fetch and update the room list
        function fetchFilteredRooms() {
            // Get the selected dates
            var startDate = $("#startDate").val();
            var endDate = $("#endDate").val();


            // Validate that both dates are selected
            if (!startDate || !endDate) {
                return; // Do nothing if dates are incomplete
            }

            // Validate that startDate and endDate are different
            if (startDate === endDate) {
                showPopup(false, "Start Date and End Date must be different.");
                return; // Stop execution if validation fails
            }

            // Validate that startDate and endDate cant be after endDate
            if (startDate > endDate ) {
                showPopup(false, "Start Date can't be after End Date .");
                return; // Stop execution if validation fails
            }


            // Send an AJAX request to filter rooms
            $.ajax({
                url: filterUrl,
                type: 'GET',
                data: {
                    startDate: startDate,
                    endDate: endDate
                },
                success: function (response) {
                    // Update the room list container with the filtered results
                    $("#roomListContainer").html(response);
                },
                error: function () {
                    alert("Error while filtering rooms.");
                }
            });
        }

        // Handle changes in the date inputs to automatically filter rooms
        $("#startDate, #endDate").on("change", function () {
            fetchFilteredRooms();
        });

        // Handle the filter form submission using AJAX
        $("#filterForm").submit(function (e) {
            e.preventDefault(); // Prevent form from submitting the traditional way
            fetchFilteredRooms(); // Trigger filtering manually on form submission
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

        $(".add-to-cart-button").click(function (e) {
            e.preventDefault();  // Prevent the form from submitting the traditional way

            var roomId = $(this).data("room-id");
            var checkInDate = $(this).data("checkin-date");
            var checkOutDate = $(this).data("checkout-date");
            var actionUrl = $(this).data("url"); // Get the URL for the action

            var startDate = $("#startDate").val();
            var endDate = $("#endDate").val();
           
           
            if (!checkInDate || !checkOutDate) {
                showPopup(false, "Please select both check-in and check-out dates.");
                return;
            }

            // Validate that startDate cant be after endDate 
            if (startDate === endDate) {
                showPopup(false, "Start Date and End Date must be different.");
                return; // Stop execution if validation fails
            }

            // Validate that startDate and endDate are different
            if (startDate > endDate && endDate != now) {
                showPopup(false, "Start Date can't be after End Date .");
                return; // Stop execution if validation fails
            }

            $.ajax({
                url: actionUrl,  
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


});