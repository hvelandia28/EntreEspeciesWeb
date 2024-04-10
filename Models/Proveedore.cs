using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }
    [Required(ErrorMessage = "El Nit es Obligatorio*")]
    [Range(1000000, 999999999999999, ErrorMessage = "El Nit debe tener entre 7 y 15 dígitos")]
    public int? NitProveedor { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligatorio*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? NomProveedor { get; set; }
    [Required(ErrorMessage = "El Correo es Obligatorio*")]
    [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "El Correo no contiene un formato válido")]
    public string? Correo { get; set; }
    [Required(ErrorMessage = "El Telefono es Obligatorio*")]
    [Range(100000, 999999999999999, ErrorMessage = "El Telefono debe tener entre 6 y 15 dígitos")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La Dirección es Obligatoria*")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "La Dirección debe tener entre 4 y 100 caracteres")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "El Nombre de Contacto es Obligatorio*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? contacto { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}