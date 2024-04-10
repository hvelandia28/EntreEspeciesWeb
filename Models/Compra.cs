    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace EntreEspeciesNuevo.Models;

    public partial class Compra
    {
        public int IdCompra { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdCategoria { get; set; }

    public DateTime? FechaCompra { get; set; } 

        public string? FormaPago { get; set; }
        public int? Total { get; set; }
        
        public virtual ICollection<DetaCompra> DetaCompras { get; set; } = new List<DetaCompra>();
        public virtual Proveedore? IdProveedorNavigation { get; set; }
    public virtual Categoria? IdCategoriaNavigation { get; set; }
}

