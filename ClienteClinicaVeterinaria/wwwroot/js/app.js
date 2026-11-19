const API = "http://localhost:5090/api";

const state = {
    view: "dashboard",
    tutores: [],
    animais: [],
    consultas: [],
    historicos: [],
    animalHistoricoId: null,
    modal: null
};

const $ = (id) => document.getElementById(id);
const content = $("content");


function escapeHtml(value) {
    return String(value ?? "").replace(/[&<>"']/g, (char) => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#039;"
    })[char]);
}


function formatDate(value) {
    if (!value) return "—";

    const date = new Date(`${String(value).slice(0, 10)}T00:00:00`);

    return Number.isNaN(date.getTime())
        ? value
        : date.toLocaleDateString("pt-BR");
}


function formatDateTime(value) {
    if (!value) return "—";

    const date = new Date(value);

    return Number.isNaN(date.getTime())
        ? value
        : date.toLocaleString("pt-BR");
}


function formatTime(value) {
    return value ? String(value).slice(0, 5) : "—";
}


// COMUNICAÇÃO COM A API

async function api(path, options = {}) {

    let response;

    try {
        response = await fetch(`${API}${path}`, {
            ...options,
            headers: {
                ...(options.body
                    ? { "Content-Type": "application/json" }
                    : {}),
                ...(options.headers || {})
            }
        });

    } catch {
        throw new Error(
            "Não foi possível conectar à API. Confira se ela está em execução em http://localhost:5090."
        );
    }

    const responseText =
        response.status === 204
            ? ""
            : await response.text();

    let data = null;

    if (responseText) {
        try {
            data = JSON.parse(responseText);
        } catch {
            data = responseText;
        }
    }

    if (!response.ok) {

        const message =
            data?.mensagem ||
            data?.title ||
            data?.detail ||
            data ||
            `Erro HTTP ${response.status}`;

        throw new Error(
            typeof message === "string"
                ? message
                : `Erro HTTP ${response.status}`
        );
    }

    return data;
}


// MENSAGENS NA INTERFACE

function toast(message, type = "success") {

    const item = document.createElement("div");

    item.className = `toast ${type}`;
    item.textContent = message;

    $("toastContainer").appendChild(item);

    setTimeout(() => item.remove(), 3800);
}


// CARREGAR INFORMAÇÕES DO BANCO PELA API

async function refreshData(showFeedback = false) {

    try {

        const [tutores, animais, consultas] = await Promise.all([
            api("/tutores"),
            api("/animais"),
            api("/consultas")
        ]);

        state.tutores = tutores || [];
        state.animais = animais || [];
        state.consultas = consultas || [];

        $("apiStatus").classList.add("online");
        $("apiStatus").classList.remove("offline");

        $("apiStatusText").textContent = "API conectada";

        render();

        if (showFeedback) {
            toast("Dados atualizados.");
        }

    } catch (error) {

        $("apiStatus").classList.add("offline");
        $("apiStatus").classList.remove("online");

        $("apiStatusText").textContent = "API desconectada";

        if (showFeedback) {
            toast(error.message, "error");
        } else {

            content.innerHTML = `
                <div class="empty-state">
                    <div class="empty-icon">⚠</div>

                    <h2>Não foi possível carregar os dados</h2>

                    <p>${escapeHtml(error.message)}</p>

                    <button class="button primary" id="tryAgain">
                        Tentar novamente
                    </button>
                </div>
            `;
        }
    }
}


// NAVEGAÇÃO

const pageNames = {
    dashboard: "Visão geral",
    tutores: "Tutores",
    animais: "Animais",
    consultas: "Consultas",
    historico: "Histórico clínico"
};


function setView(view) {

    state.view = view;

    document.querySelectorAll("[data-view]").forEach(button => {

        button.classList.toggle(
            "active",
            button.dataset.view === view
        );

    });

    $("pageTitle").textContent = pageNames[view] || "Clínica veterinária";

    render();
}


// NOMES RELACIONADOS

function tutorName(id) {

    return state.tutores.find(t => t.id === id)?.nome
        || `Tutor #${id}`;
}


function animalName(id) {

    return state.animais.find(a => a.id === id)?.nome
        || `Animal #${id}`;
}


// STATUS DAS CONSULTAS

