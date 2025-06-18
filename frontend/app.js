// Criação da instância do Axios com base URL da API
const api = axios.create({
  baseURL: "https://localhost:7109/api",
  headers: {
    "Content-Type": "application/json"
  }
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);


// Executa apenas quando abre a homepage
window.onload = () => {
  const urlParams = new URLSearchParams(window.location.search);
  const token = urlParams.get("token");

  // Se tiver token na URL (ex: vindo do Google), salva no localStorage
  if (token) {
    localStorage.setItem("token", token);
    // Remove os parâmetros da URL (opcional, visualmente mais limpo)
    window.history.replaceState({}, document.title, "homepage.html");
  }

  const savedToken = localStorage.getItem("token");
  if (!savedToken) {
    alert("Usuário não autenticado.");
    window.location.href = "index.html";
    return;
  }

  const path = window.location.pathname;
  const pagina = path.split("/").pop();
  if (pagina === "homepage.html") {
    carregarUsersNoSelect();
    carregarClientesNoSelect();
    carregarTasksNoSelect();
  }
};

async function criarEventoNoGoogleCalendar(task) {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Usuário não autenticado.");
    return;
  }

  // Ajuste o payload conforme seu DTO do backend
  const evento = {
    title: task.title,
    description: task.description,
    start: new Date().toISOString(),  // ou outro start adequado
    end: task.dueDate || new Date().toISOString()
  };

  try {
    await axios.post("https://localhost:7109/api/Auth/google/calendar/create-event", evento, {
      headers: { Authorization: `Bearer ${token}` }
    });
    console.log("Evento criado no Google Calendar");
  } catch (error) {
    console.error("Erro ao criar evento no Google Calendar:", error);
  }
}


// Função para realizar o login
function login(email, senha) {
  axios.post("https://localhost:7109/api/Auth/login", {
    email: email,
    password: senha
  })
  .then(response => {
    console.log("Login realizado:", response.data);
    alert("Login bem-sucedido!");
    localStorage.setItem("token", response.data.token); // Armazena o token
    window.location.href = "homepage.html"; // Redireciona após login
  })
  .catch(error => {
    console.error("Erro no login:", error);
    alert("Erro no login. Verifique os dados.");
  });
}

// Função disparada pelo botão de login
function handleLogin() {
  const email = document.getElementById("email").value;
  const senha = document.getElementById("senha").value;
  login(email, senha);
}

// Função para deletar cliente
function deletarCliente(id) {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Usuário não autenticado.");
    return;
  }

  const confirmar = confirm("Tem certeza que deseja deletar este cliente?");
  if (!confirmar) return;

  axios.delete(`https://localhost:7109/api/Clientes/${id}`)
  .then(() => {
    alert("Cliente deletado com sucesso.");
    carregarClientes(); // Recarrega a lista
  })
  .catch(error => {
    console.error("Erro ao deletar cliente:", error);
    alert("Erro ao deletar cliente.");
  });
}

// Carrega clientes e preenche cards na homepage
function carregarClientes() {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Usuário não autenticado.");
    window.location.href = "index.html";
    return;
  }

  axios.get("https://localhost:7109/api/Clientes/")
  .then(response => {
    const lista = response.data;
    const container = document.getElementById("clientes");
    container.innerHTML = "";

    lista.forEach(cliente => {
      const card = `
        <div class="col-md-4">
          <div class="card p-3 mb-3">
            <h5>${cliente.name}</h5>
            <p>Email: ${cliente.email}</p>
            <p>ID: ${cliente.id}</p>
            <button class="btn btn-danger btn-sm" onclick="deletarCliente('${cliente.id}')">Deletar</button>
          </div>
        </div>
      `;
      container.innerHTML += card;
    });
  })
  .catch(error => {
    console.error("Erro ao carregar clientes:", error);
    alert("Erro ao buscar os clientes.");
  });
}

