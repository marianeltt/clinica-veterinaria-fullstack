# Códigos da API da Clínica Veterinária

Este arquivo reúne os códigos-fonte do projeto. Os arquivos estão separados por caminho; crie-os no Rider com os mesmos nomes ou abra diretamente o projeto do arquivo ZIP.


## Models/Animal.cs

```csharp
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Animal
{
    public int Id { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Especie { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Raca { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Sexo { get; set; } = string.Empty;

    public DateOnly DataNascimento { get; set; }

    [Range(1, int.MaxValue)]
    public int TutorId { get; set; }

    [JsonIgnore]
    public Tutor? Tutor { get; set; }

    [JsonIgnore]
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

    [JsonIgnore]
    public ICollection<Historico> Historicos { get; set; } = new List<Historico>();
}
```

## Models/Consulta.cs

```csharp
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Consulta
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int AnimalId { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly Horario { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Motivo { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Agendada";

    public DateTime? DataRealizacao { get; set; }

    [JsonIgnore]
    public Animal? Animal { get; set; }
}
```

## Models/Historico.cs

```csharp
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Historico
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int AnimalId { get; set; }

    public int? ConsultaId { get; set; }

    public DateTime DataRegistro { get; set; }

    [Required]
    [StringLength(60)]
    public string TipoRegistro { get; set; } = string.Empty;

    [Required]
    [StringLength(1500, MinimumLength = 5)]
    public string Descricao { get; set; } = string.Empty;

    [JsonIgnore]
    public Animal? Animal { get; set; }

    [JsonIgnore]
    public Consulta? Consulta { get; set; }
}
```

## Models/Tutor.cs

```csharp
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Tutor
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Animal> Animais { get; set; } = new List<Animal>();
}
```

## DTOs/AgendarConsultaDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados necessários para agendar uma consulta.</summary>
public class AgendarConsultaDto
{
    /// <summary>Identificador do animal que será atendido.</summary>
    [Range(1, int.MaxValue)]
    public int AnimalId { get; set; }

    /// <summary>Data da consulta no formato AAAA-MM-DD.</summary>
    public DateOnly Data { get; set; }

    /// <summary>Horário da consulta no formato HH:mm:ss.</summary>
    public TimeOnly Horario { get; set; }

    /// <summary>Motivo informado para a consulta.</summary>
    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Motivo { get; set; } = string.Empty;
}
```

## DTOs/AtualizarAnimalDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados para atualizar um animal existente.</summary>
public class AtualizarAnimalDto
{
    /// <summary>Nome do animal.</summary>
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>Espécie do animal.</summary>
    [Required]
    [StringLength(50)]
    public string Especie { get; set; } = string.Empty;

    /// <summary>Raça do animal.</summary>
    [Required]
    [StringLength(50)]
    public string Raca { get; set; } = string.Empty;

    /// <summary>Sexo do animal: Macho ou Fêmea.</summary>
    [Required]
    [RegularExpression("^(Macho|Fêmea)$", ErrorMessage = "Informe Macho ou Fêmea.")]
    public string Sexo { get; set; } = string.Empty;

    /// <summary>Data de nascimento no formato AAAA-MM-DD.</summary>
    public DateOnly DataNascimento { get; set; }

    /// <summary>Identificador do tutor responsável.</summary>
    [Range(1, int.MaxValue)]
    public int TutorId { get; set; }
}
```

## DTOs/AtualizarConsultaDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados para alterar data, horário e motivo de uma consulta agendada.</summary>
public class AtualizarConsultaDto
{
    /// <summary>Nova data da consulta no formato AAAA-MM-DD.</summary>
    public DateOnly Data { get; set; }

    /// <summary>Novo horário da consulta no formato HH:mm:ss.</summary>
    public TimeOnly Horario { get; set; }

    /// <summary>Novo motivo da consulta.</summary>
    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Motivo { get; set; } = string.Empty;
}
```

## DTOs/AtualizarTutorDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados para atualizar um tutor existente.</summary>
public class AtualizarTutorDto
{
    /// <summary>Nome completo do tutor.</summary>
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>E-mail do tutor. Deve ser único.</summary>
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    /// <summary>Telefone de contato do tutor.</summary>
    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;
}
```

