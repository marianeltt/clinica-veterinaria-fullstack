# API da Clínica Veterinária

API REST desenvolvida com ASP.NET Core 10, C#, Entity Framework Core, PostgreSQL e Swagger/OpenAPI.

## Requisitos

- .NET SDK 10.0
- PostgreSQL
- JetBrains Rider ou Visual Studio
- Ferramenta `dotnet-ef` 10.0.15 para criar e aplicar migrations

## Estrutura

```text
ApiClinicaVeterinaria/
├── Controllers/
│   ├── AnimaisController.cs
│   ├── ConsultasController.cs
│   └── TutoresController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
├── Models/
├── Properties/
│   └── launchSettings.json
├── scripts/
│   └── criar_banco.sql
├── Program.cs
├── appsettings.json
└── ApiClinicaVeterinaria.csproj
```

## 1. Criar o banco PostgreSQL

No pgAdmin, crie um banco com o nome:

```sql
CREATE DATABASE clinica_veterinaria;
```

Atualize o arquivo `appsettings.Development.json` na raiz do projeto com a conexão local. Altere usuário e senha conforme sua instalação do PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=clinica_veterinaria;Username=postgres;Password=SUA_SENHA"
  }
}
```

## 2. Restaurar os pacotes

Abra o terminal na pasta que contém `ApiClinicaVeterinaria.csproj` e execute:

```bash
dotnet restore
```

## 3. Criar e aplicar a migration

Instale a ferramenta do Entity Framework Core:

```bash
dotnet tool install --global dotnet-ef --version 10.0.15
```

Se ela já estiver instalada, atualize:

```bash
dotnet tool update --global dotnet-ef --version 10.0.15
```

Crie a migration inicial e aplique-a ao PostgreSQL:

```bash
dotnet ef migrations add CriacaoInicial
dotnet ef database update
```

A migration criará as tabelas `Tutores`, `Animais`, `Consultas` e `Historicos`, os relacionamentos e os dados iniciais configurados no `AppDbContext`.

A pasta `Migrations` será criada pelo comando. Mantenha essa pasta no projeto e inclua-a na entrega.

## Alternativa: script SQL

Em vez das migrations, é possível executar `scripts/criar_banco.sql` no banco `clinica_veterinaria` pelo pgAdmin. O script cria as tabelas e insere dados de exemplo.

**Use as migrations ou o script SQL, não os dois no mesmo banco vazio.** Se utilizar migrations, não execute o script SQL.

## 4. Executar a API

No Rider, selecione o perfil `http` e clique em Run. Pelo terminal:

```bash
dotnet run --launch-profile http
```

Abra o Swagger no navegador:

```text
http://localhost:5090/swagger
```

## Endpoints

### Tutores

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/tutores` | Lista tutores |
| GET | `/api/tutores/{id}` | Busca tutor por ID |
| POST | `/api/tutores` | Cadastra tutor |
| PUT | `/api/tutores/{id}` | Atualiza tutor |
| DELETE | `/api/tutores/{id}` | Exclui tutor sem animais vinculados |

### Animais e histórico

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/animais` | Lista animais; aceita `tutorId` e `especie` como filtros |
| GET | `/api/animais/{id}` | Busca animal por ID |
| POST | `/api/animais` | Cadastra animal vinculado a um tutor |
| PUT | `/api/animais/{id}` | Atualiza animal |
| DELETE | `/api/animais/{id}` | Exclui animal sem consultas ou histórico |
| GET | `/api/animais/{id}/historico` | Consulta histórico clínico |
| POST | `/api/animais/{id}/historico` | Adiciona anotação clínica |

### Consultas

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/consultas` | Lista consultas; aceita `animalId` e `status` como filtros |
| GET | `/api/consultas/{id}` | Busca consulta por ID |
| POST | `/api/consultas/agendar` | Agenda consulta e verifica conflitos |
| PUT | `/api/consultas/{id}` | Atualiza consulta ainda agendada |
| DELETE | `/api/consultas/{id}` | Exclui consulta não finalizada |
| POST | `/api/consultas/{id}/finalizar` | Finaliza consulta e cria registro no histórico |

## Exemplos de JSON para testar no Swagger

### POST `/api/tutores`

```json
{
  "nome": "Mariana Oliveira",
  "email": "mariana@email.com",
  "telefone": "(17) 99999-3333"
}
```

### POST `/api/animais`

```json
{
  "nome": "Bidu",
  "especie": "Cachorro",
  "raca": "Poodle",
  "sexo": "Macho",
  "dataNascimento": "2022-03-15",
  "tutorId": 1
}
```

### POST `/api/consultas/agendar`

Use uma data futura e um horário posterior ao momento do teste:

```json
{
  "animalId": 1,
  "data": "2026-12-18",
  "horario": "14:00:00",
  "motivo": "Consulta de rotina"
}
```

### POST `/api/consultas/1/finalizar`

```json
{
  "diagnostico": "Animal saudável, sem alterações no exame clínico.",
  "tratamento": "Manter vacinação em dia e retornar para avaliação anual."
}
```

Ao finalizar, a API altera o status da consulta para `Finalizada` e cria automaticamente um registro no histórico do animal.

### POST `/api/animais/1/historico`

```json
{
  "tipoRegistro": "Exame",
  "descricao": "Exame de sangue realizado para avaliação de rotina."
}
```

## Respostas HTTP utilizadas

- `200 OK`: consulta ou operação concluída com retorno.
- `201 Created`: registro criado.
- `204 No Content`: atualização ou exclusão concluída sem corpo de resposta.
- `400 Bad Request`: dados inválidos ou regra de negócio não atendida.
- `404 Not Found`: tutor, animal ou consulta não encontrado.
- `409 Conflict`: e-mail duplicado ou conflito de horário.

## Teste de persistência

1. Cadastre um tutor ou animal pelo Swagger.
2. Pare a API e execute novamente.
3. Faça um GET para consultar o registro.
4. Confira também no pgAdmin.

Os dados devem continuar no PostgreSQL após reiniciar a aplicação.
