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
