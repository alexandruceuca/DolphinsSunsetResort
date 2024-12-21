 function showPopup(success, message) {
        Swal.fire({
            title: success ? 'Success' : 'Error',
            text: message,
            icon: success ? 'success' : 'error',
            confirmButtonText: 'OK'
        });
    }
