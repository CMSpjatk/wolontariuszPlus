function deleteAlert(area, controller, action, parameterName, parameterValue, whatToDelete, e) {
    e.preventDefault();

    Swal({
        title: `Czy na pewno chcesz usunąć ${whatToDelete}?`,
        text: "Tej operacji nie będzie można cofnąć!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Tak, usuń!',
        cancelButtonText: 'Anuluj'
    }).then((result) => {
        if (result.value) {
            Swal(
                'Usunięto!',
                'Operacja wykonana pomyślnie.',
                'success'
            );

            let xhttp = new XMLHttpRequest();
            let formData = new FormData();
            formData.append(parameterName, parameterValue);

            xhttp.onload = () => {
                if (xhttp.readyState === 4 && xhttp.status === 200) {
                    window.location.replace(xhttp.responseText);
                }
                else {
                    document.body.innerHTML = xhttp.responseText;
                }
            };

            xhttp.open("POST", `/${area}/${controller}/${action}`);
            xhttp.send(formData);
        }
    });
}

