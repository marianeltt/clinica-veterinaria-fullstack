using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tutores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Especie = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Raca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sexo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    TutorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animais_Tutores_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Tutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataRealizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Animais_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Historicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    ConsultaId = table.Column<int>(type: "integer", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoRegistro = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historicos_Animais_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Historicos_Consultas_ConsultaId",
                        column: x => x.ConsultaId,
                        principalTable: "Consultas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tutores",
                columns: new[] { "Id", "Email", "Nome", "Telefone" },
                values: new object[,]
                {
                    { 1, "ana.souza@email.com", "Ana Souza", "(17) 99999-1111" },
                    { 2, "carlos.lima@email.com", "Carlos Lima", "(17) 99999-2222" }
                });

            migrationBuilder.InsertData(
                table: "Animais",
                columns: new[] { "Id", "DataNascimento", "Especie", "Nome", "Raca", "Sexo", "TutorId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2021, 5, 10), "Cachorro", "Rex", "Labrador", "Macho", 1 },
                    { 2, new DateOnly(2022, 8, 20), "Gato", "Mia", "Siamês", "Fêmea", 2 }
                });

            migrationBuilder.InsertData(
                table: "Consultas",
                columns: new[] { "Id", "AnimalId", "Data", "DataRealizacao", "Horario", "Motivo", "Status" },
                values: new object[] { 1, 1, new DateOnly(2026, 11, 17), null, new TimeOnly(15, 0, 0), "Avaliação de rotina", "Agendada" });

            migrationBuilder.InsertData(
                table: "Historicos",
                columns: new[] { "Id", "AnimalId", "ConsultaId", "DataRegistro", "Descricao", "TipoRegistro" },
                values: new object[] { 1, 1, null, new DateTime(2026, 10, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Aplicação de vacina antirrábica.", "Vacinação" });

            migrationBuilder.CreateIndex(
                name: "IX_Animais_TutorId",
                table: "Animais",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_AnimalId",
                table: "Consultas",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_AnimalId",
                table: "Historicos",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_ConsultaId",
                table: "Historicos",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutores_Email",
                table: "Tutores",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historicos");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Animais");

            migrationBuilder.DropTable(
                name: "Tutores");
        }
    }
}
