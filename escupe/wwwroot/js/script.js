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
    // Situação educacional
    const selectSituacao = document.getElementById("situacao_educacional");
    const divCursando = document.getElementById("educacao_cursando");
    const divConcluida = document.getElementById("educacao_concluida");
    if (selectSituacao && divCursando && divConcluida) {
        divCursando.classList.add("escondido");
        divConcluida.classList.add("escondido");
        selectSituacao.addEventListener("change", function () {
            divCursando.classList.add("escondido");
            divConcluida.classList.add("escondido");
            if (this.value === "cursando") divCursando.classList.remove("escondido");
            else if (this.value === "concluido") divConcluida.classList.remove("escondido");
        });
    }

    // Experiência profissional
    const selectExperiencia = document.getElementById("experiencia");
    const descricaoContainer = document.getElementById("descricao_experiencia_container");
    if (selectExperiencia && descricaoContainer) {
        descricaoContainer.classList.add("escondido");
        selectExperiencia.addEventListener("change", function () {
            descricaoContainer.classList.toggle("escondido", this.value !== "sim");
        });
    }

    // Validação de CPF
    const cpfInput = document.getElementById("cpf");
    if (cpfInput) {
        cpfInput.addEventListener("blur", function () {
            const cpf = this.value.replace(/\D/g, "");
            if (cpf && !validarCPF(cpf)) {
                alert("CPF inválido!");
                this.focus();
            }
        });
    }

    // Validação de CNPJ
    const cnpjInput = document.getElementById("cnpj");
    if (cnpjInput) {
        cnpjInput.addEventListener("blur", function () {
            const cnpj = this.value.replace(/\D/g, "");
            if (cnpj && !validarCNPJ(cnpj)) {
                alert("CNPJ inválido!");
                this.focus();
            }
        });
    }

    // Preenchimento de endereço pelo CEP
    const cepInput = document.getElementById("cep");
    if (cepInput) {
        cepInput.addEventListener("blur", function () {
            const cep = this.value.replace(/\D/g, "");
            if (cep.length === 8) {
                fetch(`https://viacep.com.br/ws/${cep}/json/`)
                    .then(response => response.json())
                    .then(data => {
                        if (!data.erro) {
                            document.getElementById("logradouro").value = data.logradouro || "";
                            document.getElementById("bairro").value = data.bairro || "";
                            document.getElementById("cidade").value = data.localidade || "";
                            document.getElementById("uf").value = data.uf || "";
                        } else {
                            alert("CEP não encontrado.");
                        }
                    })
                    .catch(() => alert("Erro ao buscar o CEP."));
            } else if (cep) {
                alert("CEP inválido.");
            }
        });
    }
});
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

document.addEventListener("DOMContentLoaded", function () {
    // Validação de CPF
    const cpfInput = document.getElementById("cpf");
    if (cpfInput) {
        cpfInput.addEventListener("blur", function () {
            const cpf = this.value.replace(/\D/g, "");
            if (!validarCPF(cpf)) {
                alert("CPF inválido!");
                this.focus();
            }
        });
    }

    // Validação de CNPJ
    const cnpjInput = document.getElementById("cnpj");
    if (cnpjInput) {
        cnpjInput.addEventListener("blur", function () {
            const cnpj = this.value.replace(/\D/g, "");
            if (!validarCNPJ(cnpj)) {
                alert("CNPJ inválido!");
                this.focus();
            }
        });
    }

    // Preenchimento automático de endereço com base no CEP
    const cepInput = document.getElementById("cep");
    if (cepInput) {
        cepInput.addEventListener("blur", function () {
            const cep = this.value.replace(/\D/g, "");
            if (cep.length === 8) {
                fetch(`https://viacep.com.br/ws/${cep}/json/`)
                    .then(response => response.json())
                    .then(data => {
                        if (!data.erro) {
                            document.getElementById("logradouro").value = data.logradouro || "";
                            document.getElementById("bairro").value = data.bairro || "";
                            document.getElementById("cidade").value = data.localidade || "";
                            document.getElementById("uf").value = data.uf || "";
                        } else {
                            alert("CEP não encontrado.");
                        }
                    })
                    .catch(() => alert("Erro ao buscar o CEP."));
            } else {
                alert("CEP inválido.");
            }
        });
    }
});

// Funções de validação de CPF e CNPJ
function validarCPF(cpf) {
    cpf = cpf.replace(/\D/g, "");
    if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) return false;

    let soma = 0, resto;
    for (let i = 1; i <= 9; i++) soma += parseInt(cpf.substring(i - 1, i)) * (11 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.substring(9, 10))) return false;

    soma = 0;
    for (let i = 1; i <= 10; i++) soma += parseInt(cpf.substring(i - 1, i)) * (12 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    return resto === parseInt(cpf.substring(10, 11));
}

function validarCNPJ(cnpj) {
    cnpj = cnpj.replace(/\D/g, "");
    if (cnpj.length !== 14 || /^(\d)\1+$/.test(cnpj)) return false;

    let tamanho = cnpj.length - 2;
    let numeros = cnpj.substring(0, tamanho);
    let digitos = cnpj.substring(tamanho);
    let soma = 0, pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) pos = 9;
    }

    let resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
    if (resultado !== parseInt(digitos.charAt(0))) return false;

    tamanho++;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
    return resultado === parseInt(digitos.charAt(1));
}