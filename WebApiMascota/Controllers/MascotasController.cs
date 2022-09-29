using Microsoft.AspNetCore.Mvc;
using WebApiMascota.Entidades;
using Microsoft.EntityFrameworkCore;

namespace WebApiMascota.Controllers
{
    [ApiController]
    [Route("api/mascotas")]
    public class MascotasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public MascotasController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Mascota>>> GetAll()
        {
            return await dbContext.Mascotas.ToListAsync();
        }
        [HttpGet("primero")]
        public async Task<ActionResult<Mascota>> PrimerMascota()
        {
            return await dbContext.Mascotas.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Mascota>> Get(int id)
        {
            var mascota = await dbContext.Mascotas.FirstOrDefaultAsync(x =>x.Id == id);
            
            if (mascota == null)
            {
                return NotFound();
            }

            return mascota;
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Mascota>> GetNombre(String nombre)
        {
            var mascota = await dbContext.Mascotas.FirstOrDefaultAsync(x => x.Nombre == nombre);

            if (mascota == null)
            {
                return NotFound();
            }

            return mascota;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Mascota mascota)
        {
            var ExisteDueño = await dbContext.Dueños.AnyAsync(x => x.Id == mascota.DueñoId);
            
            if (!ExisteDueño)
            {
                return BadRequest($"No existe el dueño con el id:");
            }
            dbContext.Add(mascota);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Mascota mascota, int id)
        {
            var exist = await dbContext.Mascotas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("La mascota solicitada no existe");
            }
            if (mascota.Id != id)
            {
                return BadRequest("El id de la mascota no coincide con el establecido en la url");
            }
            dbContext.Update(mascota);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Mascotas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no fue encontrado");
            }
            dbContext.Remove(new Mascota()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
