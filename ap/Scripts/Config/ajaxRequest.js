// Función AJAX genérica
function ajaxRequest(url, method, data, successCallback, errorCallback) {
    $.ajax({
        url: url,
        type: method,
        data: data,
        contentType: 'application/json; charset=utf-8',  
        dataType: 'json', 
        success: function (response) {
            if (successCallback) {
                successCallback(response); 
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            if (errorCallback) {
                errorCallback(xhr, status, error);  
            }
        }
    });
}
