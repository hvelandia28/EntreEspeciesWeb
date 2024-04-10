using System.ComponentModel.DataAnnotations;

namespace EntreEspeciesNuevo.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        //[Required(ErrorMessage = "La Contraseña es Obligatoria*")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "La Contraseña debe tener entre 8 y 50 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$", ErrorMessage = "La Contraseña debe tener entre 8 y 50 caracteres al menos una letra mayúscula, una minúscula, un número y un carácter especial")]
        public string NuevaContraseña { get; set; }
        
    }
}
