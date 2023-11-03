document.addEventListener("DOMContentLoaded", function() {
    const editLinks = document.querySelectorAll(".edit-link");
    const detailsLinks = document.querySelectorAll(".details-link");
    const deleteLinks = document.querySelectorAll(".delete-link");

    // Example event listeners (replace with your own logic)
    editLinks.forEach(function(link) {
        link.addEventListener("click", function(event) {
            event.preventDefault();
            // Add your edit functionality here
        });
    });

    detailsLinks.forEach(function(link) {
        link.addEventListener("click", function(event) {
            event.preventDefault();
            // Add your details functionality here
        });
    });

    deleteLinks.forEach(function(link) {
        link.addEventListener("click", function(event) {
            event.preventDefault();
            // Add your delete functionality here
        });
    });
});