function statusBadge(status) {

    const finalizada =
        String(status).toLowerCase() === "finalizada";

    return `
        <span class="status ${finalizada ? "done" : "pending"}">
            <span></span>
            ${escapeHtml(status || "—")}
        </span>
    `;
}


// TELA INICIAL

function renderDashboard() {

    const agendadas =
        state.consultas.filter(c => c.status === "Agendada").length;

    const finalizadas =
        state.consultas.filter(c => c.status === "Finalizada").length;

    const recentes = [...state.consultas]
        .sort((a, b) =>
            `${b.data} ${b.horario}`.localeCompare(
                `${a.data} ${a.horario}`
            )
        )
        .slice(0, 5);

    return `

        <section class="welcome-card">

            <div class="welcome-copy">

                <span class="eyebrow light">
                    CUIDADO QUE FAZ BEM
                </span>

                <h2>
                    Olá! Seja bem-vindo(a) à VetCare.
                </h2>

                <p>
                    Organize os atendimentos e acompanhe
                    a saúde dos seus pacientes em um só lugar.
                </p>

                <button class="button white" data-view="consultas">
                    Ver consultas <span>→</span>
                </button>

            </div>

            <div class="welcome-art" aria-hidden="true">
                <span class="paw">🐾</span>
                <span class="circle circle-one"></span>
                <span class="circle circle-two"></span>
            </div>

        </section>


        <div class="stats-grid">

            <article class="stat-card">

                <div class="stat-icon lilac">♙</div>

                <div>
                    <span>Tutores cadastrados</span>
                    <strong>${state.tutores.length}</strong>
                    <small>Responsáveis registrados</small>
                </div>

            </article>


            <article class="stat-card">

                <div class="stat-icon mint">🐾</div>

                <div>
                    <span>Animais cadastrados</span>
                    <strong>${state.animais.length}</strong>
                    <small>Pacientes da clínica</small>
                </div>

            </article>


            <article class="stat-card">

                <div class="stat-icon peach">▣</div>

                <div>
                    <span>Consultas agendadas</span>
                    <strong>${agendadas}</strong>
                    <small>Atendimentos pendentes</small>
                </div>

            </article>


            <article class="stat-card">

                <div class="stat-icon blue">✓</div>

                <div>
                    <span>Consultas finalizadas</span>
                    <strong>${finalizadas}</strong>
                    <small>Atendimentos concluídos</small>
                </div>

            </article>

        </div>


        <div class="dashboard-grid">

            <section class="panel recent-panel">

                <div class="panel-heading">

                    <div>
                        <span class="eyebrow">ACOMPANHAMENTO</span>
                        <h3>Consultas recentes</h3>
                    </div>

                    <button class="text-button" data-view="consultas">
                        Ver todas <span>→</span>
                    </button>

                </div>


                ${recentes.length ? `

                    <div class="table-wrap">

                        <table>

                            <thead>
                                <tr>
                                    <th>Paciente</th>
                                    <th>Data</th>
                                    <th>Horário</th>
                                    <th>Status</th>
                                </tr>
                            </thead>

                            <tbody>

                                ${recentes.map(c => `

                                    <tr>

                                        <td>
                                            <strong>
                                                ${escapeHtml(animalName(c.animalId))}
                                            </strong>

                                            <small class="cell-sub">
                                                Consulta #${c.id}
                                            </small>
                                        </td>

                                        <td>${formatDate(c.data)}</td>

                                        <td>${formatTime(c.horario)}</td>

                                        <td>${statusBadge(c.status)}</td>

                                    </tr>

                                `).join("")}

                            </tbody>

                        </table>

                    </div>

                ` : `

                    <div class="empty-inline">
                        Nenhuma consulta cadastrada ainda.
                    </div>

                `}

            </section>


            <section class="panel shortcuts-panel">

                <div class="panel-heading">

                    <div>
                        <span class="eyebrow">ACESSO RÁPIDO</span>
                        <h3>O que deseja fazer?</h3>
                    </div>

                </div>


                <button class="shortcut" data-new="tutor">

                    <span class="shortcut-icon lilac">♙</span>

                    <span>
                        <strong>Cadastrar tutor</strong>
                        <small>Adicionar responsável</small>
                    </span>

                    <b>→</b>

                </button>


                <button class="shortcut" data-new="animal">

                    <span class="shortcut-icon mint">🐾</span>

                    <span>
                        <strong>Cadastrar animal</strong>
                        <small>Adicionar paciente</small>
                    </span>

                    <b>→</b>

                </button>


                <button class="shortcut" data-new="consulta">

                    <span class="shortcut-icon peach">＋</span>

                    <span>
                        <strong>Agendar consulta</strong>
                        <small>Marcar atendimento</small>
                    </span>

                    <b>→</b>

                </button>

            </section>

        </div>

    `;
}


