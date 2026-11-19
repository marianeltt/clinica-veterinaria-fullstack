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
