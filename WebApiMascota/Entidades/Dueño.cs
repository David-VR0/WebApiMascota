using System.ComponentModel.DataAnnotations;
using WebApiMascota.Validaciones;

namespace WebApiMascota.Entidades
{
    public class Dueño
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        [Range(18,100, ErrorMessage = "El campo Edad no se encuentra dentro del rango")]
        public int Edad { get; set; }
        [Phone(ErrorMessage = "El campo Teléfono no es un número de teléfono válido")]
        public string Telefono { get; set; }
        public List<Mascota> mascotas { get; set; }
    }
}
