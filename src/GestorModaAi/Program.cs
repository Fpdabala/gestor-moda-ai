using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "../../../wwwroot")
});

// Configurar a ligação ao PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Obrigatório para servir o index.html da pasta wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Mapeia os controladores da API
app.MapControllers();


app.Run();