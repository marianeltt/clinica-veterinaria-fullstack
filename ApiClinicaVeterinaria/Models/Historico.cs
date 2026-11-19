using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Historico
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int AnimalId { get; set; }

    public int? ConsultaId { get; set; }

    public DateTime DataRegistro { get; set; }

    [Required]
    [StringLength(60)]
    public string TipoRegistro { get; set; } = string.Empty;

    [Required]
    [StringLength(1500, MinimumLength = 5)]
    public string Descricao { get; set; } = string.Empty;

    [JsonIgnore]
    public Animal? Animal { get; set; }

    [JsonIgnore]
    public Consulta? Consulta { get; set; }
}
