namespace ClienteClinicaVeterinaria.Models;

public class Consulta
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public DateOnly Data { get; set; }
    public TimeOnly Horario { get; set; }
    public string Motivo { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime? DataRealizacao { get; set; }
}