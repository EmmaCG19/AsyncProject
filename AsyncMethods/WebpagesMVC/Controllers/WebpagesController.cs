using Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WebpagesMVC.Models;

namespace WebpagesMVC.Controllers
{
    public class WebpagesController : Controller
    {
        [HttpGet]
        public ActionResult ObtenerSitiosWeb()
        {
            WebPageVM model = new WebPageVM();
            ViewBag.Title = "Lista de websites";

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerSitiosWeb(WebPageVM model, string cargaAsync, string cargaSync)
        {
            //ViewBag.Resultados = "";

            if (ModelState.IsValid)
            {
                
                if (cargaAsync != null)
                {
                    ViewBag.Resultados = await RunAsyncParallel(model.WebPages);
                }
                else if (cargaSync != null)
                {
                    ViewBag.Resultados = RunSync(model.WebPages);
                }
                //RedirectToAction("ObtenerSitiosWeb", "WebPages");
            }

            return View(model);
         }

        /// <summary>
        /// Ejecuta asincronicamente la descarga de la informacion de las páginas
        /// </summary>
        /// <param name="websites"></param>
        /// <returns></returns>
        private async Task<string> RunAsync(List<WebPage> websites)
        {
            Stopwatch watch = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            watch.Start();

            foreach (var website in websites)
            {
                WebPage result = await Task.Run(() => DownloadWebsite(website));
                sb.AppendLine(ReportWebsite(result));
            }

            watch.Stop();
            sb.AppendLine($"Execution time: {watch.ElapsedMilliseconds} ms");

            return sb.ToString();

        }

        /// <summary>
        /// Realizar de manera asincronica y paralela la descarga de los sitios web
        /// </summary>
        /// <param name="websites"></param>
        /// <returns></returns>
        private async Task<string> RunAsyncParallel(List<WebPage> websites)
        {
            Stopwatch watch = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            List<Task<WebPage>> tasks = new List<Task<WebPage>>();

            watch.Start();

            //Se agrega la tarea a realizar en una lista de tareas 
            foreach (var website in websites)
            {
                tasks.Add(Task.Run(() => DownloadWebsite(website)));
            }

            //Espero que todas las tareas sean realizadas, obtiene los resultados y avanza
            var results = await Task.WhenAll(tasks);

            //Muestra la informacion y la concatena en el StringBuilder
            foreach (var page in results)
            {
                sb.AppendLine(ReportWebsite(page));
            }


            watch.Stop();
            sb.AppendLine($"Execution time: {watch.ElapsedMilliseconds} ms");
            return sb.ToString();
        }


        /// <summary>
        /// Ejecuta sincronicamente la descarga de la informacion de las páginas
        /// </summary>
        /// <param name="websites"></param>
        /// <returns></returns>
        private string RunSync(List<WebPage> websites)
        {
            Stopwatch watch = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            
            watch.Start();
            foreach (var website in websites)
            {
                this.DownloadWebsite(website);
                sb.AppendLine(ReportWebsite(website));
            }

            watch.Stop();
            sb.AppendLine($"Execution time: {watch.ElapsedMilliseconds} ms");

            return sb.ToString();
        }

        private string ReportWebsite(WebPage webpage) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Website: {webpage.Nombre},");
            sb.Append($"URL: {webpage.URL},");
            sb.Append($"downloaded: {webpage.Data.Length} character.");
            sb.AppendLine();

            return sb.ToString();
        }

        private WebPage DownloadWebsite(WebPage webpage) 
        {
            WebClient client = new WebClient();

            try
            {
                webpage.Data = client.DownloadString(webpage.URL);

            }
            catch (WebException)
            {
                webpage.Data = String.Empty;
            }

            return webpage;
        }


    }
}