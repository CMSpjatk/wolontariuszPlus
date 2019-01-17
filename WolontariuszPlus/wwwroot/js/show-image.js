let imgInput = document.getElementById('img-input');

imgInput.addEventListener('input', () => {
    let files = imgInput.files;
    let file = files.item(0);
    let div = document.getElementById('uploaded-image-container');

    if (file) {
        let img = document.createElement('img');
        img.src = URL.createObjectURL(file);
        img.alt = file.name;
        div.innerHTML = '';
        div.appendChild(img);
    }
    else {
        div.innerHTML = '';
    }
});