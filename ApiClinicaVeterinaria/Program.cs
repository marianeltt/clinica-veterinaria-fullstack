using System.Reflection;
using ApiClinicaVeterinaria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Habilita os Controllers da API.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClienteClinica", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5000",
                "http://localhost:5200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configura o acesso ao PostgreSQL por meio do Entity Framework Core.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Habilita a descoberta dos endpoints para o Swagger.
builder.Services.AddEndpointsApiExplorer();

// Configuração da documentação Swagger.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API da Clínica Veterinária",
        Version = "v1",
        Description = "API REST para gerenciamento de tutores, animais, consultas e histórico clínico."
    });

    // Permite exibir no Swagger os comentários XML dos Controllers e DTOs.
    var nomeXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var caminhoXml = Path.Combine(AppContext.BaseDirectory, nomeXml);

    if (File.Exists(caminhoXml))
    {
        options.IncludeXmlComments(caminhoXml);
    }
});

var app = builder.Build();

// Disponibiliza o Swagger durante o desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API da Clínica Veterinária v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("ClienteClinica");
app.UseAuthorization();
app.MapControllers();

app.Run();
