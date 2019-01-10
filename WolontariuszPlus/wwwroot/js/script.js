function deleteAlert(area, controller, action, parameter, e) {
    e.preventDefault();

    Swal({
        title: 'Czy na pewno chcesz usunąć to wydarzenie?',
        text: "Nie będzie można go przywrócić!",
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
                'Wydarzenie zostało usunięte.',
                'success'
            )
            
            let xhttp = new XMLHttpRequest();
            let formData = new FormData();
            formData.append("EventId", parameter);

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
    })
}