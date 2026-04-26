// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Funcion de retroceso para Panel de Admin
function goBack() {

    const prev = sessionStorage.getItem("previousPage");

    if (prev && prev !== window.location.href) {
        window.location.href = prev;
    } else {
        window.location.href = '/Home/Index';
    }
}


//Funcion de AJAX para no recargar pagina en cliente
function ajaxPost(button) {

    const url = button.dataset.url;
    const id = button.dataset.id;

    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ id: id })
    })
        .then(async res => {
            if (!res.ok) {
                const text = await res.text();
                console.error(text);
                throw new Error("Error servidor");
            }
            return res.json();
        })
        .then(data => {

            if (data.success) {

                // recarga lo visual
                location.reload();

            } else {
                alert(data.message || "Error");
            }

        })
        .catch(err => {
            console.error(err);
            alert("Error");
        });
}