## DTOs/CriarAnimalDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados necessários para cadastrar um animal.</summary>
public class CriarAnimalDto
{
    /// <summary>Nome do animal.</summary>
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>Espécie do animal, por exemplo Cachorro ou Gato.</summary>
    [Required]
    [StringLength(50)]
    public string Especie { get; set; } = string.Empty;

    /// <summary>Raça do animal.</summary>
    [Required]
    [StringLength(50)]
    public string Raca { get; set; } = string.Empty;

    /// <summary>Sexo do animal.</summary>
    [Required]
    [RegularExpression("^(Macho|Fêmea)$", ErrorMessage = "Informe Macho ou Fêmea.")]
    public string Sexo { get; set; } = string.Empty;

    /// <summary>Data de nascimento no formato AAAA-MM-DD.</summary>
    public DateOnly DataNascimento { get; set; }

    /// <summary>Identificador do tutor responsável.</summary>
    [Range(1, int.MaxValue)]
    public int TutorId { get; set; }
}
```

## DTOs/CriarHistoricoDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados para adicionar uma anotação ao histórico clínico do animal.</summary>
public class CriarHistoricoDto
{
    /// <summary>Tipo do registro, por exemplo Vacinação, Exame ou Observação.</summary>
    [Required]
    [StringLength(60)]
    public string TipoRegistro { get; set; } = string.Empty;

    /// <summary>Descrição da informação clínica.</summary>
    [Required]
    [StringLength(1500, MinimumLength = 5)]
    public string Descricao { get; set; } = string.Empty;
}
```

## DTOs/CriarTutorDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Dados necessários para cadastrar um tutor.</summary>
public class CriarTutorDto
{
    /// <summary>Nome completo do tutor.</summary>
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>E-mail do tutor. Deve ser único.</summary>
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    /// <summary>Telefone de contato do tutor.</summary>
    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;
}
```

## DTOs/FinalizarConsultaDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaVeterinaria.DTOs;

/// <summary>Informações clínicas necessárias para finalizar uma consulta.</summary>
public class FinalizarConsultaDto
{
    /// <summary>Diagnóstico realizado pelo veterinário.</summary>
    [Required]
    [StringLength(1000, MinimumLength = 5)]
    public string Diagnostico { get; set; } = string.Empty;

    /// <summary>Tratamento ou orientação indicada.</summary>
    [Required]
    [StringLength(1000, MinimumLength = 5)]
    public string Tratamento { get; set; } = string.Empty;
}
```

## Data/AppDbContext.cs

```csharp
using ApiClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaVeterinaria.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Historico> Historicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Um tutor pode possuir vários animais. O tutor não pode ser excluído
        // enquanto houver animais vinculados a ele.
        modelBuilder.Entity<Tutor>()
            .HasMany(t => t.Animais)
            .WithOne(a => a.Tutor)
            .HasForeignKey(a => a.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um animal pode possuir várias consultas.
        modelBuilder.Entity<Animal>()
            .HasMany(a => a.Consultas)
            .WithOne(c => c.Animal)
            .HasForeignKey(c => c.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um animal pode possuir vários registros no histórico.
        modelBuilder.Entity<Animal>()
            .HasMany(a => a.Historicos)
            .WithOne(h => h.Animal)
            .HasForeignKey(h => h.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um registro de histórico pode estar relacionado a uma consulta.
        modelBuilder.Entity<Historico>()
            .HasOne(h => h.Consulta)
            .WithMany()
            .HasForeignKey(h => h.ConsultaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tutor>()
            .HasIndex(t => t.Email)
            .IsUnique();

        // Dados iniciais para facilitar os testes no Swagger.
        modelBuilder.Entity<Tutor>().HasData(
            new Tutor
            {
                Id = 1,
                Nome = "Ana Souza",
                Email = "ana.souza@email.com",
                Telefone = "(17) 99999-1111"
            },
            new Tutor
            {
                Id = 2,
                Nome = "Carlos Lima",
                Email = "carlos.lima@email.com",
                Telefone = "(17) 99999-2222"
            }
        );

        modelBuilder.Entity<Animal>().HasData(
            new Animal
            {
                Id = 1,
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                DataNascimento = new DateOnly(2021, 5, 10),
                TutorId = 1
            },
            new Animal
            {
                Id = 2,
                Nome = "Mia",
                Especie = "Gato",
                Raca = "Siamês",
                Sexo = "Fêmea",
                DataNascimento = new DateOnly(2022, 8, 20),
                TutorId = 2
            }
        );

        modelBuilder.Entity<Consulta>().HasData(
            new Consulta
            {
                Id = 1,
                AnimalId = 1,
                Data = new DateOnly(2026, 11, 17),
                Horario = new TimeOnly(15, 0),
                Motivo = "Avaliação de rotina",
                Status = "Agendada",
                DataRealizacao = null
            }
        );

        modelBuilder.Entity<Historico>().HasData(
            new Historico
            {
                Id = 1,
                AnimalId = 1,
                ConsultaId = null,
                DataRegistro = new DateTime(2026, 10, 15, 12, 0, 0, DateTimeKind.Utc),
                TipoRegistro = "Vacinação",
                Descricao = "Aplicação de vacina antirrábica."
            }
        );
    }
}
```

