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
         public async Task<ActionResult<List<Dueño>>> Get(){
            return await dbContext.Dueños.Include( x => x.mascotas).ToListAsync();
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
