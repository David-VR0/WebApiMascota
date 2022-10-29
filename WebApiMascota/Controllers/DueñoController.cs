using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMascota.Entidades;
using WebApiMascota.Services;
using WebApiMascota.Filtros;

namespace WebApiMascota.Controllers
{

    [ApiController]
    [Route("api/Dueños")]
    public class DueñoController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<DueñoController> logger;
        private readonly IWebHostEnvironment env;
        private readonly string nuevosRegistros = "nuevosRegistros.txt";
        private readonly string registrosConsultados = "registrosConsultados.txt";
        public DueñoController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<DueñoController> logger,
            IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            throw new NotImplementedException();
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                DueñosControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                DueñosControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                DueñosControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet]
         [HttpGet("listado")]
         [HttpGet("/listado")]
         public async Task<ActionResult<List<Dueño>>> GetDueños(){

            //throw new NotImplementedException();
            logger.LogInformation("Se obtiene el listado de dueños");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();

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
                logger.LogError("No se encuentra el deuño. ");
                return NotFound();
            }

            var ruta = $@"{env.ContentRootPath}\wwwroot\{registrosConsultados}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(dueño.Id + " " + dueño.Nombre); }

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
