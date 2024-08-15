function mostrarRegistrosSalud(jugadorId) {
    var mainContent = document.getElementById('contenedor_registros');

    fetch(`/Doctor/ObtenerDatosSalud?id=${jugadorId}`)
        .then(response => response.json())
        .then(data => {
            // Manipular los datos recibidos y construir la tabla
            let registrosSalud = data;
            let html = `
            <table id="healthTable">
                <thead>
                    <tr>
                        <th>Jugador</th>
                        <th>Frecuencia Cardiaca</th>
                        <th>Estatus</th>
                        <th>Fecha</th>
                    </tr>
                </thead>
                <tbody>`;

            if (registrosSalud && registrosSalud.length > 0) {
                registrosSalud.forEach(registro => {
                    html += `
                    <tr>
                        <td>${registro.jugador}</td>
                        <td>${registro.frecuenciaCardiaca}</td>
                        <td>${registro.estatus}</td>
                        <td>${new Date(registro.fecha).toLocaleDateString()}</td>
                    </tr>`;
                });
            } else {
                html += `
                <tr>
                    <td colspan="5">No hay registros de salud disponibles</td>
                </tr>`;
            }
            html += `</tbody></table>`;
            mainContent.innerHTML = html;
        })
        .catch(error => console.error('Error al obtener los registros de salud:', error));
}
