// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let displayMessage = function (message, msgType, title) {
    toastr.options = {
        closeButton: true,
        debug: false,
        newestOnTop: false,
        progressBar: true,
        positionClass: "toast-top-center",
        preventDuplicates: true,
        onclick: null,
        showDuration: "300",
        hideDuration: "1000",
        timeOut: "5000",
        extendedTimeOut: "1000",
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut",
    };
    toastr[msgType](message, title);
};

if ($("#success").val()) {
    displayMessage($("#success").val(), "success", "Sucesso!");
}
if ($("#info").val()) {
    displayMessage($("#info").val(), "info", "Informação!");
}
if ($("#warning").val()) {
    displayMessage($("#warning").val(), "warning", "Aviso!");
}
if ($("#error").val()) {
    displayMessage($("#error").val(), "error", "Error!");
}

function isEmail(email) {
    let regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function isTelefone(tel) {
    let regex = /\(\d{2,}\) \d{4,}\-\d{4}/;
    return regex.test(tel);
}





function backBtn() {
    window.history.back();
}

function changePage(controller, action, id) {
    if (id === undefined || id === null) {
        location.href = `/${controller}/${action}`;
        return;
    }
    location.href = `/${controller}/${action}/${id}`;
}

sidebarNone();
function sidebarNone() {
    if (screen.width <= 900) {
        const sidebar = document.querySelector(".sidebar");
        sidebar.classList.toggle("none");
    }
}

function successSwal(message = "Sucesso") {
    return Swal.fire({
        title: "Sucesso!",
        text: message,
        icon: "success",
        timer: 999999,
        allowEnterKey: true,
    });
}

function confirmSwal(title, text) {
    return Swal.fire({
        title: title,
        html: text,
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "Confirmar",
        confirmButtonColor: "var(--primary-color)",
    });
}
function approvedSwal(title, text) {
    return Swal.fire({
        title: title,
        html: text,
        icon: "success",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "Confirmar",
        confirmButtonColor: "var(--primary-color)",
    });
}
function cancelSwal(title, text) {
    return Swal.fire({
        title: title,
        html: text,
        icon: "error",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "Confirmar",
        confirmButtonColor: "var(--primary-color)",
    });
}

function deleteSwal(title, text) {
    return Swal.fire({
        title: title,
        html: text,
        imageUrl: "/imgs/lixo-icon.svg",
        imageWidth: 300,
        imageHeight: 150,
        imageAlt: "Custom image",
        showCancelButton: true,
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar",
        confirmButtonColor: "var(--primary-color)",
    });
}

function refersh(title, text) {
    return Swal.fire({
        title: title,
        html: text,
        imageUrl: "/imgs/refresh-icon.svg",
        imageWidth: 300,
        imageHeight: 150,
        imageAlt: "Custom image",
        showCancelButton: true,
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar",
        confirmButtonColor: "var(--primary-color)",
    });
}

function infoSwal(msg, options = {}) {
    return Swal.fire({
        title: "Informação",
        text: msg,
        icon: "info",
        confirmButtonText: "Ok",
        confirmButtonColor: "var(--primary-color)",
        cancelButtonText: "Cancelar",
        ...options
    });
}
function errorSwal(msg, options = {}) {
    return Swal.fire({
        title: "Erro",
        html: msg,
        //text: msg,
        icon: "error",
        confirmButtonText: "Ok",
        confirmButtonColor: "#f27474",
        //cancelButtonText: "Cancelar",
        ...options
    });
}

const formatter = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
});

function preventNonNumericalInput(e) {
    e = e || window.event;
    var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
    var charStr = String.fromCharCode(charCode);

    if (!charStr.match(/^[0-9]+$/))
        e.preventDefault();
}

