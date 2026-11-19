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
