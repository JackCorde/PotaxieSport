const hamBurger = document.querySelector(".toggle-btn");
const navToggleBtn = document.querySelector(".toggle-sidebar-btn");

function toggleSidebar() {
    document.querySelector("#sidebar").classList.toggle("expand");
}

hamBurger.addEventListener("click", toggleSidebar);
navToggleBtn.addEventListener("click", toggleSidebar);
//paginacion

document.addEventListener("DOMContentLoaded", function () {
    const usuarios = JSON.parse(document.getElementById('usuarios-data').textContent); // Obtener los datos del elemento script
    const rowsPerPage = 10;
    const tableBody = document.getElementById("usuarios-tbody");
    const pagination = document.getElementById("pagination");
    let currentPage = 1;
    let filteredUsuarios = usuarios;

    function renderTable(page, data) {
        tableBody.innerHTML = "";
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        const paginatedUsers = data.slice(start, end);

        paginatedUsers.forEach(usuario => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${usuario.nombre}</td>
                <td>${usuario.apPaterno}</td>
                <td>${usuario.apMaterno}</td>
                <td>${usuario.username}</td>
                <td>${usuario.email}</td>
                <td class="action-icons">
                <a href="/Administrador/ActualizarUsuario/${usuario.usuarioId}">
                    <i class="fas fa-edit"></i>
                </a>
            </td>            `;
            tableBody.appendChild(row);
        });
    }

    function renderPagination(data) {
        pagination.innerHTML = "";
        const totalPages = Math.ceil(data.length / rowsPerPage);

        for (let i = 1; i <= totalPages; i++) {
            const button = document.createElement("button");
            button.innerText = i;
            button.className = "btn btn-secondary me-2";
            button.addEventListener("click", () => {
                currentPage = i;
                renderTable(currentPage, data);
                updatePagination();
            });
            pagination.appendChild(button);
        }
    }

    function updatePagination() {
        Array.from(pagination.children).forEach((button, index) => {
            button.classList.toggle("btn-primary", index + 1 === currentPage);
        });
    }

    function filterUsers(searchTerm) {
        return usuarios.filter(usuario =>
            usuario.nombre.toLowerCase().includes(searchTerm) ||
            usuario.apPaterno.toLowerCase().includes(searchTerm) ||
            usuario.apMaterno.toLowerCase().includes(searchTerm) ||
            usuario.username.toLowerCase().includes(searchTerm) ||
            usuario.email.toLowerCase().includes(searchTerm)
        );
    }

    // Event listener for search input (dynamic search)
    document.getElementById("searchInput").addEventListener("input", function () {
        const searchTerm = this.value.toLowerCase();
        filteredUsuarios = filterUsers(searchTerm);
        currentPage = 1;
        renderTable(currentPage, filteredUsuarios);
        renderPagination(filteredUsuarios);
    });

    renderTable(currentPage, filteredUsuarios);
    renderPagination(filteredUsuarios);
});






