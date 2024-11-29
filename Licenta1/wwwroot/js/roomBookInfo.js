$(document).ready(function () {
    const now = new Date();
    function showPopup(success, message) {
        Swal.fire({
            title: success ? 'Success' : 'Error',
            text: message,
            icon: success ? 'success' : 'error',
            confirmButtonText: 'OK'
        });
    }

    $(".add-to-cart-button-info").click(function (e) {
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

});