// Criação da instância do Axios com base URL da API
const api = axios.create({
  baseURL: "https://localhost:7109/api",
  headers: {
    "Content-Type": "application/json"
  }
});

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

function deletarCliente(id) {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Usuário não autenticado.");
    return;
  }

  const confirmar = confirm("Tem certeza que deseja deletar este cliente?");
  if (!confirmar) return;

  axios.delete(`https://localhost:7109/api/Clientes/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  })
  .then(() => {
    alert("Cliente deletado com sucesso.");
    carregarClientes(); // Recarrega a lista
  })
  .catch(error => {
    console.error("Erro ao deletar cliente:", error);
    alert("Erro ao deletar cliente.");
  });
}


// Função para obter todos os clientes (com token de autenticação)
function carregarClientes() {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Usuário não autenticado.");
    window.location.href = "login.html";
    return;
  }

  axios.get("https://localhost:7109/api/Clientes/", {
    headers: {
      Authorization: `Bearer ${token}`
    }
  })
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


// Função para buscar todos os clientes (sem token)
async function getAllClients() {
  try {
    const response = await api.get("/Clientes");
    console.log("Clientes:", response.data);
  } catch (error) {
    console.error("Erro ao buscar clientes:", error);
  } 
}

// Função para buscar cliente por ID (exemplo futuro)
async function getClientsById(id) {
  try {
    const response = await api.get(`/Clientes/${id}`);
    console.log("Cliente:", response.data);
  } catch (error) {
    console.error("Erro ao buscar cliente por ID:", error);
  } 
}





// Tornando funções globais para uso no HTML
window.handleLogin = handleLogin;
window.login = login;
window.getAllClients = getAllClients;
window.getClientsById = getClientsById;
window.carregarClientes = carregarClientes;
window.deletarCliente = deletarCliente;