function buscarTaskSelecionada() {
  const id = document.getElementById("taskSelect").value;
  if (!id) {
    alert("Selecione uma task!");
    return;
  }
  const token = localStorage.getItem("token");
  api.get(`/Tasks/by-id/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
})
  .then(response => {
    const task = response.data;
    exibirTasksDoClienteNaTela(task);
  })
  .catch(error => {
    console.error("Erro ao buscar task:", error);
    alert("Erro ao buscar task.");
  });
}

function exibirTaskNaTela(task) {
  const container = document.getElementById("taskInfo");
  container.innerHTML = `
    <div class="col-md-4">
      <div class="card p-3 mb-3">
        <h5>${task.title}</h5>
        <p><strong>ID:</strong> ${task.id}</p>
        <p><strong>Descrição:</strong> ${task.description}</p>
        <p><strong>Status:</strong> ${task.status}</p>
        <p><strong>Due date:</strong> ${task.dueDate ? task.dueDate : 'Não informado'}</p>
        <p><strong>Assigned to:</strong> ${task.assignedTo ? task.assignedTo : 'Não informado'}</p>
        <p><strong>Assigned to client:</strong> ${task.assignedToClient ? task.assignedToClient : 'Não informado'}</p>
        <p><strong>Assigned to client:</strong> ${task.status ? task.status : 'Não informado'}</p>
        <button class="btn btn-danger btn-sm" onclick="deletarTask('${task.id}')">Deletar</button>
      </div>
    </div>
  `;
}

async function buscarTasksPorUsuarioId() {
  try {
    const id = document.getElementById("buscarTarefasUsuarioId").value;

    const response = await api.get(`/Tasks/by-assigned/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
}); 

    const tasks = response.data;
    const tarefasDiv = document.getElementById("tarefasUsuario");
    tarefasDiv.innerHTML = "<h4>Tasks do Usuário Responsável</h4>";

    if (tasks.length === 0) {
      tarefasDiv.innerHTML += "<p>Nenhuma task encontrada para este usuário.</p>";
      return;
    }

    tasks.forEach(task => {
      tarefasDiv.innerHTML += `
        <div class="card text-dark mb-2">
          <div class="card-body">
            <h5 class="card-title">${task.title}</h5>
            <p>${task.description}</p>
            <p>Status: ${task.status}</p>
            <p>Prazo: ${task.dueDate ? task.dueDate.split("T")[0] : "Não informado"}</p>

            <input type="text" id="title_u${task.id}" class="form-control mb-1" placeholder="Novo Título" value="${task.title}">
            <input type="text" id="desc_u${task.id}" class="form-control mb-1" placeholder="Nova Descrição" value="${task.description}">
            <select id="status_u${task.id}" class="form-select mb-1">
              <option ${task.status === 'pendente' ? 'selected' : ''} value="pendente">Pendente</option>
              <option ${task.status === 'em_andamento' ? 'selected' : ''} value="em_andamento">Em andamento</option>
              <option ${task.status === 'concluido' ? 'selected' : ''} value="concluido">Concluído</option>
            </select>
            <input type="date" id="due_u${task.id}" class="form-control mb-1" value="${task.dueDate ? task.dueDate.split('T')[0] : ''}">

            <button class="btn btn-warning" onclick="atualizarTaskPorUsuario(${task.id})">Atualizar Task</button>
          </div>
        </div>
      `;
    });

  } catch (error) {
    console.error("Erro ao buscar tasks por usuário responsável:", error);
    alert("Erro ao buscar tasks.");
  }
}


function deletarTask(id) {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Usuário não autenticado.");
    return;
  }

  if (!confirm("Tem certeza que deseja deletar esta task?")) return;

  api.delete(`/Tasks/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
})
  .then(() => {
    alert("Task deletada com sucesso.");
    document.getElementById("taskInfo").innerHTML = ""; // limpa o card
    carregarTasksNoSelect(); // recarrega o dropdown se quiser
  })
  .catch(error => {
    console.error("Erro ao deletar task:", error);
    alert("Erro ao deletar task.");
  });
}

function buscarTasksPorClienteId() {
  const id = document.getElementById("assignedToClientBusca").value;
  if (!id) {
    alert("Selecione um cliente!");
    return;
  }

  const token = localStorage.getItem("token");
  api.get(`/Tasks/by-client/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
})
  .then(response => {
    exibirTasksDoClienteNaTela(response.data);
  })
  .catch(error => {
    console.error("Erro ao buscar tasks por cliente:", error);
    alert("Erro ao buscar tasks por cliente.");
  });
}

async function buscarTasksPorStatusEDate() {
  try {
    const status = document.getElementById("statusBusca").value;
    const dueDate = document.getElementById("dueDateBusca").value;

    if (!dueDate) {
      alert("Por favor, selecione uma data.");
      return;
    }

    const token = localStorage.getItem("token");

    const response = await api.get(`/Tasks/by-status-duedate`, {
      params: { status, dueDate }
    }, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});

    exibirTasksDoClienteNaTela(response.data);
  } catch (error) {
    console.error("Erro ao buscar tasks por status e data:", error);
    alert("Erro ao buscar tasks.");
  }
}


