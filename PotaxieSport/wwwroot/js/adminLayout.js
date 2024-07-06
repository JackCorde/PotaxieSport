const hamBurger = document.querySelector(".toggle-btn");
const navToggleBtn = document.querySelector(".toggle-sidebar-btn");

function toggleSidebar() {
    document.querySelector("#sidebar").classList.toggle("expand");
}

hamBurger.addEventListener("click", toggleSidebar);
navToggleBtn.addEventListener("click", toggleSidebar);
