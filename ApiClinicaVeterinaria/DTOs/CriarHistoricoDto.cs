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
