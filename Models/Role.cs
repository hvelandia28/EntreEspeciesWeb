using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models;

public partial class Role
{
    public int IdRol { get; set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 50 caracteres")]
    public string? NomRol { get; set; }

    public virtual ICollection<Configuracion> Configuracions { get; set; } = new List<Configuracion>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
