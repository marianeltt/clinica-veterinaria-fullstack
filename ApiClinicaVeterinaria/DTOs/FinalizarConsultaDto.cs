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
