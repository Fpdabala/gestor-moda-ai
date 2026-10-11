using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contentor.
builder.Services.AddControllers();

// Configurar a ligação ao PostgreSQL (certifique-se de que a connection string está no appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar suporte ao Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Isto ativa a interface gráfica do Swagger na raiz
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();