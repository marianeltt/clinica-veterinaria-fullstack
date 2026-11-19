namespace ClienteClinicaVeterinaria.Models;

public class Historico
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public int? ConsultaId { get; set; }
    public DateTime DataRegistro { get; set; }
    public string TipoRegistro { get; set; } = "";
    public string Descricao { get; set; } = "";
}