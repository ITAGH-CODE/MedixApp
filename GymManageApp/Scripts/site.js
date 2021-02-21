function NotifyUtilisateur(type, message) {
    if (type == "error") {
        $.notify({
            title: '<strong>Message : </strong>',
            message: message
        }, {
            offset: {
                y: 50
            },
            placement: {
                align: 'center'
            },
            type: 'danger',
        });
    }
    else if (type == "success") {
        $.notify({
            title: '<strong>Message : </strong>',
            message: message
        }, {
            offset: {
                y: 50
            },
            placement: {
                align: 'center'
            },
            type: 'success'
        });
    }
}

function GetWorkOrders(){
    $.ajax({
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json;charset=utf8-8',
        url: '/WorkOrders/GetWorkOrders',
        success: function (response) {
            $('#count-modal').modal('show');
            document.getElementById("wo-count").innerText = response.Count;
        }
    });
}