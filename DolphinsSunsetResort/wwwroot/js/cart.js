$(function () {

	$(".RemoveLink").click(function () {
		// Get the id from the link
		var recordToDelete = $(this).attr("data-id");
		if (recordToDelete != '') {
			// Perform the ajax post
			$.post("/BookingCart/RemoveFromCart", { "id": recordToDelete },
				function (data) {
					// Successful requests get here
					// Update the page elements
					$('#row-' + data.deleteId).fadeOut('slow', function () {
						$(this).remove(); // Remove the row after fading out
					});
					$('#cart-total').text(data.cartTotal);
					$('#update-message').text(data.message);
				});
		}
	});
});
