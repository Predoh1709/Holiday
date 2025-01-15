# Holiday

Este repositório contém dois projetos principais:

1. **HolidayAPI**: Uma API desenvolvida em ASP.NET Core.
2. **holiday-app**: Uma aplicação web frontend desenvolvida em Angular.

## HolidayAPI - Configuração Inicial

1. **Configurar a string de conexão**

   Abra o arquivo `appsettings.json` e configure a seção `ConnectionStrings` com os detalhes do seu banco de dados SQL Server:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data source=DESKTOP-D1D6V76\\SQLEXPRESS;database=Holiday;Trusted_connection=true;Encrypt=false;TrustServerCertificate=true"
   }
   ```

2. **Rodar as migrações**

   No terminal, execute os seguintes comandos para aplicar as migrações e configurar o banco de dados:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

## Endpoints

### 1. Buscar e salvar feriados nacionais
**GET /api/Holiday/dadosbr-feriados-nacionais**

Busca feriados nacionais de uma fonte externa e os salva no banco de dados.

- **Resposta de Sucesso:**
  - **Status:** 200 OK
  - **Body:** Lista de feriados no formato JSON

- **Resposta de Erro:**
  - **Status:** 500 Internal Server Error

---

### 2. Listar todos os feriados
**GET /api/Holiday**

Retorna todos os feriados armazenados no banco de dados.

- **Resposta de Sucesso:**
  - **Status:** 200 OK
  - **Body:** Lista de feriados com suas respectivas datas variáveis.

---

### 3. Buscar feriado por ID
**GET /api/Holiday/{id}**

Retorna os detalhes de um feriado específico pelo seu ID.

- **Parâmetros:**
  - `id` (int): ID do feriado.

- **Resposta de Sucesso:**
  - **Status:** 200 OK
  - **Body:** Dados do feriado.

- **Resposta de Erro:**
  - **Status:** 404 Not Found (caso o feriado não seja encontrado).

---

### 4. Atualizar a descrição de um feriado
**PUT /api/Holiday/{id}/description**

Atualiza a descrição de um feriado pelo seu ID.

- **Parâmetros:**
  - `id` (int): ID do feriado.
  - **Body:** (string): Nova descrição.

- **Resposta de Sucesso:**
  - **Status:** 200 OK
  - **Body:** Dados do feriado atualizado.

- **Resposta de Erro:**
  - **Status:** 400 Bad Request (caso a descrição seja nula ou vazia).
  - **Status:** 404 Not Found (caso o feriado não seja encontrado).

---

### 5. Excluir feriado
**DELETE /api/Holiday/{id}**

Remove um feriado do banco de dados pelo seu ID.

- **Parâmetros:**
  - `id` (int): ID do feriado.

- **Resposta de Sucesso:**
  - **Status:** 204 No Content

- **Resposta de Erro:**
  - **Status:** 404 Not Found (caso o feriado não seja encontrado).

---

![api](https://github.com/user-attachments/assets/6b93d260-055a-445d-80ed-a00bdd05d312)

## holiday-app

A **holiday-app** é a interface de usuário desenvolvida com **Angular 16**. Ela consome os endpoints fornecidos pela HolidayAPI.

### Configuração e Execução
1. Certifique-se de ter o **Node.js** e o **Angular CLI** instalados.
2. Navegue até a pasta `holiday-app`:
   ```bash
   cd holiday-app
   ```
3. Instale as dependências:
   ```bash
   npm install
   ```
4. Inicie o servidor de desenvolvimento:
   ```bash
   ng serve
   ```
5. A aplicação estará disponível em `http://localhost:4200`.

![front](https://github.com/user-attachments/assets/311de5ed-4b20-40b4-870e-7bd34a53b4f4)
