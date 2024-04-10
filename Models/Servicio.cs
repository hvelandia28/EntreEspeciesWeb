using EntreEspeciesNuevo.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models
{
    public partial class Servicio
    {
        public int IdServicio { get; set; }

        [Required(ErrorMessage = "El Nombre es Obligatorio*")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre del Servicio debe tener entre 3 y 50 caracteres.")]
        public string NomServico { get; set; }

        [Required(ErrorMessage = "El Categoría es obligatorio*")]
        [RegularExpression("^(Belleza|Medicinal)$", ErrorMessage = "La Categoría debe ser 'Belleza' o 'Medicinal'.")]
        public string? Categoria { get; set; }

        [Required(ErrorMessage = "El Precio es Obligatorio*")]
        [Range(1000, 999999, ErrorMessage = "El Precio debe estar entre 1000 y 999999.")]
        public int? Precio { get; set; }

        public virtual ICollection<CitaInterna> CitaInternas { get; set; } = new List<CitaInterna>();

    }
}
