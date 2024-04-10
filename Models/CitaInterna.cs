using EntreEspeciesNuevo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.models;

public partial class CitaInterna
{
    public int IdCitaInt { get; set; }

    public int? IdMascota { get; set; }
    [Required(ErrorMessage = "El Documento es Obligatorio*")]
    [Range(1000000, 999999999999999, ErrorMessage = "El N° Documento debe tener entre 7 y 10 dígitos")]
    public int? DocumentoCliente { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligatorio*")] 
    public int? IdServicio { get; set; }
    public string? NomCita { get; set; }
    [Required(ErrorMessage = "La Persona a Cargo es Obligatoria*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? PersonaCargo { get; set; }
    [Required(ErrorMessage = "La Fecha es Obligatoria*")]
    public DateTime? FechaHora { get; set; }

    public string? Estado { get; set; }
    [Required(ErrorMessage = "El Precio es Obligatorio*")]

    public int? Precio { get; set; }

    public virtual Cliente? DocumentoClienteNavigation { get; set; }

    public virtual Mascota? IdMascotaNavigation { get; set; }
    public virtual Servicio? IdServicioNavigation { get; set; }
    public virtual ICollection<DetaVentum> DetaVentas { get; set; } = new List<DetaVentum>();
}


public static class CitaInternaEndpoints
{
	public static void MapCitaInternaEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/CitaInterna", async (EntreespeciessqlContext db) =>
        {
            return await db.CitaInternas.ToListAsync();
        })
        .WithName("GetAllCitaInternas")
        .Produces<List<CitaInterna>>(StatusCodes.Status200OK);

        routes.MapGet("/api/CitaInterna/{id}", async (int IdCitaInt, EntreespeciessqlContext db) =>
        {
            return await db.CitaInternas.FindAsync(IdCitaInt)
                is CitaInterna model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetCitaInternaById")
        .Produces<CitaInterna>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/CitaInterna/{id}", async (int IdCitaInt, CitaInterna citaInterna, EntreespeciessqlContext db) =>
        {
            var foundModel = await db.CitaInternas.FindAsync(IdCitaInt);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(citaInterna);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateCitaInterna")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/CitaInterna/", async (CitaInterna citaInterna, EntreespeciessqlContext db) =>
        {
            db.CitaInternas.Add(citaInterna);
            await db.SaveChangesAsync();
            return Results.Created($"/CitaInternas/{citaInterna.IdCitaInt}", citaInterna);
        })
        .WithName("CreateCitaInterna")
        .Produces<CitaInterna>(StatusCodes.Status201Created);


        routes.MapDelete("/api/CitaInterna/{id}", async (int IdCitaInt, EntreespeciessqlContext db) =>
        {
            if (await db.CitaInternas.FindAsync(IdCitaInt) is CitaInterna citaInterna)
            {
                db.CitaInternas.Remove(citaInterna);
                await db.SaveChangesAsync();
                return Results.Ok(citaInterna);
            }

            return Results.NotFound();
        })
        .WithName("DeleteCitaInterna")
        .Produces<CitaInterna>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
// epit-pdf
