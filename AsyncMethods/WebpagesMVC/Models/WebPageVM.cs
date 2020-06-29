using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidades;

namespace WebpagesMVC.Models
{
    public class WebPageVM
    {
        public WebPageVM()
        {
            //Cargar en memoria una lista de webpages a buscar
            this.WebPages = new List<WebPage>()
            {
                new WebPage(){Nombre="Google", URL="https://www.google.com.ar" },
                new WebPage(){Nombre="Infobae", URL="https://www.infobae.com.ar" },
                new WebPage(){Nombre="Twitter", URL="https://www.twitter.com" },
                new WebPage(){Nombre="Drive", URL="https://www.google.com.ar" },
                new WebPage(){Nombre="C-Sharp Corner", URL="https://www.c-sharpcorner.com" }
            };


        }

        public List<WebPage> WebPages { get; set; }

    }
}