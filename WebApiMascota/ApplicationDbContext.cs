using Microsoft.EntityFrameworkCore;
using WebApiMascota.Entidades;

namespace WebApiMascota
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Mascota> Mascotas { get; set; }

        public DbSet<Dueño> Dueños { get; set; }
    }
}
