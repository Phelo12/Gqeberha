// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    // Check if the student has registered (replace 'hasRegistered' with your condition)
    var hasRegistered = true; // Replace with your logic to determine if the student has registered

    // If the student has registered, show the modal
    if (hasRegistered) {
        var modal = new bootstrap.Modal(document.getElementById("myModal"));
        modal.show();

        // Add an event listener to close the modal when the 'x' button is clicked
        var closeButton = document.getElementById("close");
        closeButton.addEventListener("click", function () {
            modal.hide();
        });
    }
    //TempData
    setTimeout(function () {
        var tempDataMessage = document.getElementById('tempDataMessage');
        if (tempDataMessage) {

            tempDataMessage.remove();
            tempDataMessage.textContent = '';

        }
    }, 9000);

});
$(document).ready(function () {
    $('#MyTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'pdf', 'print'
        ]
    });
});