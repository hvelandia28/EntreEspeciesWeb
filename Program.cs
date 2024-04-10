using EntreEspeciesNuevo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Controllers;
using EntreEspeciesNuevo.models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddDbContext<EntreespeciessqlContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("Conexion")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Index"; // Ruta de la página de inicio de sesión
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LogoutPath = "/Acceso/Logout"; // Ruta de la página de cierre de sesión
    });

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Creación de la aplicación
var app = builder.Build();

// Configuración del pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAnyOrigin");

// Configuración de enrutamiento de controladores y endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers(); // Esto habilitará la API

app.MapUsuarioEndpoints();
app.MapCitaInternaEndpoints();

app.Run();