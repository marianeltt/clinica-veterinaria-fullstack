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
