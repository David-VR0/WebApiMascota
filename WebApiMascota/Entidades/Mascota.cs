using System.ComponentModel.DataAnnotations;
using WebApiMascota.Validaciones;

namespace WebApiMascota.Entidades
{
    public class Mascota
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        [Range(1, 20, ErrorMessage = "El campo Edad no se encuentra dentro del rango")]
        public int Edad { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Tipo { get; set; }

        public int DueñoId { get; set; }


    }
}
