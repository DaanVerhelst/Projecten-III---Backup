using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Interface;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofAPI.Controllers
{
    public class PDFController : ControllerBase
    {//gitfail
        private readonly ITemplateRepository _templateRepository;
        private readonly IDagRepository _dagRepository;
        private Dag dag;
        public PDFController(ITemplateRepository tempRepo, IDagRepository dagRepo)
        {
            _templateRepository = tempRepo;
            _dagRepository = dagRepo;
        }

        [Route("createAndReturnPDF/{date}/{dagNummer}/{weekNummer}")]
        [HttpGet]
        public IActionResult createAndReturnPDF(DateTime date , int dagNummer, int weekNummer)
        {
            var data = _templateRepository.GeefTemplate(dagNummer, weekNummer);
            var day = _dagRepository.GetDagByDay(date);
           // dag = _dagRepository.GetDagByDay(_templateRepository)
            if (data == null)
            {
                return null;
            }
            else
            {

                var Renderer = new IronPdf.HtmlToPdf();
                String dag = "";
                switch (date.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        dag = "Maandag";
                        break;
                    case System.DayOfWeek.Tuesday:
                        dag = "Dinsdag";
                        break;
                    case System.DayOfWeek.Wednesday:
                        dag = "Woensdag";
                        break;
                    case System.DayOfWeek.Thursday:
                        dag = "Donderdag";
                        break;
                    case System.DayOfWeek.Friday:
                        dag = "Vrijdag";
                        break;
                    case System.DayOfWeek.Saturday:
                        dag = "Zaterdag";// weekend zou niet moeten en gaat niet gebruikt worden maar zou er toch ooit een weekend dag zijn waar ze iets doen gaat dan green probleem zijn
                        break;
                    case System.DayOfWeek.Sunday:
                        dag = "Zondag";
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                if (day != null)
                {
                    string html = "<h1 style=\"border:3px; border-style:solid; border-color:black; padding: 1em;\">" + dag + " (Week" + data.WeekNR + ")" + "</h1></br>" +
                        "<h2>Stagiaires/vrijwilliger:</h2><p>" +
                        persoonNaarString(day.GeefStagair()) +
                        persoonNaarString(day.GeefVrijwilligers()) +
                        "</p></br>" +
                        "<p style=\"border:3px; border-style:solid; border-color:black; padding: 1em; background-color: grey\">" +
                        "</br>AFWEZIG:" +
                        persoonNaarString(day.GeefAfwezigen()) +
                        "</br>ZIEK:" +
                        persoonNaarString(day.GeefAfwezigen()) +
                        "</br>VERVOER:" +
                        "" +
                        "</p>" +
                        "<div></br>" +
                        "<p>VOORMIDDAG:</p></br>" +
                       maakMoment(data.GeefAteliersVM()) +
                        "</div>" +
                       "<div><p>  NAMIDDAG: </p>" +
                       //maakMoment(data.GeefAteliersNM()) +
                       "</div></br> " +
                        "<p>VOORBEREIDING:</p>" + "" +
                        "<p>KINE:</p></br>" +
                        Commentaar(day)
                       ;

                    var PDF = Renderer.RenderHtmlAsPdf(html);
                    String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Templates";

                    if (!Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    var OutputPath = (path + "\\Template.pdf").ToString();
                    PDF.SaveAs(OutputPath);

                    var dataBytes = System.IO.File.ReadAllBytes(OutputPath);

                    var dataStream = new MemoryStream(dataBytes);

                }



            }
           // return File(PDF, path, "Templatepdf");

            
            return null;
        }
        public String maakMoment(List<Atelier_Dag> ad)
        {
            var s = "";
            foreach (Atelier_Dag at in ad)
            {//apart maken en zetten 
                s += "<p style=\" text-decoration: underline\">";
                s += at.Atelier.Naam;
                s += "</p>";
                s += "<p>";
                foreach (Begeleider b in at.Begeleiders)
                {
                    s += b.Voornaam;
                }
                foreach (Client c in at.Clienten)
                {
                    s += c.Voornaam;
                }
                s += "</p></br>";


            }

            return s;
        }
        public String persoonNaarString(IEnumerable<Persoon> pers)
        {
            var s = "";
            foreach(Persoon p in pers)
            {
                s += p.Voornaam + " , ";
            }
            return s;
            
        }
        public String Commentaar(Dag day)
        {
            if(day.getNotietieBlock()!= null)
            {
                var s = "";
                foreach (NotiteLijsten n in day.getNotietieBlock().LijstNoties)
                {
                    s += "<p>";
                    s += n.Comment;
                    s += "</p>";
                }
                return s;
            }
            else
            {
                return "";
            }
            

        }

    }
}