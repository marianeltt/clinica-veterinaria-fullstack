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