// ESTRUTURA DAS TABELAS

function tablePage(
    title,
    description,
    buttonText,
    entity,
    headers,
    rows
) {

    return `

        <div class="section-heading">

            <div>
                <span class="eyebrow">GERENCIAMENTO</span>
                <h2>${title}</h2>
                <p>${description}</p>
            </div>

            <button class="button primary" data-new="${entity}">
                <span>＋</span> ${buttonText}
            </button>

        </div>


        <section class="panel table-panel">

            <div class="table-toolbar">

                <div class="search-box">
                    <span>⌕</span>

                    <input
                        type="search"
                        data-search
                        placeholder="Buscar ${title.toLowerCase()}..."
                    >
                </div>

                <span class="result-count">
                    ${rows.length} registro(s)
                </span>

            </div>


            <div class="table-wrap">

                <table>

                    <thead>
                        <tr>
                            ${headers.map(h => `<th>${h}</th>`).join("")}
                        </tr>
                    </thead>

                    <tbody>

                        ${rows.length
        ? rows.join("")
        : `
                                <tr>
                                    <td colspan="${headers.length}" class="empty-cell">
                                        Nenhum registro encontrado.
                                    </td>
                                </tr>
                            `
    }

                    </tbody>

                </table>

            </div>

        </section>

    `;
}


// TUTORES

function renderTutores() {

    const rows = state.tutores.map(t => `

        <tr>

            <td>
                <div class="person-cell">

                    <span class="avatar">
                        ${escapeHtml((t.nome || "T").charAt(0).toUpperCase())}
                    </span>

                    <div>
                        <strong>${escapeHtml(t.nome)}</strong>

                        <small class="cell-sub">
                            #${t.id}
                        </small>
                    </div>

                </div>
            </td>

            <td>${escapeHtml(t.email)}</td>

            <td>${escapeHtml(t.telefone)}</td>

            <td>
                <div class="row-actions">

                    <button class="icon-button"
                        data-action="edit-tutor"
                        data-id="${t.id}"
                        title="Editar">✎</button>

                    <button class="icon-button danger"
                        data-action="delete-tutor"
                        data-id="${t.id}"
                        title="Excluir">×</button>

                </div>
            </td>

        </tr>

    `);

    return tablePage(
        "Tutores",
        "Gerencie os responsáveis pelos pacientes da clínica.",
        "Novo tutor",
        "tutor",
        ["TUTOR", "E-MAIL", "TELEFONE", "AÇÕES"],
        rows
    );
}


// ANIMAIS

function renderAnimais() {

    const rows = state.animais.map(a => `

        <tr>

            <td>
                <div class="person-cell">

                    <span class="pet-avatar">🐾</span>

                    <div>
                        <strong>${escapeHtml(a.nome)}</strong>

                        <small class="cell-sub">
                            #${a.id}
                        </small>
                    </div>

                </div>
            </td>

            <td>${escapeHtml(a.especie)}</td>

            <td>${escapeHtml(a.raca)}</td>

            <td>${escapeHtml(a.sexo)}</td>

            <td>${escapeHtml(tutorName(a.tutorId))}</td>

            <td>
                <div class="row-actions">

                    <button class="icon-button"
                        data-action="history-animal"
                        data-id="${a.id}"
                        title="Histórico">▤</button>

                    <button class="icon-button"
                        data-action="edit-animal"
                        data-id="${a.id}"
                        title="Editar">✎</button>

                    <button class="icon-button danger"
                        data-action="delete-animal"
                        data-id="${a.id}"
                        title="Excluir">×</button>

                </div>
            </td>

        </tr>

    `);

    return tablePage(
        "Animais",
        "Acompanhe os pacientes e seus respectivos tutores.",
        "Novo animal",
        "animal",
        ["PACIENTE", "ESPÉCIE", "RAÇA", "SEXO", "TUTOR", "AÇÕES"],
        rows
    );
}


// CONSULTAS

