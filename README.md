# Inventory Management System

Este projeto é um sistema de controle de estoque construído com .NET 8, utilizando MediatR para implementar CQRS, Dapper para acesso ao banco de dados e Entity Framework Core para mapeamento objeto-relacional. Também inclui testes unitários utilizando TDD.

### Configuração Inicial
  Clone o repositório do projeto:

    git clone https://github.com/seu-usuario/inventory-management.git
    cd inventory-management

Restaure as dependências do projeto:

    dotnet restore

### Configuração do Banco de Dados
Este projeto utiliza MySQL como banco de dados. A configuração do banco de dados é especificada no arquivo appsettings.json.

    {
    "ConnectionStrings": {
      "DefaultConnection": "server=db;database=inventory;user=root;password=root"
    }
    }
    
Usando Docker para criar o banco de dados, caso seja preciso.

    docker-compose up --build

### Usando .NET CLI
  Certifique-se de que o MySQL está rodando e a conexão está configurada corretamente no appsettings.json.

    dotnet ef database update

  Execute a aplicação:

    dotnet run

  Acesse a aplicação em seu navegador em http://localhost:5000. O Swagger estará disponível em http://localhost:5000/swagger.
  

## Testes

Para executar os testes unitários, utilize o comando:

    dotnet test
    

## Documentação da API

  A documentação da API está disponível via Swagger.

### Rotas da API

#### Produtos

GET /api/product/{id}: Obtém um produto pelo ID.
GET /api/product: Obtém todos os produtos.
POST /api/product: Cria um novo produto.

    {
      "partNumber": "string",
      "name": "string",
      "costPrice": 0,
      "stockQuantity": 0
    }
    
PUT /api/product/{id}: Atualiza um produto existente.

        {
          "id": 0,
          "partNumber": "string",
          "name": "string",
          "costPrice": 0,
          "stockQuantity": 0
        }

DELETE /api/product/{id}: Exclui um produto pelo ID.

#### Consumo de Produtos:

POST /api/product/consume: Consome uma quantidade de um produto.

        {
          "productId": 0,
          "quantity": 0
        }

#### Logs de Consumo Diário

GET /api/product/consumption: Obtém os logs de consumo diário.

  Parâmetros de consulta:
            day: Dia do consumo.
            month: Mês do consumo.
            year: Ano do consumo.
 exemplo: 
 
     GET /api/product/consumption?day=9&month=7&year=2024
