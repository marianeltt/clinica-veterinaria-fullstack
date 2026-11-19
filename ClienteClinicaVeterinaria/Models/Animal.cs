namespace ClienteClinicaVeterinaria.Models;

public class Animal
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Especie { get; set; } = "";
    public string Raca { get; set; } = "";
    public string Sexo { get; set; } = "";
    public DateOnly DataNascimento { get; set; }
    public int TutorId { get; set; }
}