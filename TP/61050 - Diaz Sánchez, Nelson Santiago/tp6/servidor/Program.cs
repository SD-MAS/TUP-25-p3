using Microsoft.EntityFrameworkCore;
using servidor.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowClientApp", policy => {
       policy.WithOrigins("http://localhost:5177") 
      .AllowAnyHeader()
      .AllowAnyMethod();
    });
});

builder.Services.AddControllers(); // <- ¡Esto es crucial!
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowClientApp");

app.UseRouting(); // <- ¡Necesario para los controladores!

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // <- Registra tus controladores como ProductosController
});

app.Run();
