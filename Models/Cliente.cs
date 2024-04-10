using EntreEspeciesNuevo.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models;

public partial class Cliente
{
    [Required(ErrorMessage = "El Documento es Obligatorio*")]
    [Range(100000, 9999999999, ErrorMessage = "El N° Documento debe tener entre 6 y 10 dígitos")]
    public int DocumentoCliente { get; set; }

    public string? TipoDocu { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligatorio*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? NombreCliente { get; set; }
    [Required(ErrorMessage = "La Dirección es Obligatoria*")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "La Dirección debe tener entre 4 y 100 caracteres")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "El Telefono es Obligatorio*")]
    [Range(100000, 999999999999999, ErrorMessage = "El Telefono debe tener entre 6 y 15 dígitos")]
    public string? Telefono { get; set; }
    [Required(ErrorMessage = "El Correo es Obligatorio*")]
    [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "El Correo no contiene un formato válido")]
    public string? Correo { get; set; }

    public virtual ICollection<CitaInterna> CitaInternas { get; } = new List<CitaInterna>();
    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