## Controllers/AnimaisController.cs

```csharp
using ApiClinicaVeterinaria.Data;
using ApiClinicaVeterinaria.DTOs;
using ApiClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaVeterinaria.Controllers;

[ApiController]
[Route("api/animais")]
public class AnimaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnimaisController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Retorna os animais cadastrados, com filtros opcionais.</summary>
    /// <param name="tutorId">Filtra pelo identificador do tutor.</param>
    /// <param name="especie">Filtra pela espécie do animal.</param>
    /// <returns>Lista de animais.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Animal>>> Listar(
        [FromQuery] int? tutorId,
        [FromQuery] string? especie)
    {
        var query = _context.Animais.AsQueryable();

        if (tutorId.HasValue)
            query = query.Where(a => a.TutorId == tutorId.Value);

        if (!string.IsNullOrWhiteSpace(especie))
            query = query.Where(a => a.Especie.ToLower() == especie.ToLower());

        var animais = await query.OrderBy(a => a.Nome).ToListAsync();
        return Ok(animais);
    }

    /// <summary>Busca um animal pelo identificador.</summary>
    /// <param name="id">Identificador do animal.</param>
    /// <returns>Dados do animal encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Animal>> BuscarPorId(int id)
    {
        var animal = await _context.Animais.FindAsync(id);

        if (animal is null)
            return NotFound(new { mensagem = $"Animal com ID {id} não encontrado." });

        return Ok(animal);
    }

    /// <summary>Cadastra um animal e o vincula a um tutor existente.</summary>
    /// <param name="dados">Dados do animal e identificador do tutor.</param>
    /// <returns>O animal cadastrado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Animal>> Cadastrar([FromBody] CriarAnimalDto dados)
    {
        var tutorExiste = await _context.Tutores
            .AnyAsync(t => t.Id == dados.TutorId);

        if (!tutorExiste)
            return NotFound(new { mensagem = $"Tutor com ID {dados.TutorId} não encontrado." });

        if (dados.DataNascimento == default || dados.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
            return BadRequest(new { mensagem = "Informe uma data de nascimento válida, que não seja futura." });

        var animal = new Animal
        {
            Nome = dados.Nome,
            Especie = dados.Especie,
            Raca = dados.Raca,
            Sexo = dados.Sexo,
            DataNascimento = dados.DataNascimento,
            TutorId = dados.TutorId
        };

        await _context.Animais.AddAsync(animal);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = animal.Id }, animal);
    }

    /// <summary>Atualiza os dados de um animal existente.</summary>
    /// <param name="id">Identificador do animal.</param>
    /// <param name="dados">Novos dados do animal.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarAnimalDto dados)
    {
        var animal = await _context.Animais.FindAsync(id);

        if (animal is null)
            return NotFound(new { mensagem = $"Animal com ID {id} não encontrado." });

        var tutorExiste = await _context.Tutores
            .AnyAsync(t => t.Id == dados.TutorId);

        if (!tutorExiste)
            return NotFound(new { mensagem = $"Tutor com ID {dados.TutorId} não encontrado." });

        if (dados.DataNascimento == default || dados.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
            return BadRequest(new { mensagem = "Informe uma data de nascimento válida, que não seja futura." });

        animal.Nome = dados.Nome;
        animal.Especie = dados.Especie;
        animal.Raca = dados.Raca;
        animal.Sexo = dados.Sexo;
        animal.DataNascimento = dados.DataNascimento;
        animal.TutorId = dados.TutorId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Exclui um animal que ainda não possua consultas ou histórico clínico.</summary>
    /// <param name="id">Identificador do animal.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        var animal = await _context.Animais.FindAsync(id);

        if (animal is null)
            return NotFound(new { mensagem = $"Animal com ID {id} não encontrado." });

        var possuiConsultas = await _context.Consultas.AnyAsync(c => c.AnimalId == id);
        var possuiHistorico = await _context.Historicos.AnyAsync(h => h.AnimalId == id);

        if (possuiConsultas || possuiHistorico)
            return BadRequest(new { mensagem = "Não é possível excluir o animal porque existem consultas ou registros no histórico clínico." });

        _context.Animais.Remove(animal);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>Consulta o histórico clínico de um animal.</summary>
    /// <param name="id">Identificador do animal.</param>
    /// <returns>Registros do histórico em ordem decrescente de data.</returns>
    [HttpGet("{id:int}/historico")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Historico>>> ConsultarHistorico(int id)
    {
        var animalExiste = await _context.Animais.AnyAsync(a => a.Id == id);

        if (!animalExiste)
            return NotFound(new { mensagem = $"Animal com ID {id} não encontrado." });

        var historicos = await _context.Historicos
            .Where(h => h.AnimalId == id)
            .OrderByDescending(h => h.DataRegistro)
            .ToListAsync();

        return Ok(historicos);
    }

    /// <summary>Adiciona uma anotação ao histórico clínico de um animal.</summary>
    /// <param name="id">Identificador do animal.</param>
    /// <param name="dados">Tipo e descrição do registro clínico.</param>
    /// <returns>O registro incluído no histórico.</returns>
    [HttpPost("{id:int}/historico")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Historico>> RegistrarHistorico(
        int id,
        [FromBody] CriarHistoricoDto dados)
    {
        var animalExiste = await _context.Animais.AnyAsync(a => a.Id == id);

        if (!animalExiste)
            return NotFound(new { mensagem = $"Animal com ID {id} não encontrado." });

        var historico = new Historico
        {
            AnimalId = id,
            DataRegistro = DateTime.UtcNow,
            TipoRegistro = dados.TipoRegistro,
            Descricao = dados.Descricao
        };

        await _context.Historicos.AddAsync(historico);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ConsultarHistorico), new { id }, historico);
    }
}
```

