using Microsoft.AspNetCore.Mvc;
using WebApiMascota.Controllers;
using WebApiMascota.Entidades;

namespace WebApiMascota.Services
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;

        private readonly string nombreArchivo = "Archivo001.txt";
        // private readonly string archivo = "ListadoDueños.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Se ejecuta cuando cargamos la aplicacion 1 vez
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Se ejecuta cuando detenemos la aplicacion aunque puede que no se ejecute por algun error. 
            timer.Dispose();
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecucion: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            //GuardarDueños();
        }
        private void Escribir(string msg)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";

            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg); }
        }

        private void GuardarDueños()
        {
            //var ruta = $@"{env.ContentRootPath}\wwwroot\{archivo}";
            //ActionResult task = alumnosController.ObtenerGuid();
            //object Alumno = task.Result.Value;
            //using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(Alumno); }
        }
    }
}