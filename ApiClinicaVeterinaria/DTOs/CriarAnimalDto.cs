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
