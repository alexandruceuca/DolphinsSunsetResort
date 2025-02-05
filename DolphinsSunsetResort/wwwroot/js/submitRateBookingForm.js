
$(document).on('submit', '#ratingForm', function (e) {
        e.preventDefault();
        const form = $(this);
        const url = $(this).data("url");
        const data = form.serialize();
 
        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            success: function (response) {
                if (response.success) {
 
                    $('#ratingModal').modal('hide');
                    showPopup(true, "Thank you for you rating!");
                } else {
                    Swal.fire('Error', 'Something went wrong.', 'error');
                }
            },
            error: function () {
                Swal.fire('Error', 'Something went wrong.', 'error');
            }
        });
    });