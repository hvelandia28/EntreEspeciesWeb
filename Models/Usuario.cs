using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntreEspeciesNuevo.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreEspeciesNuevo.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int? IdRol { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligario*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? Nombre { get; set; }

    public string? TipoDoc { get; set; }
    [Required(ErrorMessage = "El Documento es Obligatorio*")]
    [Range(1000000, 9999999999, ErrorMessage = "El N° Documento debe tener entre 7 y 10 dígitos")]
    public int? Documento { get; set; }

    public string? Telefono { get; set; }
    [Required(ErrorMessage = "El Correo es Obligatorio*")]
    [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "El Correo no contiene un formato válido")]
    public string? Correo { get; set; }

    //[Required(ErrorMessage = "La Contraseña es Obligatoria*")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "La Contraseña debe tener entre 8 y 50 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$", ErrorMessage = "La Contraseña debe tener entre 8 y 50 caracteres al menos una letra mayúscula, una minúscula, un número y un carácter especial")]    
    public string? Contraseña { get; set; }
    public string? Estado { get; set; }
    [Required(ErrorMessage = "La Dirección es Obligatoria*")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "La Dirección debe tener entre 4 y 100 caracteres")]
    public string? Direccion { get; set; }
    public virtual Role? IdRolNavigation { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiration { get; set; }
}


public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Usuario", async (EntreespeciessqlContext db) =>
        {
            return await db.Usuarios.ToListAsync();
        })
        .WithName("GetAllUsuarios")
        .Produces<List<Usuario>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Usuario/{id}", async (int IdUsuario, EntreespeciessqlContext db) =>
        {
            return await db.Usuarios.FindAsync(IdUsuario)
                is Usuario model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetUsuarioById")
        .Produces<Usuario>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Usuario/{id}", async (int IdUsuario, Usuario usuario, EntreespeciessqlContext db) =>
        {
            var foundModel = await db.Usuarios.FindAsync(IdUsuario);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(usuario);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateUsuario")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Usuario/", async (Usuario usuario, EntreespeciessqlContext db) =>
        {
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            return Results.Created($"/Usuarios/{usuario.IdUsuario}", usuario);
        })
        .WithName("CreateUsuario")
        .Produces<Usuario>(StatusCodes.Status201Created);


        routes.MapDelete("/api/Usuario/{id}", async (int IdUsuario, EntreespeciessqlContext db) =>
        {
            if (await db.Usuarios.FindAsync(IdUsuario) is Usuario usuario)
            {
                db.Usuarios.Remove(usuario);
                await db.SaveChangesAsync();
                return Results.Ok(usuario);
            }

            return Results.NotFound();
        })
        .WithName("DeleteUsuario")
        .Produces<Usuario>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}