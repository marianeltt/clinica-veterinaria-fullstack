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
