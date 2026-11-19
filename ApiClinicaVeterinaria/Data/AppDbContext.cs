using ApiClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaVeterinaria.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Historico> Historicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Um tutor pode possuir vários animais. O tutor não pode ser excluído
        // enquanto houver animais vinculados a ele.
        modelBuilder.Entity<Tutor>()
            .HasMany(t => t.Animais)
            .WithOne(a => a.Tutor)
            .HasForeignKey(a => a.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um animal pode possuir várias consultas.
        modelBuilder.Entity<Animal>()
            .HasMany(a => a.Consultas)
            .WithOne(c => c.Animal)
            .HasForeignKey(c => c.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um animal pode possuir vários registros no histórico.
        modelBuilder.Entity<Animal>()
            .HasMany(a => a.Historicos)
            .WithOne(h => h.Animal)
            .HasForeignKey(h => h.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um registro de histórico pode estar relacionado a uma consulta.
        modelBuilder.Entity<Historico>()
            .HasOne(h => h.Consulta)
            .WithMany()
            .HasForeignKey(h => h.ConsultaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tutor>()
            .HasIndex(t => t.Email)
            .IsUnique();

        // Dados iniciais para facilitar os testes no Swagger.
        modelBuilder.Entity<Tutor>().HasData(
            new Tutor
            {
                Id = 1,
                Nome = "Ana Souza",
                Email = "ana.souza@email.com",
                Telefone = "(17) 99999-1111"
            },
            new Tutor
            {
                Id = 2,
                Nome = "Carlos Lima",
                Email = "carlos.lima@email.com",
                Telefone = "(17) 99999-2222"
            }
        );

        modelBuilder.Entity<Animal>().HasData(
            new Animal
            {
                Id = 1,
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                DataNascimento = new DateOnly(2021, 5, 10),
                TutorId = 1
            },
            new Animal
            {
                Id = 2,
                Nome = "Mia",
                Especie = "Gato",
                Raca = "Siamês",
                Sexo = "Fêmea",
                DataNascimento = new DateOnly(2022, 8, 20),
                TutorId = 2
            }
        );

        modelBuilder.Entity<Consulta>().HasData(
            new Consulta
            {
                Id = 1,
                AnimalId = 1,
                Data = new DateOnly(2026, 11, 17),
                Horario = new TimeOnly(15, 0),
                Motivo = "Avaliação de rotina",
                Status = "Agendada",
                DataRealizacao = null
            }
        );

        modelBuilder.Entity<Historico>().HasData(
            new Historico
            {
                Id = 1,
                AnimalId = 1,
                ConsultaId = null,
                DataRegistro = new DateTime(2026, 10, 15, 12, 0, 0, DateTimeKind.Utc),
                TipoRegistro = "Vacinação",
                Descricao = "Aplicação de vacina antirrábica."
            }
        );
    }
}
