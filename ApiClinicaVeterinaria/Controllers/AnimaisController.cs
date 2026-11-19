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
