using Microsoft.AspNetCore.Mvc.Rendering;

namespace EntreEspeciesNuevo.Models
{
    public class VentaViewModel
    {
        public List<Venta> Ventas { get; set; }
        public List<DetaVentum> DetalleVentas { get; set; }
    }
}
