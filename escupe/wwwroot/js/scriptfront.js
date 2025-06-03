const steps = document.querySelectorAll(".step");
let currentStep = 0;

function mostrarEtapa(index) {
    steps.forEach((step, i) => {
        step.classList.toggle("active", i === index);
    });
}

function proximaEtapa() {
    if (currentStep < steps.length - 1) {
        currentStep++;
        mostrarEtapa(currentStep);
    }
}

function voltarEtapa() {
    if (currentStep > 0) {
        currentStep--;
        mostrarEtapa(currentStep);
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const selectSituacao = document.getElementById("situacao_educacional");
    const divCursando = document.getElementById("educacao_cursando");
    const divConcluida = document.getElementById("educacao_concluida");

    // Inicialmente esconde os blocos
    divCursando.classList.add("escondido");
    divConcluida.classList.add("escondido");

    selectSituacao.addEventListener("change", function () {
        const valor = selectSituacao.value;

        // Sempre esconde antes de exibir o correto
        divCursando.classList.add("escondido");
        divConcluida.classList.add("escondido");

        if (valor === "cursando") {
            divCursando.classList.remove("escondido");
        } else if (valor === "concluido") {
            divConcluida.classList.remove("escondido");
        }
    });

    function validarEtapa1() {
        var telefone = document.getElementById('Telefone');
        var email = document.getElementById('Email');
        var senha = document.getElementById('Senha');
        var valido = true;

        telefone.classList.remove('input-invalido');
        email.classList.remove('input-invalido');
        senha.classList.remove('input-invalido');

        if (!telefone.value) {
            telefone.classList.add('input-invalido');
            valido = false;
        }
        if (!email.value || email.value.indexOf('@') === -1) {
            email.classList.add('input-invalido');
            valido = false;
        }
        if (!senha.value || senha.value.length < 6) {
            senha.classList.add('input-invalido');
            valido = false;
        }

        if (valido) {
            window.location.href = '/Usuarios/CadastrarEmpresa';
        }
    }


    // Lógica para mostrar o campo de descrição da experiência
    const selectExperiencia = document.getElementById("experiencia");
    const descricaoContainer = document.getElementById("descricao_experiencia_container");

    // Inicialmente esconde o campo de descrição
    descricaoContainer.classList.add("escondido");

    if (selectExperiencia) {
        selectExperiencia.addEventListener("change", function () {
            if (selectExperiencia.value === "sim") {
                descricaoContainer.classList.remove("escondido");
            } else {
                descricaoContainer.classList.add("escondido");
            }
        });
    }
});