using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EntreEspeciesNuevo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace EntreEspeciesNuevo.Models;
public partial class Producto
{

    public int IdProducto { get; set; }
    [Required(ErrorMessage = "La Categoria es Obligatoria.")]

    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "El Nombre es Obligatorio*")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El Nombre debe tener entre 2 y 50 caracteres")]
    public string NomProducto { get; set; }

    public int Disponibilidad { get; set; }
    [Required(ErrorMessage = "La Cantidad es Obligatoria*")]

    public int? Cantidad { get; set; }

    public DateTime? FechaVen { get; set; }

    [Required(ErrorMessage = "El Precio es Obligatorio*")]

    public int? Precio { get; set; }
    public byte[]? Foto { get; set; }

    public virtual ICollection<DetaCompra> DetaCompras { get; set; } = new List<DetaCompra>();

    public virtual ICollection<DetaVentum> DetaVenta { get; set; } = new List<DetaVentum>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }





}
public static class HashHelper
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash: Devuelve un arreglo de bytes
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convertir arreglo de bytes a cadena hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}