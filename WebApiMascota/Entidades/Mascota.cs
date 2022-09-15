namespace WebApiMascota.Entidades
{
    public class Mascota
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int Edad { get; set; }

        public string Tipo { get; set; }

        public int DueñoId { get; set; }


    }
}