## Controllers/ConsultasController.cs

```csharp
using ApiClinicaVeterinaria.Data;
using ApiClinicaVeterinaria.DTOs;
using ApiClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaVeterinaria.Controllers;

[ApiController]
[Route("api/consultas")]
public class ConsultasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ConsultasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Lista as consultas cadastradas, permitindo filtros por animal e status.</summary>
    /// <param name="animalId">Identificador opcional do animal.</param>
    /// <param name="status">Status opcional: Agendada ou Finalizada.</param>
    /// <returns>Lista de consultas.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Consulta>>> Listar(
        [FromQuery] int? animalId,
        [FromQuery] string? status)
    {
        var query = _context.Consultas.AsQueryable();

        if (animalId.HasValue)
            query = query.Where(c => c.AnimalId == animalId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.Status.ToLower() == status.ToLower());

        var consultas = await query
            .OrderBy(c => c.Data)
            .ThenBy(c => c.Horario)
            .ToListAsync();

        return Ok(consultas);
    }

    /// <summary>Busca uma consulta pelo identificador.</summary>
    /// <param name="id">Identificador da consulta.</param>
    /// <returns>Dados da consulta.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Consulta>> BuscarPorId(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta is null)
            return NotFound(new { mensagem = $"Consulta com ID {id} não encontrada." });

        return Ok(consulta);
    }

    /// <summary>
    /// Agenda uma consulta. Verifica se o animal existe, se a data é futura e se não há
    /// outra consulta agendada para o mesmo animal no mesmo dia e horário.
    /// </summary>
    /// <param name="dados">Animal, data, horário e motivo da consulta.</param>
    /// <returns>A consulta agendada.</returns>
    [HttpPost("agendar")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Consulta>> Agendar([FromBody] AgendarConsultaDto dados)
    {
        var animalExiste = await _context.Animais
            .AnyAsync(a => a.Id == dados.AnimalId);

        if (!animalExiste)
            return NotFound(new { mensagem = $"Animal com ID {dados.AnimalId} não encontrado." });

        if (dados.Data == default || dados.Horario == default)
            return BadRequest(new { mensagem = "Informe uma data e um horário válidos para a consulta." });

        var dataHora = dados.Data.ToDateTime(dados.Horario);

        if (dataHora <= DateTime.Now)
            return BadRequest(new { mensagem = "A data e o horário da consulta devem ser futuros." });

        var conflito = await _context.Consultas.AnyAsync(c =>
            c.AnimalId == dados.AnimalId &&
            c.Data == dados.Data &&
            c.Horario == dados.Horario &&
            c.Status == "Agendada");

        if (conflito)
            return Conflict(new { mensagem = "Este animal já possui uma consulta agendada para esse dia e horário." });

        var consulta = new Consulta
        {
            AnimalId = dados.AnimalId,
            Data = dados.Data,
            Horario = dados.Horario,
            Motivo = dados.Motivo,
            Status = "Agendada"
        };

        await _context.Consultas.AddAsync(consulta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = consulta.Id }, consulta);
    }

    /// <summary>Atualiza a data, o horário e o motivo de uma consulta ainda agendada.</summary>
    /// <param name="id">Identificador da consulta.</param>
    /// <param name="dados">Novos dados da consulta.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarConsultaDto dados)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta is null)
            return NotFound(new { mensagem = $"Consulta com ID {id} não encontrada." });

        if (consulta.Status != "Agendada")
            return BadRequest(new { mensagem = "Somente consultas com status Agendada podem ser alteradas." });

        if (dados.Data == default || dados.Horario == default)
            return BadRequest(new { mensagem = "Informe uma data e um horário válidos para a consulta." });

        if (dados.Data.ToDateTime(dados.Horario) <= DateTime.Now)
            return BadRequest(new { mensagem = "A data e o horário da consulta devem ser futuros." });

        var conflito = await _context.Consultas.AnyAsync(c =>
            c.Id != id &&
            c.AnimalId == consulta.AnimalId &&
            c.Data == dados.Data &&
            c.Horario == dados.Horario &&
            c.Status == "Agendada");

        if (conflito)
            return Conflict(new { mensagem = "Este animal já possui outra consulta agendada para esse dia e horário." });

        consulta.Data = dados.Data;
        consulta.Horario = dados.Horario;
        consulta.Motivo = dados.Motivo;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Exclui uma consulta que ainda não foi finalizada.</summary>
    /// <param name="id">Identificador da consulta.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta is null)
            return NotFound(new { mensagem = $"Consulta com ID {id} não encontrada." });

        if (consulta.Status == "Finalizada")
            return BadRequest(new { mensagem = "Não é possível excluir uma consulta finalizada, pois ela possui registro no histórico clínico." });

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Finaliza uma consulta agendada e cria automaticamente um registro no histórico clínico
    /// do animal com o diagnóstico e o tratamento informados.
    /// </summary>
    /// <param name="id">Identificador da consulta.</param>
    /// <param name="dados">Diagnóstico e tratamento.</param>
    /// <returns>A consulta finalizada.</returns>
    [HttpPost("{id:int}/finalizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Consulta>> Finalizar(int id, [FromBody] FinalizarConsultaDto dados)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta is null)
            return NotFound(new { mensagem = $"Consulta com ID {id} não encontrada." });

        if (consulta.Status != "Agendada")
            return BadRequest(new { mensagem = "Somente consultas com status Agendada podem ser finalizadas." });

        if (consulta.Data.ToDateTime(consulta.Horario) > DateTime.Now)
            return BadRequest(new { mensagem = "A consulta só pode ser finalizada depois da data e do horário agendados." });

        consulta.Status = "Finalizada";
        consulta.DataRealizacao = DateTime.UtcNow;

        var historico = new Historico
        {
            AnimalId = consulta.AnimalId,
            ConsultaId = consulta.Id,
            DataRegistro = DateTime.UtcNow,
            TipoRegistro = "Consulta",
            Descricao = $"Diagnóstico: {dados.Diagnostico} Tratamento: {dados.Tratamento}"
        };

        await _context.Historicos.AddAsync(historico);
        await _context.SaveChangesAsync();

        return Ok(consulta);
    }
}
```