function renderConsultas() {

    const rows = state.consultas.map(c => `

        <tr>

            <td>
                <strong>${escapeHtml(animalName(c.animalId))}</strong>

                <small class="cell-sub">
                    Consulta #${c.id}
                </small>
            </td>

            <td>${formatDate(c.data)}</td>

            <td>${formatTime(c.horario)}</td>

            <td>${escapeHtml(c.motivo)}</td>

            <td>${statusBadge(c.status)}</td>

            <td>
                <div class="row-actions">

                    ${c.status === "Agendada" ? `

                        <button class="icon-button"
                            data-action="edit-consulta"
                            data-id="${c.id}"
                            title="Editar">✎</button>

                        <button class="icon-button success"
                            data-action="finish-consulta"
                            data-id="${c.id}"
                            title="Finalizar">✓</button>

                        <button class="icon-button danger"
                            data-action="delete-consulta"
                            data-id="${c.id}"
                            title="Excluir">×</button>

                    ` : `

                        <button class="icon-button"
                            data-action="history-animal"
                            data-id="${c.animalId}"
                            title="Ver histórico">▤</button>

                    `}

                </div>
            </td>

        </tr>

    `);

    return tablePage(
        "Consultas",
        "Agende, acompanhe e finalize os atendimentos.",
        "Agendar consulta",
        "consulta",
        ["PACIENTE", "DATA", "HORÁRIO", "MOTIVO", "STATUS", "AÇÕES"],
        rows
    );
}


// HISTÓRICO CLÍNICO

