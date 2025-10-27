# 123Vendas - Domain Sales

Sistema de gestão de vendas desenvolvido com .NET 8.0 seguindo os princípios de Domain-Driven Design (DDD) e Arquitetura Hexagonal (Ports & Adapters).

## Arquitetura

O projeto segue Arquitetura Hexagonal com DDD, separando claramente as responsabilidades:

- **Domain**: Regras de negócio puras, entidades, value objects, eventos de domínio
- **Application**: Casos de uso, comandos/queries (CQRS), handlers, DTOs
- **Api**: Controllers REST, middlewares, validações
- **Database**: Implementação de repositórios, contexto EF Core

### Principais Padrões Aplicados:

- Domain Events para comunicação assíncrona
- Repository Pattern para acesso a dados
- Pipeline Behaviors para validação e logging
- Value Objects

## Tecnologias Utilizadas

- .NET 8.0
- ASP.NET Core Web API
- MediatR - Mediator pattern para CQRS
- FluentValidation - Validação declarativa
- Entity Framework Core InMemory - Banco de dados em memória
- Serilog - Logging estruturado
- Elasticsearch + Kibana - Armazenamento e visualização de logs
- AutoMapper - Mapeamento objeto-para-objeto
- xUnit + Shouldly + Bogus + NSubstitute - Testes unitários

## Como Executar o Projeto

### 1. Clonar o Repositório

```bash
git clone <url-do-repositorio>
cd 123Vendas_Domain_Sales
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Iniciar Elasticsearch e Kibana (Opcional)

Se quiser ver os logs no Kibana:

```bash
docker-compose up -d
```

Aguarde alguns segundos e verifique se subiu:

```bash
docker ps
```

### 4. Executar a API

```bash
dotnet run --project src/123Vendas.Domain.Sales.Api
```

A API vai estar disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `https://localhost:5001/swagger`

### 5. Testar a API

Acesse o Swagger em `https://localhost:5001/swagger`:

#### Criar uma venda:
```http
POST https://localhost:5001/api/sales
Content-Type: application/json

{
  "saleNumber": "VENDA-001",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "Charly RODRIGIUES",
  "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
  "branchName": "Filial 1",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "productName": "Produto A",
      "unitPrice": 100.00,
      "quantity": 5
    }
  ]
}
```

#### Listar todas as vendas:
```http
GET https://localhost:5001/api/sales
```

#### Buscar venda por ID:
```http
GET https://localhost:5001/api/sales/{id}
```

#### Cancelar uma venda:
```http
DELETE https://localhost:5001/api/sales/{id}
```

## Como Executar os Testes

Pra rodar todos os testes:

```bash
dotnet test
```

Se quiser ver mais detalhes:

```bash
dotnet test --logger "console;verbosity=detailed"
```

Se quiser ver cobertura de código:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Tecnologias de Teste

- xUnit: Framework de testes
- Shouldly: Assertions fluentes e legíveis
- Bogus: Geração de dados fake realistas
- NSubstitute: Mocking de dependências

## Como Visualizar os Logs

### Logs no Console

Quando você roda a API, os logs aparecem no console assim:

```
[10:30:45 INF] Iniciando execução do comando/query: CreateSaleCommand
[10:30:45 INF] Entidades de domínio criadas: SaleId=xxx, Items=3, TotalValue=450.00, Duration=2ms
[10:30:45 INF] Venda persistida no banco: SaleId=xxx, Duration=15ms
[10:30:45 INF] Eventos de domínio publicados: SaleId=xxx, EventsCount=1, Duration=5ms
[10:30:45 INF] Venda criada com sucesso: SaleId=xxx, TotalDuration=22ms
[10:30:45 INF] Comando/Query executado com sucesso: CreateSaleCommand em 23ms
```


## Endpoints da API

### POST `/api/sales` - Criar Venda
Cria uma nova venda com validação automática.

Validações:
- Número da venda é obrigatório (máx. 50 caracteres)
- Customer e Branch são obrigatórios
- Mínimo de 1 item por venda
- Quantidade máxima de 20 itens iguais por produto
- Descontos automáticos:
  - 4-9 itens: 10% de desconto
  - 10+ itens: 20% de desconto

Resposta: `201 Created` com ID da venda criada

---

### GET `/api/sales` - Listar Vendas
Retorna todas as vendas ordenadas por data (mais recentes primeiro).

Resposta: `200 OK` com lista de vendas

---

### GET `/api/sales/{id}` - Buscar Venda por ID
Retorna detalhes completos de uma venda incluindo todos os itens.

Resposta:
- `200 OK` com detalhes da venda
- `404 Not Found` se venda não existe

---

### DELETE `/api/sales/{id}` - Cancelar Venda
Cancela uma venda existente (mudança de status).

Resposta:
- `200 OK` com confirmação de cancelamento
- `404 Not Found` se venda não existe

## Comentários no Código

Coloquei alguns comentários no código explicando o motivo de ter tomado algumas decisões. Exemplos:

### `SaleRepository.cs`
```csharp
//Isso poderia vir de um Unit of work, achei mais simples fazer isso,
//mas um projeto da vida real, eu provavelmente iria para algo como Unit of work!
public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
{
    await context.SaveChangesAsync(cancellationToken);
}
```

### `Sale.cs`
```csharp
//Poderiamos ter um evento para item cancelado também.
public void CancelItem(Guid productId)
{
    var item = _items.FirstOrDefault(i => i.ProductId == productId);
    if (item == null)
        throw new DomainException("Item não encontrado para essa venda.");
}
```

### `Program.cs`
```csharp
// Register Repositories - Isso pode vir de uma classe especifica de DI,
// eu deixei assim para adiantar o teste.
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
```

Esses comentários explicam decisões de arquitetura que tomei por simplicidade, e possíveis melhorias que poderiam ser aplicadas num projeto real de produção.

Desenvolvido com .NET 8.0 e boas práticas de DDD/Clean Architecture
