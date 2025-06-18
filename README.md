#  ProductClientHub

Sistema completo de gerenciamento de **clientes** e **tarefas**, com autenticação via **login tradicional (e-mail/senha)** e **login com conta Google**, integração com o **Google Calendar** e interface frontend simples em HTML + JavaScript.

---

##  Funcionalidades

- ✅ Cadastro e login de administradores
- ✅ Autenticação JWT protegendo as rotas
- ✅ Login com conta Google (OAuth2)
- ✅ Criação e edição de clientes
- ✅ Criação, atualização e atribuição de tarefas para clientes e usuários
- ✅ Integração com **Google Calendar**
- ✅ Frontend 100% separado do backend, consumindo via API

---

##  Tecnologias utilizadas

### Backend (.NET 8 + PostgreSQL)

- ASP.NET Core 8
- JWT Authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Google Authentication (`Microsoft.AspNetCore.Authentication.Google`)
- Google Calendar API (`Google.Apis.Calendar.v3`)
- PostgreSQL + Dapper
- Swagger

### Frontend

- HTML5 + JavaScript (puro)
- Axios para requisições HTTP

---

## 🗃️ Estrutura do Projeto

```
ProductClientHub
│
├── ProductClientHub.API                # Projeto principal da API
├── ProductClientHub.Communication     # DTOs de Request/Response
├── ProductClientHub.Exceptions        # Gerenciador de erros personalizados
├── database.sql                       # Script de criação das tabelas PostgreSQL
└── frontend/
    ├── index.html                     # Tela de login
    ├── homepage.html                  # Tela principal com dashboard
    ├── app.js                         # Arquivo JavaScript com funções principais
```

---

## 🚀 Como rodar na sua máquina

### 🧱 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Visual Studio ou VS Code](https://visualstudio.microsoft.com/)
- [Google Cloud Console](https://console.cloud.google.com/) com OAuth2 configurado

---

### 🔌 Passo a passo

#### 1. Clone o repositório

```bash
git clone https://github.com/NicoMasss/TEST-CSHARP.git
```

#### 2. Configure o banco de dados

- Crie um banco PostgreSQL chamado `Project_Engenharia`
- Execute o script `database.sql` com as tabelas:

```sql
-- admin_users, clients, tasks
```

- Verifique se a string de conexão está correta no `appsettings.json`:

```json
"ConnectionStrings": {
  "Default": "Server=localhost;Port=5432;Database=Project_Engenharia;User Id=postgres;Password=123"
}
```

#### 3. Configure as credenciais do Google

- Crie um projeto no [Google Cloud Console](https://console.cloud.google.com/)
- Ative a **Google Calendar API**
- Vá até **APIs e serviços > Tela de consentimento OAuth**
- Vá até **Credenciais > Criar credenciais > OAuth Client ID**
- Adicione o `Redirect URI`:

```bash
https://localhost:7109/api/Auth/google/callback
```

- Copie o `ClientId` e `ClientSecret` e substitua no backend dentro do appsettings.json.

#### 4. Rode a API

```bash
cd ProductClientHub.API
dotnet restore
dotnet run
```

> A API estará rodando em `https://localhost:7109/swagger/index.html`

#### 5. Rode o frontend

- Abra o `frontend/index.html` com o Live Server do VS Code, ou use Python:

```bash
cd frontend
```

- Acesse no navegador:

```bash
http://localhost:5500
```

---

##  Autenticação

### Login normal

- Registre um admin  pelo Swagger
- Faça login via `/api/Auth/login` e salve o token no localStorage se for pelo swagger

### Login com Google

- Clique no botão "Login com Google" no `index.html`
- Será redirecionado com o token JWT via URL (capturado e salvo pelo `app.js`)

---

##  Integração com Google Calendar

- Toda vez que uma **task** é atribuída a um **usuário**, é possível usar a rota: **so funcional se tiver permissão de teste que tem que ser requisitada para o administrador. somente possivel criar tarefas no google agenda com emails de teste.

```http
POST /api/Auth/google/calendar/create-event
```

- Payload de exemplo:

```json
{
  "title": "Reunião",
  "description": "Discussão de tarefas",
  "start": "2025-06-18T15:00:00",
  "end": "2025-06-18T16:00:00"
}
```

---

##  Observações

- Os tokens JWT expiram em 1 hora.
- Os tokens de acesso do Google são salvos em memória (pode ser adaptado para banco de dados).
- Frontend pode ser facilmente migrado para React, Angular ou outro framework futuramente.

---

## 🧑 Autor

Desenvolvido por **Nicolas Massochin**, **Guilherme Menna** e **Artur Costa**.
