# Escupe

Sistema ASP.NET Core MVC para gestão de vagas, empresas e candidatos.

## Visão Geral
- Plataforma para cadastro de empresas, candidatos e gerenciamento de vagas de emprego.
- Estrutura modular: Controllers, Services, DTOs, Models, ViewModels e Views.
- Frontend com Razor Views, CSS customizado e scripts JS para interatividade.

## Estrutura do Projeto
- **Controllers**: Lógica de requisições HTTP (ex: `FeedController`, `UsuariosController`).
- **Services**: Lógica de negócio e integrações externas (ex: `CEPService`, `CNPJService`).
- **DTOs**: Objetos de transferência de dados para entrada/saída.
- **ViewModels**: Dados preparados para exibição nas views.
- **Views**: Razor Views organizadas por contexto.
- **wwwroot**: Arquivos estáticos (CSS, JS, imagens, uploads).

## Principais Funcionalidades
- Cadastro e autenticação de usuários (empresas e candidatos).
- Criação, edição e listagem de vagas.
- Candidatura a vagas e upload de currículos.
- Consulta de CEP, CNPJ e CPF via APIs externas.

## Como rodar o projeto
1. **Pré-requisitos:** .NET 6+ instalado.
2. **Build:**
   ```bash
   dotnet build
   ```
3. **Rodar:**
   ```bash
   dotnet run
   ```
4. Acesse via navegador: `https://localhost:5001` (ou porta configurada).

## Desenvolvimento
- Debug configurado em `Properties/launchSettings.json`.
- Dependências via NuGet no `.csproj`.
- Swagger disponível em dev para testar APIs.

## Exemplos de arquivos importantes
- `escupe/Program.cs`: Configuração principal.
- `escupe/Controllers/FeedController.cs`: Lógica de vagas.
- `escupe/Views/Feed/FeedVaga.cshtml`: Exibição dinâmica de vagas.
- `escupe/wwwroot/js/vagas.js`: Scripts para interação de vagas.

## Observações
- Uploads de arquivos são salvos em subpastas de `wwwroot/`.
- Customizações de layout em `Views/Shared/_Layout.cshtml` e CSS dedicado.
- Scripts front-end para etapas de cadastro e exibição dinâmica.

---

Contribuições e sugestões são bem-vindas!
