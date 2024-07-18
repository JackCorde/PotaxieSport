const hamBurger = document.querySelector(".toggle-btn");
const navToggleBtn = document.querySelector(".toggle-sidebar-btn");

function toggleSidebar() {
    document.querySelector("#sidebar").classList.toggle("expand");
}

hamBurger.addEventListener("click", toggleSidebar);
navToggleBtn.addEventListener("click", toggleSidebar);




document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('searchButton').addEventListener('click', function () {
        var searchText = document.getElementById('searchInput').value.trim().toLowerCase();
        var tableRows = document.getElementById('usuarios-table').getElementsByTagName('tbody')[0].getElementsByTagName('tr');

        for (var i = 0; i < tableRows.length; i++) {
            var userName = tableRows[i].getElementsByTagName('td')[0].textContent.trim().toLowerCase();

            if (userName.includes(searchText)) {
                tableRows[i].style.display = '';
            } else {
                tableRows[i].style.display = 'none';
            }
        }
    });
});