function exibirTasksDoClienteNaTela(tasks) {
  const container = document.getElementById("taskInfo");
  container.innerHTML = ""; // limpa antes de exibir novas

  const lista = Array.isArray(tasks) ? tasks : [tasks];

  lista.forEach(task => {
    const card = `
      <div class="col-md-4">
        <div class="card p-3 mb-3">
          <input type="text" class="form-control mb-1" id="title-${task.id}" value="${task.title}">
          <textarea class="form-control mb-1" id="desc-${task.id}">${task.description}</textarea>

          <select class="form-select mb-1" id="status-${task.id}">
            <option value="pendente" ${task.status === "pendente" ? "selected" : ""}>Pendente</option>
            <option value="em_andamento" ${task.status === "em_andamento" ? "selected" : ""}>Em Andamento</option>
            <option value="concluida" ${task.status === "concluida" ? "selected" : ""}>Concluída</option>
          </select>

          <input type="date" class="form-control mb-1" id="dueDate-${task.id}" value="${task.dueDate ? task.dueDate.substring(0,10) : ''}">

          <input type="text" class="form-control mb-1" id="assignedTo-${task.id}" value="${task.assignedTo ? task.assignedTo : ''}">

          <input type="text" class="form-control mb-1" id="assignedToClient-${task.id}" value="${task.assignedToClient ? task.assignedToClient : ''}">

          <div class="d-flex justify-content-between">
            <button class="btn btn-success btn-sm" onclick="atualizarTask('${task.id}')">Salvar</button>
            <button class="btn btn-danger btn-sm" onclick="deletarTask('${task.id}')">Deletar</button>
          </div>
        </div>
      </div>
    `;
    container.innerHTML += card;
  });
}


