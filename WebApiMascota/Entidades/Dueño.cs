namespace WebApiMascota.Entidades
{
    public class Dueño
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        
        public int Edad { get; set; }

        public List<Mascota> mascotas { get; set; }
    }
}