function renderHistorico() {

    const options = state.animais.map(a => `

        <option value="${a.id}"
            ${Number(state.animalHistoricoId) === a.id ? "selected" : ""}>

            ${escapeHtml(a.nome)} — ${escapeHtml(a.especie)} (#${a.id})

        </option>

    `).join("");


    const rows = state.historicos.map(h => `

        <tr>

            <td>${formatDateTime(h.dataRegistro)}</td>

            <td>
                <span class="type-tag">
                    ${escapeHtml(h.tipoRegistro)}
                </span>
            </td>

            <td>${escapeHtml(h.descricao)}</td>

            <td>${h.consultaId ? `#${h.consultaId}` : "—"}</td>

        </tr>

    `).join("");


    return `

        <div class="section-heading">

            <div>
                <span class="eyebrow">SAÚDE DO PACIENTE</span>
                <h2>Histórico clínico</h2>

                <p>
                    Consulte vacinas, exames, observações e registros das consultas.
                </p>
            </div>

        </div>


        <section class="panel history-filter">

            <div class="filter-copy">

                <span class="filter-icon">▤</span>

                <div>
                    <strong>Selecione um paciente</strong>

                    <small>
                        Escolha o animal para visualizar seu histórico.
                    </small>
                </div>

            </div>


            <div class="filter-controls">

                <select id="historyAnimalSelect">

                    <option value="">Selecione um animal</option>

                    ${options}

                </select>

                <button class="button primary" id="loadHistoryBtn">
                    Consultar
                </button>

                ${state.animalHistoricoId
        ? `<button class="button secondary" data-new="history">＋ Novo registro</button>`
        : ""
    }

            </div>

        </section>


        <section class="panel table-panel history-table">

            <div class="panel-heading">

                <div>
                    <span class="eyebrow">REGISTROS</span>

                    <h3>
                        ${state.animalHistoricoId
        ? `Histórico de ${escapeHtml(animalName(Number(state.animalHistoricoId)))}`
        : "Registros clínicos"
    }
                    </h3>
                </div>

                <span class="result-count">
                    ${state.historicos.length} registro(s)
                </span>

            </div>


            <div class="table-wrap">

                <table>

                    <thead>
                        <tr>
                            <th>DATA</th>
                            <th>TIPO</th>
                            <th>DESCRIÇÃO</th>
                            <th>CONSULTA</th>
                        </tr>
                    </thead>

                    <tbody>

                        ${rows || `

                            <tr>
                                <td colspan="4" class="empty-cell">

                                    ${state.animalHistoricoId
        ? "Nenhum registro para este animal."
        : "Selecione um animal para consultar o histórico."
    }

                                </td>
                            </tr>

                        `}

                    </tbody>

                </table>

            </div>

        </section>

    `;
}


// ATUALIZAR A TELA

function render() {

    if (!content) return;

    const views = {
        dashboard: renderDashboard,
        tutores: renderTutores,
        animais: renderAnimais,
        consultas: renderConsultas,
        historico: renderHistorico
    };

    content.innerHTML = (views[state.view] || renderDashboard)();
}


// CAMPOS DOS FORMULÁRIOS

function fieldsFor(entity, item = {}) {

    if (entity === "tutor") {

        return [
            {
                name: "nome",
                label: "Nome completo",
                required: true,
                value: item.nome || "",
                placeholder: "Ex.: Mariana Oliveira"
            },
            {
                name: "email",
                label: "E-mail",
                type: "email",
                required: true,
                value: item.email || "",
                placeholder: "nome@email.com"
            },
            {
                name: "telefone",
                label: "Telefone",
                required: true,
                value: item.telefone || "",
                placeholder: "(00) 00000-0000"
            }
        ];

    }


    if (entity === "animal") {

        return [
            {
                name: "nome",
                label: "Nome do animal",
                required: true,
                value: item.nome || "",
                placeholder: "Ex.: Mel"
            },
            {
                name: "especie",
                label: "Espécie",
                type: "select",
                required: true,
                value: item.especie || "Cachorro",
                options: ["Cachorro", "Gato", "Ave", "Roedor", "Outro"]
            },
            {
                name: "raca",
                label: "Raça",
                required: true,
                value: item.raca || "",
                placeholder: "Ex.: Golden Retriever"
            },
            {
                name: "sexo",
                label: "Sexo",
                type: "select",
                required: true,
                value: item.sexo || "Macho",
                options: ["Macho", "Fêmea"]
            },
            {
                name: "dataNascimento",
                label: "Data de nascimento",
                type: "date",
                required: true,
                value: item.dataNascimento || ""
            },
            {
                name: "tutorId",
                label: "Tutor responsável",
                type: "select",
                required: true,
                value: item.tutorId || "",
                options: state.tutores.map(t => ({
                    value: t.id,
                    label: `${t.nome} (#${t.id})`
                }))
            }
        ];

    }


    if (entity === "consulta") {

        return [
            {
                name: "animalId",
                label: "Animal",
                type: "select",
                required: true,
                value: item.animalId || "",
                options: state.animais.map(a => ({
                    value: a.id,
                    label: `${a.nome} (#${a.id})`
                }))
            },
            {
                name: "data",
                label: "Data da consulta",
                type: "date",
                required: true,
                value: item.data || ""
            },
            {
                name: "horario",
                label: "Horário",
                type: "time",
                required: true,
                value: formatTime(item.horario) === "—"
                    ? ""
                    : formatTime(item.horario)
            },
            {
                name: "motivo",
                label: "Motivo",
                type: "textarea",
                required: true,
                value: item.motivo || "",
                placeholder: "Descreva o motivo do atendimento",
                wide: true
            }
        ];

    }


    if (entity === "history") {

        return [
            {
                name: "tipoRegistro",
                label: "Tipo de registro",
                type: "select",
                required: true,
                value: "Vacinação",
                options: [
                    "Vacinação",
                    "Exame",
                    "Observação",
                    "Alergia",
                    "Tratamento",
                    "Outro"
                ]
            },
            {
                name: "descricao",
                label: "Descrição clínica",
                type: "textarea",
                required: true,
                value: "",
                placeholder: "Descreva as informações do registro",
                wide: true
            }
        ];

    }


    if (entity === "finalize") {

        return [
            {
                name: "diagnostico",
                label: "Diagnóstico",
                type: "textarea",
                required: true,
                value: "",
                placeholder: "Informe o diagnóstico",
                wide: true
            },
            {
                name: "tratamento",
                label: "Tratamento / orientação",
                type: "textarea",
                required: true,
                value: "",
                placeholder: "Informe o tratamento ou orientação",
                wide: true
            }
        ];

    }

    return [];
}


// MONTAR OS CAMPOS NA TELA

