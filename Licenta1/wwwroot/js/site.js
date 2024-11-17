// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//------------------Script for slide show--------------------------
let currentSlide = 0;
const slides = document.querySelectorAll('.img-slide');

function showSlide(index) {
    slides.forEach((slide, i) => {
        slide.style.display = i === index ? 'block' : 'none';
    });

}

function nextSlide() {
    currentSlide = (currentSlide + 1) % slides.length;
    showSlide(currentSlide);
}

function prevSlide() {
    currentSlide = (currentSlide - 1 + slides.length) % slides.length;
    showSlide(currentSlide);
}

// Set an interval for automatic sliding, e.g., every 10 seconds
setInterval(nextSlide, 10000);

// Optionally, you can add event listeners for navigation
document.getElementById('nextButton').addEventListener('click', nextSlide);
document.getElementById('prevButton').addEventListener('click', prevSlide);

// Show the initial slide
showSlide(currentSlide);




//------------------Script for slide show--------------------------


//-----------------Script for date-picker



//-----------------

