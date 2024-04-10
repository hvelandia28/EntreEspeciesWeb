using EntreEspeciesNuevo.models;

namespace EntreEspeciesNuevo.Models
{
    public class DetalleCitaViewModel
    {
        public Mascota Mascota { get; set; }
        public Cliente Cliente { get; set; }
        public CitaInterna CitaInterna { get; set; }
        public Servicio? Servicio { get; set; }

    }
}
