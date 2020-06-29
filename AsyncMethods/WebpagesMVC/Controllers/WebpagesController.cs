using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        
    }
}