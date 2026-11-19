namespace ClienteClinicaVeterinaria.Models;

public class CriarTutorDto
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telefone { get; set; } = "";
}

public class AtualizarTutorDto
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telefone { get; set; } = "";
}

public class CriarAnimalDto
{
    public string Nome { get; set; } = "";
    public string Especie { get; set; } = "";
    public string Raca { get; set; } = "";
    public string Sexo { get; set; } = "";
    public DateOnly DataNascimento { get; set; }
    public int TutorId { get; set; }
}

public class AtualizarAnimalDto
{
    public string Nome { get; set; } = "";
    public string Especie { get; set; } = "";
    public string Raca { get; set; } = "";
    public string Sexo { get; set; } = "";
    public DateOnly DataNascimento { get; set; }
    public int TutorId { get; set; }
}

public class AgendarConsultaDto
{
    public int AnimalId { get; set; }
    public DateOnly Data { get; set; }
    public TimeOnly Horario { get; set; }
    public string Motivo { get; set; } = "";
}

public class AtualizarConsultaDto
{
    public DateOnly Data { get; set; }
    public TimeOnly Horario { get; set; }
    public string Motivo { get; set; } = "";
}

public class FinalizarConsultaDto
{
    public string Diagnostico { get; set; } = "";
    public string Tratamento { get; set; } = "";
}

public class CriarHistoricoDto
{
    public string TipoRegistro { get; set; } = "";
    public string Descricao { get; set; } = "";
}