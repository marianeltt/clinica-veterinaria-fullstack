using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Consulta
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int AnimalId { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly Horario { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Motivo { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Agendada";

    public DateTime? DataRealizacao { get; set; }

    [JsonIgnore]
    public Animal? Animal { get; set; }
}
