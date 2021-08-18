using FSWDFinalProject.UI.MVC.Utilities;
using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;

namespace FSWDFinalProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet] //Get - sends the view to the browser - this view contains our form
        //Upon form submission, the form requests the UploadPost action
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPost(HttpPostedFileBase myImage)
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AdminMenu()
        {

            return View();
        }          
    }
}
