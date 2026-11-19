using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaVeterinaria.Models;

public class Tutor
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Animal> Animais { get; set; } = new List<Animal>();
}
