using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMascota.Entidades;

namespace WebApiMascota.Controllers
{

    [ApiController]
    [Route("api/Dueños")]
    public class DueñoController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        public DueñoController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        
         [HttpGet]
         [HttpGet("listado")]
         [HttpGet("/listado")]
         public async Task<ActionResult<List<Dueño>>> GetDueños(){
            return await dbContext.Dueños.Include( x => x.mascotas).ToListAsync();
         }
        [HttpGet("primero")]
        public async Task<ActionResult<Dueño>> PrimerDueño()
        {
            return await dbContext.Dueños.Include(x => x.mascotas).FirstOrDefaultAsync();
        }
        [HttpGet("primero2")]
        public ActionResult<Dueño> PrimerDueñoD()
        {
            return new Dueño() { Nombre = "DOS"};
        }

        [HttpGet("{id:int}/{param}")]
        public async Task<ActionResult<Dueño>> Get(int id, string param)
        {
            var mascota = await dbContext.Dueños.Include(x => x.mascotas).FirstOrDefaultAsync(x => x.Id == id);

            if (mascota == null)
            {
                return NotFound();
            }

            return mascota;
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Dueño>> GetNombre(String nombre)
        {
            var dueño = await dbContext.Dueños.FirstOrDefaultAsync(x => x.Nombre == nombre);

            if (dueño == null)
            {
                return NotFound();
            }

            return dueño;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Dueño dueño)
        {
            dbContext.Add(dueño);
            await dbContext.SaveChangesAsync();
            return Ok();
        } 
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Dueño dueño, int id)
        {
            var exist = await dbContext.Dueños.AnyAsync(x => x.Id == id);
            if (!exist)
             {
                 return NotFound();
             }
            if (dueño.Id != id)
            {
               return BadRequest("El id del dueño no coincide con el establecido en la url");
            }

            dbContext.Update(dueño);
            await dbContext.SaveChangesAsync();
             return Ok();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            {
                var exist = await dbContext.Dueños.AnyAsync(x => x.Id == id);
                if (!exist)
                    {
                        return NotFound("El recurso no fue encontrado");
                    }
                dbContext.Remove(new Dueño()
                    {
                        Id = id
                    });
                await dbContext.SaveChangesAsync();
                return Ok();
        }
    }
}
