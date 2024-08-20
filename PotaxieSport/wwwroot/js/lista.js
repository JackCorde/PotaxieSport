$(document).ready(function () {
    $("#equipoV").change(function () {
        var equipoid = $(this).val();
        var torneoId = $("#torneoId").val();  // Corregido

        $.ajax({
            url: "/Compartido/ObtenerSubSubEquipos",
            type: "GET",
            data: { equipoId: equipoid, torneoId: torneoId },  // Corregido
            success: function (data) {
                var subcategoriaDropdown = $("#equipoD");
                subcategoriaDropdown.empty();

                subcategoriaDropdown.append("<option value=''>Seleccionar Equipo</option>");
                $.each(data, function (i, item) {
                    subcategoriaDropdown.append("<option value='" + item.EquipoId + "'>" + item.EquipoNombre + "</option>");
                });
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

    $("#hora").change(function () {
        var hora = $(this).val();
        var fecha = $("#fecha").val();  // Corregido

        $.ajax({
            url: "/Compartido/ObtenerSubArbitros",
            type: "GET",
            data: {fecha: fecha, hora: hora },  // Corregido
            success: function (data) {
                var subcategoriaDropdown = $("#arbitro");
                subcategoriaDropdown.empty();

                subcategoriaDropdown.append("<option value=''>Selecciona árbitro</option>");
                $.each(data, function (i, item) {
                    subcategoriaDropdown.append("<option value=" + item.UsuarioId + ">" + item.Nombre + " " + item.ApPaterno + "</option>");
                });
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });
});
