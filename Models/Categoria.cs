using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }
    [Required(ErrorMessage = "El Nombre es Obligatorio*")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre de la categoria debe tener entre 3 y 50 caracteres.")]
    public string? NomCategoria { get; set; }
    public string? Estado { get; set; } 

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