## Controllers/TutoresController.cs

```csharp
using ApiClinicaVeterinaria.Data;
using ApiClinicaVeterinaria.DTOs;
using ApiClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaVeterinaria.Controllers;

[ApiController]
[Route("api/tutores")]
public class TutoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public TutoresController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Retorna todos os tutores cadastrados.</summary>
    /// <returns>Lista de tutores.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Tutor>>> Listar()
    {
        var tutores = await _context.Tutores
            .OrderBy(t => t.Nome)
            .ToListAsync();

        return Ok(tutores);
    }

    /// <summary>Busca um tutor pelo identificador.</summary>
    /// <param name="id">Identificador do tutor.</param>
    /// <returns>Dados do tutor encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tutor>> BuscarPorId(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);

        if (tutor is null)
            return NotFound(new { mensagem = $"Tutor com ID {id} não encontrado." });

        return Ok(tutor);
    }

    /// <summary>Cadastra um novo tutor.</summary>
    /// <param name="dados">Nome, e-mail e telefone do tutor.</param>
    /// <returns>O tutor cadastrado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Tutor>> Cadastrar([FromBody] CriarTutorDto dados)
    {
        var emailExiste = await _context.Tutores
            .AnyAsync(t => t.Email.ToLower() == dados.Email.ToLower());

        if (emailExiste)
            return Conflict(new { mensagem = "Já existe um tutor cadastrado com este e-mail." });

        var tutor = new Tutor
        {
            Nome = dados.Nome,
            Email = dados.Email,
            Telefone = dados.Telefone
        };

        await _context.Tutores.AddAsync(tutor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = tutor.Id }, tutor);
    }

    /// <summary>Atualiza os dados de um tutor existente.</summary>
    /// <param name="id">Identificador do tutor.</param>
    /// <param name="dados">Novos dados do tutor.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTutorDto dados)
    {
        var tutor = await _context.Tutores.FindAsync(id);

        if (tutor is null)
            return NotFound(new { mensagem = $"Tutor com ID {id} não encontrado." });

        var emailExiste = await _context.Tutores
            .AnyAsync(t => t.Id != id && t.Email.ToLower() == dados.Email.ToLower());

        if (emailExiste)
            return Conflict(new { mensagem = "O e-mail informado já está cadastrado para outro tutor." });

        tutor.Nome = dados.Nome;
        tutor.Email = dados.Email;
        tutor.Telefone = dados.Telefone;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Exclui um tutor que não possua animais vinculados.</summary>
    /// <param name="id">Identificador do tutor.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);

        if (tutor is null)
            return NotFound(new { mensagem = $"Tutor com ID {id} não encontrado." });

        var possuiAnimais = await _context.Animais
            .AnyAsync(a => a.TutorId == id);

        if (possuiAnimais)
            return BadRequest(new { mensagem = "Não é possível excluir o tutor porque existem animais vinculados a ele." });

        _context.Tutores.Remove(tutor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
```

