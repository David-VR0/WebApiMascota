using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiMascota.Validaciones;

namespace WebApiMascota.Entidades
{
    public class Dueño : IValidatableObject
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new String[] { nameof(Nombre) });
                }
            }
        }
    }
    
}
