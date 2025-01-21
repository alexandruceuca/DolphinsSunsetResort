
$(document).ready(function () {
    // When a star is clicked, change the rating
    $('#star-rating i').on('click', function () {
        var value = $(this).data('value');
        // Update hidden input with the selected rating
        $('#rating').val(value);

        // Highlight the stars based on the selected rating
        $('#star-rating i').removeClass('text-primary');  // Remove the highlighted color from all stars
        for (var i = 1; i <= value; i++) {
            $('#star-rating i:nth-child(' + i + ')').addClass('text-primary'); // Add the highlighted color up to the selected star
        }
    });

    // Optional: Visual feedback when hovering over the stars
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