## Properties/launchSettings.json

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5090",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:7092;http://localhost:5090",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## scripts/criar_banco.sql

```sql
-- Execute este script dentro do banco clinica_veterinaria.
-- Use este script OU as migrations do Entity Framework Core. Nao use os dois.

CREATE TABLE IF NOT EXISTS "Tutores" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "Nome" character varying(100) NOT NULL,
    "Email" character varying(150) NOT NULL,
    "Telefone" character varying(20) NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Tutores_Email"
    ON "Tutores" ("Email");

CREATE TABLE IF NOT EXISTS "Animais" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "Nome" character varying(80) NOT NULL,
    "Especie" character varying(50) NOT NULL,
    "Raca" character varying(50) NOT NULL,
    "Sexo" character varying(20) NOT NULL,
    "DataNascimento" date NOT NULL,
    "TutorId" integer NOT NULL,
    CONSTRAINT "FK_Animais_Tutores_TutorId"
        FOREIGN KEY ("TutorId") REFERENCES "Tutores" ("Id") ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS "Consultas" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "AnimalId" integer NOT NULL,
    "Data" date NOT NULL,
    "Horario" time without time zone NOT NULL,
    "Motivo" character varying(500) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "DataRealizacao" timestamp with time zone NULL,
    CONSTRAINT "FK_Consultas_Animais_AnimalId"
        FOREIGN KEY ("AnimalId") REFERENCES "Animais" ("Id") ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS "Historicos" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "AnimalId" integer NOT NULL,
    "ConsultaId" integer NULL,
    "DataRegistro" timestamp with time zone NOT NULL,
    "TipoRegistro" character varying(60) NOT NULL,
    "Descricao" character varying(1500) NOT NULL,
    CONSTRAINT "FK_Historicos_Animais_AnimalId"
        FOREIGN KEY ("AnimalId") REFERENCES "Animais" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Historicos_Consultas_ConsultaId"
        FOREIGN KEY ("ConsultaId") REFERENCES "Consultas" ("Id") ON DELETE RESTRICT
);

INSERT INTO "Tutores" ("Id", "Nome", "Email", "Telefone") VALUES
    (1, 'Ana Souza', 'ana.souza@email.com', '(17) 99999-1111'),
    (2, 'Carlos Lima', 'carlos.lima@email.com', '(17) 99999-2222')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Animais" ("Id", "Nome", "Especie", "Raca", "Sexo", "DataNascimento", "TutorId") VALUES
    (1, 'Rex', 'Cachorro', 'Labrador', 'Macho', DATE '2021-05-10', 1),
    (2, 'Mia', 'Gato', 'Siamês', 'Fêmea', DATE '2022-08-20', 2)
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Consultas" ("Id", "AnimalId", "Data", "Horario", "Motivo", "Status", "DataRealizacao") VALUES
    (1, 1, DATE '2026-11-17', TIME '15:00:00', 'Avaliação de rotina', 'Agendada', NULL)
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Historicos" ("Id", "AnimalId", "ConsultaId", "DataRegistro", "TipoRegistro", "Descricao") VALUES
    (1, 1, NULL, TIMESTAMPTZ '2026-10-15 12:00:00+00', 'Vacinação', 'Aplicação de vacina antirrábica.')
ON CONFLICT ("Id") DO NOTHING;

SELECT setval(pg_get_serial_sequence('"Tutores"', 'Id'), GREATEST((SELECT MAX("Id") FROM "Tutores"), 1));
SELECT setval(pg_get_serial_sequence('"Animais"', 'Id'), GREATEST((SELECT MAX("Id") FROM "Animais"), 1));
SELECT setval(pg_get_serial_sequence('"Consultas"', 'Id'), GREATEST((SELECT MAX("Id") FROM "Consultas"), 1));
SELECT setval(pg_get_serial_sequence('"Historicos"', 'Id'), GREATEST((SELECT MAX("Id") FROM "Historicos"), 1));
```

## Program.cs

```csharp
using System.Reflection;
using ApiClinicaVeterinaria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Habilita os Controllers da API.
builder.Services.AddControllers();

// Configura o acesso ao PostgreSQL por meio do Entity Framework Core.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Habilita a descoberta dos endpoints para o Swagger.
builder.Services.AddEndpointsApiExplorer();

// Configuração da documentação Swagger.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API da Clínica Veterinária",
        Version = "v1",
        Description = "API REST para gerenciamento de tutores, animais, consultas e histórico clínico."
    });

    // Permite exibir no Swagger os comentários XML dos Controllers e DTOs.
    var nomeXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var caminhoXml = Path.Combine(AppContext.BaseDirectory, nomeXml);

    if (File.Exists(caminhoXml))
    {
        options.IncludeXmlComments(caminhoXml);
    }
});

var app = builder.Build();

// Disponibiliza o Swagger durante o desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API da Clínica Veterinária v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=clinica_veterinaria;Username=postgres;Password=COLOQUE_SUA_SENHA"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## ApiClinicaVeterinaria.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.9" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="10.2.3" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.15">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```
