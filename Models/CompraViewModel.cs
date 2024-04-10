    using System.ComponentModel.DataAnnotations.Schema;

    namespace EntreEspeciesNuevo.Models
    {
    public class CompraViewModel
    {
        [NotMapped]
        public List<DetaCompra> DetaCompras { get; set; }
        [NotMapped]
        public List<Compra> Compras { get; set; }  
    }
}
