
$(document).ready(function () {

    $('#ratingModal').on('shown.bs.modal', function (e) {

        var bookingId = $(e.relatedTarget).data('booking-id');

        // Call an AJAX request to get the current rating for the booking
        $.ajax({
            url: '/Booking/GetBookingRating',
            type: 'GET',
            data: { bookingId: bookingId },
            success: function (response) {

                var currentRating = response.rating; 
                var recommendation = response.recommendation; 

                // Set the hidden input for rating
                $('#rating').val(currentRating);

                // Highlight the stars based on the current rating
                $('#star-rating i').each(function () {
                    var value = $(this).data('value');
                    if (value <= currentRating) {
                        $(this).addClass('text-warning'); // Highlight the stars up to the rating
                    } else {
                        $(this).removeClass('text-warning'); // Remove the highlight from the remaining stars
                    }
                });

                // Set the recommendation radio buttons based on the current value
                if (recommendation == 1) {
                    $('#recommendYes').prop('checked', true);
                } else if (recommendation == 2) {
                    $('#recommendNo').prop('checked', true);
                } else if (recommendation == 3) {
                    $('#recommendNoComment').prop('checked', true);
                }
            },
            error: function () {
                Swal.fire('Error', 'Failed to load booking details.', 'error');
            }
        });
    })

    // When a star is clicked, change the rating
    $('#star-rating i').on('click', function () {
        var value = $(this).data('value');
        // Update hidden input with the selected rating
        $('#rating').val(value);

        // Highlight the stars based on the selected rating
        $('#star-rating i').removeClass('text-warning');  // Remove the highlighted color from all stars
        for (var i = 1; i <= value; i++) {
            $('#star-rating i:nth-child(' + i + ')').addClass('text-warning'); // Add the highlighted color up to the selected star
        }
    });

  
    $('#star-rating i').hover(function () {
        var value = $(this).data('value');
        $('#star-rating i').removeClass('text-primary');
        for (var i = 1; i <= value; i++) {
            $('#star-rating i:nth-child(' + i + ')').addClass('text-primary');
        }
    }, function () {
        var selectedValue = $('#rating').val();
        $('#star-rating i').removeClass('text-primary');
        for (var i = 1; i <= selectedValue; i++) {
            $('#star-rating i:nth-child(' + i + ')').addClass('text-primary');
        }
    });
});