async function buscarTasksPorUserId() {
  try {
    const userId = document.getElementById("assignedToUserBusca").value;
    if (!userId) {
      alert("Selecione um usuário.");
      return;
    }

    const token = localStorage.getItem("token");
    const response = await api.get(`/Tasks/by-user/${userId}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});

    const tarefasDiv = document.getElementById("tarefas");
    tarefasDiv.innerHTML = ""; // Limpa antes de exibir novas

    if (response.data.length === 0) {
      tarefasDiv.innerHTML = "<p>Nenhuma tarefa encontrada para este usuário.</p>";
      return;
    }
    const data = [response.data];

    exibirTasksDoClienteNaTela(response.data);

  } catch (error) {
    console.error("Erro ao buscar tarefas do usuário:", error);
    alert("Erro ao buscar tarefas do usuário.");
  }
}


// Preenche o select com clientes para criação de tarefa
function carregarClientesNoSelect() {
  const token = localStorage.getItem("token");
  api.get("/Clientes", {
  headers: {
    Authorization: `Bearer ${token}`
  }
}).then(response => {
    const selectBusca = document.getElementById("assignedToClientBusca");
    const selectCriacao = document.getElementById("assignedToClientCriacao");

    selectBusca.innerHTML = '<option value="">-- Selecione --</option>';
    selectCriacao.innerHTML = '<option disabled selected value="">Selecione um cliente</option>';

    response.data.forEach(cliente => {
      const optionBusca = document.createElement("option");
      optionBusca.value = cliente.id;
      optionBusca.text = `${cliente.name} (${cliente.email})`;
      selectBusca.appendChild(optionBusca);

      const optionCriacao = document.createElement("option");
      optionCriacao.value = cliente.id;
      optionCriacao.text = `${cliente.name} (${cliente.email})`;
      selectCriacao.appendChild(optionCriacao);
    });
  }).catch(error => {
    console.error("Erro ao carregar clientes:", error);
    alert("Erro ao buscar clientes.");
  });
}

function carregarUsersNoSelect() {
  const token = localStorage.getItem("token");
  api.get("/Auth", {
  headers: {
    Authorization: `Bearer ${token}`
  }
} ).then(response => {
    const selectBusca = document.getElementById("assignedToUserBusca");
    const selectCriacao = document.getElementById("assignedTo");

    selectBusca.innerHTML = '<option value="">-- Selecione --</option>';
    selectCriacao.innerHTML = '<option disabled selected value="">Selecione um responsável</option>';

    response.data.forEach(user => {
      const optionBusca = document.createElement("option");
      optionBusca.value = user.id;
      optionBusca.text = `${user.name} (${user.email})`;
      selectBusca.appendChild(optionBusca);

      const optionCriacao = document.createElement("option");
      optionCriacao.value = user.id;
      optionCriacao.text = `${user.name} (${user.email})`;
      selectCriacao.appendChild(optionCriacao);
    });
  }).catch(error => {
    console.error("Erro ao carregar usuários:", error);
    alert("Erro ao buscar usuários.");
  });
}


function carregarTasksNoSelect() {
  const token = localStorage.getItem("token");
  api.get("/Tasks", {
  headers: {
    Authorization: `Bearer ${token}`
  }
})
  .then(response => {
    const select = document.getElementById("taskSelect");
    select.innerHTML = '<option value="">-- Selecione --</option>'; // limpa o select

    response.data.forEach(task => {
      const option = document.createElement("option");
      option.value = task.id;
      option.text = `${task.title} (${task.status})`;
      select.appendChild(option);
    });
  })
  .catch(error => {
    console.error("Erro ao carregar tasks:", error);
    alert("Erro ao buscar tasks.");
  });
}

// Função para buscar todos os clientes (sem token)
async function getAllClients() {
  try {
    const response = await api.get("/Clientes", {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    console.log("Clientes:", response.data);
  } catch (error) {
    console.error("Erro ao buscar clientes:", error);
  } 
}

// Função para buscar cliente por ID (sem interface ainda)
async function getClientsById() {
  try {
    const id = document.getElementById("buscarClienteId").value;
    const response = await api.get(`/Clientes/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    const client = response.data;

    const clientesDiv = document.getElementById("clientes");
    clientesDiv.innerHTML = `
      <div class="col-12 mb-3">
        <div class="card text-dark">
          <div class="card-body">
            <h5 class="card-title">${client.name}</h5>
            <p class="card-text">Email: ${client.email}</p>
            <p class="card-text">Descrição: ${client.description}</p>

            <input type="text" id="editName" class="form-control mb-2" placeholder="Novo Nome" value="${client.name}">
            <input type="email" id="editEmail" class="form-control mb-2" placeholder="Novo Email" value="${client.email}">
            <input type="text" id="editDescription" class="form-control mb-2" placeholder="Nova Descrição" value="${client.description}">
            
            <button class="btn btn-warning mb-2" onclick="atualizarClientePorId(${client.id})">Atualizar Cliente</button>
            <button class="btn btn-info mb-2" onclick="mostrarTasksCliente(${client.id})">Gerenciar Tasks</button>
          </div>
        </div>
      </div>
    `;
  } catch (error) {
    console.error("Erro ao buscar cliente:", error);
    alert("Cliente não encontrado.");
  }
}

async function mostrarTasksCliente(idCliente) {
  try {
    const response = await api.get(`/Tasks/by-user/${idCliente}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    const tasks = response.data;

    const tarefasDiv = document.getElementById("tarefas");
    tarefasDiv.innerHTML = "<h4 class='mt-3'>Tasks do Cliente</h4>";

    tasks.forEach(task => {
      tarefasDiv.innerHTML += `
        <div class="card text-dark mb-2">
          <div class="card-body">
            <h5 class="card-title">${task.title}</h5>
            <p>${task.description}</p>
            <p>Status: ${task.status}</p>
            <p>Prazo: ${task.dueDate ? task.dueDate.split("T")[0] : "Não informado"}</p>

            <input type="text" id="title_${task.id}" class="form-control mb-1" placeholder="Novo Título" value="${task.title}">
            <input type="text" id="desc_${task.id}" class="form-control mb-1" placeholder="Nova Descrição" value="${task.description}">
            <select id="status_${task.id}" class="form-select mb-1">
              <option ${task.status === 'pendente' ? 'selected' : ''} value="pendente">Pendente</option>
              <option ${task.status === 'em_andamento' ? 'selected' : ''} value="em_andamento">Em andamento</option>
              <option ${task.status === 'concluido' ? 'selected' : ''} value="concluido">Concluído</option>
            </select>
            <input type="date" id="due_${task.id}" class="form-control mb-1" value="${task.dueDate ? task.dueDate.split('T')[0] : ''}">

            <button class="btn btn-warning" onclick="atualizarTaskPorId(${task.id})">Atualizar Task</button>
          </div>
        </div>
      `;
    });
  } catch (error) {
    console.error("Erro ao carregar tasks do cliente:", error);
    alert("Erro ao carregar tasks.");
  }
}


async function atualizarClientePorId(id) {
  try {
    const name = document.getElementById("editName").value;
    const email = document.getElementById("editEmail").value;
    const description = document.getElementById("editDescription").value;

    const token = localStorage.getItem("token");

    await api.put(`/Clientes/${id}`, {
      name,
      email,
      description
    }, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});

    alert("Cliente atualizado com sucesso!");
    getClientsById(); // Recarrega os dados atualizados
  } catch (error) {
    console.error("Erro ao atualizar cliente:", error);
    alert("Erro ao atualizar cliente.");
  }
}


function exibirClienteNaTela(cliente) {
  const container = document.getElementById("clientes");
  container.innerHTML = `
    <div class="col-md-4">
      <div class="card p-3 mb-3">
        <h5>${cliente.name}</h5>
        <p><strong>Email:</strong> ${cliente.email}</p>
        <p><strong>Descrição:</strong> ${cliente.description ? cliente.description : 'Não informado'}</p>
        <p><strong>ID:</strong> ${cliente.id}</p>
      </div>
    </div>
  `;
}

async function atualizarTaskPorId(idTask) {
  try {
    const title = document.getElementById(`title_${idTask}`).value;
    const description = document.getElementById(`desc_${idTask}`).value;
    const status = document.getElementById(`status_${idTask}`).value;
    const dueDate = document.getElementById(`due_${idTask}`).value;

    const token = localStorage.getItem("token");

    await api.put(`/Tasks/${idTask}`, {
      title,
      description,
      status,
      dueDate
    }, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});

    alert("Task atualizada com sucesso!");
    await criarEventoNoGoogleCalendar(novaTask); 
    const clienteId = document.getElementById("buscarClienteId").value;
    mostrarTasksCliente(clienteId);
  } catch (error) {
    console.error("Erro ao atualizar task:", error);
    alert("Erro ao atualizar task.");
  }
}



async function getTaskById() {
  try {
    const id = document.getElementById("buscarTaskId").value;
    const response = await api.get(`/Tasks/by-id/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    console.log("Task:", response.data);
  } catch (error) {
    console.error("Erro ao buscar tarefa por ID:", error);
  } 
}

async function getTaskByUser() { //utilizar
  try {
    const id = document.getElementById("buscarTarefasClienteId").value;
    const response = await api.get(`/Tasks/by-user/${id}`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    console.log("Task:", response.data);
  } catch (error) {
    console.error("Erro ao buscar tarefas por cliente:", error);
  } 
}

async function createTask() {
  const titulo = document.getElementById("titulo").value;
  const descricao = document.getElementById("descricaoTask").value;
  const status = document.getElementById("status").value; // Alteração: pega do select
  const assignedTo = document.getElementById("assignedTo").value;
  const assignedToClient = document.getElementById("assignedToClientCriacao").value;
  const dueDate = document.getElementById("dueDate").value;
  const token = localStorage.getItem("token");

  if (!token) {
    alert("Você precisa estar logado.");
    return;
  }

  if (!titulo || !descricao) {
    alert("Preencha todos os campos!");
    return;
  }

  const novaTask = {
    title: titulo,
    description: descricao,
    status: status,
    assignedTo: assignedTo,
    assignedToClient: assignedToClient,
    dueDate: dueDate
  };

  try {
    const response = await api.post("/Tasks", novaTask, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    console.log(response.data);
    alert("Tarefa criada com sucesso!");
    await criarEventoNoGoogleCalendar(novaTask); 
    document.getElementById("titulo").value = "";
    document.getElementById("descricaoTask").value = "";
    document.getElementById("assignedTo").value = "";
    document.getElementById("status").value = "";
    document.getElementById("assignedToClientCriacao").value = "";
    document.getElementById("dueDate").value = "";
  } catch (error) {
    console.error("Erro ao criar tarefa:", error);
    alert("Erro ao criar a tarefa.");
  }
}

async function createClient() {
  const nome = document.getElementById("nome").value;
  const descricao = document.getElementById("descricaoCliente").value;
  const email = document.getElementById("email").value;

  const token = localStorage.getItem("token");

  if (!token) {
    alert("Você precisa estar logado.");
    return;
  }

  if (!nome || !descricao || !email) {
    alert("Preencha todos os campos!");
    return;
  }

  const novoCliente = {
    name: nome,
    description: descricao,
    email: email,
  };

  try {
    const response = await api.post("/Clientes", novoCliente, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
    console.log(response.data);
    alert("Tarefa criada com sucesso!");
    document.getElementById("nome").value = "";
    document.getElementById("descricaoCliente").value = "";
    document.getElementById("email").value = "";
  } catch (error) {
    console.error("Erro ao criar tarefa:", error);
    alert("Erro ao criar a tarefa.");
  }
}



// Tornando funções globais para uso no HTML
window.handleLogin = handleLogin;
window.login = login;
window.getAllClients = getAllClients;
window.getClientsById = getClientsById;
window.carregarClientes = carregarClientes;
window.deletarCliente = deletarCliente;
window.createTask = createTask;
window.buscarTasksPorClienteId = buscarTasksPorClienteId;