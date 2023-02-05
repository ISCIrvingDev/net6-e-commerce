using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.API.Data;
using ShopOnline.API.Repositories;
using ShopOnline.API.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EntityFrameworkCore Migrations - Code First
/*
Notas:
* xxx -> Es el nombre de la migracion

Comandos de las migraciones (todos los proyectos ya tienen que estar "build" para que funcione)
 * Add-Migration xxx -> Genera la migracion
 * update-database -> Ejecuta la migracion
 * update-database 0 -> Hace un rollback de todas las migraciones
 * remove-migration -> Remueve la "x" migracion
 * update-database xxx -> Remueve la "x" migracion
*/
// Contexto -> ShopOnlineDbContext
builder.Services.AddDbContextPool<ShopOnlineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"))
);

// Repositorios (Capa de dominio)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuracion de los CORS
app.UseCors(policy =>
    //policy.WithOrigins(
    //    "https://localhost:3000",
    //    "https://localhost:7060",
    //    "https://localhost:7061"
    //)
    policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
