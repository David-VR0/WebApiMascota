using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApiMascota;
using WebApiMascota.Services;
using WebApiMascota.Middlewares;
using WebApiMascota.Filtros;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace WebApiMarcota
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Se encarga de configurar ApplicationDbContext como un servicio
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddTransient<IService, ServiceA>();
            services.AddTransient<ServiceTransient>();
            services.AddScoped<ServiceScoped>();
            services.AddSingleton<ServiceSingleton>();
            services.AddTransient<FiltroDeAccion>();
            services.AddHostedService<EscribirEnArchivo>();
            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiMascotas", Version = "v1" });
            });
        }
        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseResponseHttpMiddleware();

            app.Map("/maping", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Interceptando las peticiones");
                });
            });


            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