function renderField(field) {

    const id = `field-${field.name}`;

    let control = "";

    if (field.type === "select") {

        const options = (field.options || []).map(option => {

            const value = typeof option === "object"
                ? option.value
                : option;

            const label = typeof option === "object"
                ? option.label
                : option;

            return `
                <option
                    value="${escapeHtml(value)}"
                    ${String(value) === String(field.value) ? "selected" : ""}>

                    ${escapeHtml(label)}

                </option>
            `;

        }).join("");


        control = `
            <select
                id="${id}"
                name="${field.name}"
                ${field.required ? "required" : ""}>

                <option value="">Selecione</option>

                ${options}

            </select>
        `;

    } else if (field.type === "textarea") {

        control = `
            <textarea
                id="${id}"
                name="${field.name}"
                rows="3"
                placeholder="${escapeHtml(field.placeholder || "")}"
                ${field.required ? "required" : ""}>${escapeHtml(field.value)}</textarea>
        `;

    } else {

        control = `
            <input
                id="${id}"
                name="${field.name}"
                type="${field.type || "text"}"
                value="${escapeHtml(field.value)}"
                placeholder="${escapeHtml(field.placeholder || "")}"
                ${field.required ? "required" : ""}>
        `;

    }

    return `
        <label class="form-field ${field.wide ? "wide" : ""}" for="${id}">

            <span>
                ${field.label}
                ${field.required ? " <em>*</em>" : ""}
            </span>

            ${control}

        </label>
    `;
}


// ABRIR FORMULÁRIOS

function openModal(entity, item = null, extraId = null) {

    const editing = Boolean(item && entity !== "finalize");

    const titles = {
        tutor: editing ? "Editar tutor" : "Novo tutor",
        animal: editing ? "Editar animal" : "Novo animal",
        consulta: editing ? "Editar consulta" : "Agendar consulta",
        history: "Adicionar ao histórico",
        finalize: "Finalizar consulta"
    };

    state.modal = {
        entity,
        id: item?.id || null,
        extraId
    };

    $("modalTitle").textContent = titles[entity] || "Cadastro";

    $("modalSubtitle").textContent = entity === "finalize"
        ? `Consulta #${item?.id || ""} · O registro será enviado ao histórico do animal.`
        : "Preencha as informações abaixo.";

    $("modalFields").innerHTML =
        fieldsFor(entity, item || {})
            .map(renderField)
            .join("");

    $("modalBackdrop").classList.remove("hidden");

    document.body.classList.add("modal-open");

    const first = $("modalFields").querySelector("input, select, textarea");

    if (first) first.focus();
}


function closeModal() {

    $("modalBackdrop").classList.add("hidden");

    document.body.classList.remove("modal-open");

    $("recordForm").reset();

    state.modal = null;
}


// SALVAR FORMULÁRIOS

async function submitModal(event) {

    event.preventDefault();

    if (!state.modal) return;

    const formData = new FormData(event.currentTarget);

    const data = Object.fromEntries(formData.entries());

    const { entity, id, extraId } = state.modal;

    try {

        if (entity === "tutor") {

            await api(id ? `/tutores/${id}` : "/tutores", {
                method: id ? "PUT" : "POST",
                body: JSON.stringify(data)
            });

        }

        else if (entity === "animal") {

            data.tutorId = Number(data.tutorId);

            await api(id ? `/animais/${id}` : "/animais", {
                method: id ? "PUT" : "POST",
                body: JSON.stringify(data)
            });

        }

        else if (entity === "consulta") {

            data.animalId = Number(data.animalId);

            // TimeOnly recebe HH:mm:ss

            if (data.horario && data.horario.length === 5) {
                data.horario += ":00";
            }

            await api(id ? `/consultas/${id}` : "/consultas/agendar", {
                method: id ? "PUT" : "POST",
                body: JSON.stringify(data)
            });

        }

        else if (entity === "history") {

            await api(`/animais/${extraId}/historico`, {
                method: "POST",
                body: JSON.stringify(data)
            });

            state.animalHistoricoId = Number(extraId);

        }

        else if (entity === "finalize") {

            await api(`/consultas/${id}/finalizar`, {
                method: "POST",
                body: JSON.stringify(data)
            });

        }


        closeModal();

        await refreshData();

        if (entity === "history" || entity === "finalize") {

            if (entity === "finalize") {
                state.animalHistoricoId = extraId;
            }

            if (state.animalHistoricoId) {

                state.historicos = await api(
                    `/animais/${state.animalHistoricoId}/historico`
                ) || [];

                setView("historico");
            }
        }

        toast("Operação realizada com sucesso!");

    } catch (error) {

        toast(error.message, "error");

    }
}


// CONSULTAR HISTÓRICO

