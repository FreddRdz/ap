$(document).ready(function () {

    console.log("Hola")

    $('#clienteForm').submit(function (e) {
        e.preventDefault(); 

        // Serializar los datos del formulario
        const clienteData = {
            Nombre: $('#Nombre').val(),
            Apellido: $('#Apellido').val(),
            CorreoElectronico: $('#CorreoElectronico').val(),
            Telefono: Number($('#Telefono').val()) ,
            Direccion: $('#Direccion').val(),
            Contrasenia: $('#Contraseña').val()
        };

        // Convertir el objeto a JSON
        const jsonData = JSON.stringify(clienteData);

        // Usar la función AJAX genérica para enviar los datos del formulario
        ajaxRequest('/Auth/SignUp', 'POST', jsonData, function (response) {
            if (response.success) {
                window.location.href = response.redirectUrl;
            } else {
                // Mostrar los errores específicos en los campos
                $('#errorNombre').text(response.errors.Nombre || '');
                $('#errorApellido').text(response.errors.Apellido || '');
                $('#errorCorreoElectronico').text(response.errors.CorreoElectronico || '');
                $('#errorTelefono').text(response.errors.Telefono || '');
                $('#errorDireccion').text(response.errors.Direccion || '');
                $('#errorContraseña').text(response.errors.Contraseña || '');
            }
        }, function (xhr, status, error) {
            console.error("Error al registrar el cliente:", error);
            alert("Ocurrió un error al intentar registrar al cliente.", error);
        });
    });
});