async function loadHistory(animalId) {

    if (!animalId) {
        toast("Selecione um animal.", "error");
        return;
    }

    try {

        state.animalHistoricoId = Number(animalId);

        state.historicos = await api(
            `/animais/${animalId}/historico`
        ) || [];

        setView("historico");

    } catch (error) {

        toast(error.message, "error");

    }
}


// EXCLUIR REGISTROS

async function deleteRecord(entity, id) {

    const names = {
        tutor: "este tutor",
        animal: "este animal",
        consulta: "esta consulta"
    };

    if (!confirm(`Deseja realmente excluir ${names[entity]}?`)) {
        return;
    }

    const path = {
        tutor: "/tutores",
        animal: "/animais",
        consulta: "/consultas"
    }[entity];

    try {

        await api(`${path}/${id}`, {
            method: "DELETE"
        });

        await refreshData();

        toast("Registro excluído com sucesso.");

    } catch (error) {

        toast(error.message, "error");

    }
}


// CLIQUES NAS TELAS

content.addEventListener("click", async (event) => {

    const viewButton = event.target.closest("[data-view]");

    if (viewButton) {
        setView(viewButton.dataset.view);
        return;
    }


    const newButton = event.target.closest("[data-new]");

    if (newButton) {

        const entity = newButton.dataset.new;

        if (entity === "animal" && state.tutores.length === 0) {
            return toast("Cadastre um tutor antes de cadastrar um animal.", "error");
        }

        if (entity === "consulta" && state.animais.length === 0) {
            return toast("Cadastre um animal antes de agendar uma consulta.", "error");
        }

        if (entity === "history" && !state.animalHistoricoId) {
            return toast("Selecione um animal primeiro.", "error");
        }

        openModal(
            entity,
            null,
            entity === "history" ? state.animalHistoricoId : null
        );

        return;
    }


    const action = event.target.closest("[data-action]");

    if (!action) return;

    const id = Number(action.dataset.id);

    const type = action.dataset.action;


    if (type === "edit-tutor") {
        openModal("tutor", state.tutores.find(t => t.id === id));
    }

    if (type === "edit-animal") {
        openModal("animal", state.animais.find(a => a.id === id));
    }

    if (type === "edit-consulta") {
        openModal("consulta", state.consultas.find(c => c.id === id));
    }

    if (type === "delete-tutor") {
        deleteRecord("tutor", id);
    }

    if (type === "delete-animal") {
        deleteRecord("animal", id);
    }

    if (type === "delete-consulta") {
        deleteRecord("consulta", id);
    }

    if (type === "history-animal") {
        loadHistory(id);
    }

    if (type === "finish-consulta") {

        const consulta = state.consultas.find(c => c.id === id);

        if (consulta) {
            openModal("finalize", consulta, consulta.animalId);
        }

    }

});


// BOTÕES DA TELA DE HISTÓRICO E DE CONEXÃO

content.addEventListener("click", (event) => {

    if (event.target.id === "loadHistoryBtn") {
        loadHistory($("historyAnimalSelect")?.value);
    }

    if (event.target.id === "tryAgain") {
        refreshData(true);
    }

});


// PESQUISA NAS TABELAS

content.addEventListener("input", (event) => {

    if (!event.target.matches("[data-search]")) return;

    const query = event.target.value.trim().toLocaleLowerCase("pt-BR");

    content.querySelectorAll("tbody tr").forEach(row => {

        row.hidden = !row.textContent
            .toLocaleLowerCase("pt-BR")
            .includes(query);

    });

});


// MENU LATERAL

document.querySelectorAll("[data-view]").forEach(button => {

    if (button.closest(".sidebar")) {

        button.addEventListener("click", () => {
            setView(button.dataset.view);
        });

    }

});


// EVENTOS DOS FORMULÁRIOS

$("recordForm").addEventListener("submit", submitModal);

$("closeModal").addEventListener("click", closeModal);

$("cancelModal").addEventListener("click", closeModal);

$("modalBackdrop").addEventListener("click", (event) => {

    if (event.target === $("modalBackdrop")) {
        closeModal();
    }

});

$("refreshBtn").addEventListener("click", () => {
    refreshData(true);
});


// DATA ATUAL

$("currentDate").textContent = new Date().toLocaleDateString("pt-BR", {
    weekday: "long",
    day: "numeric",
    month: "long",
    year: "numeric"
});


// INICIAR A INTERFACE

refreshData();