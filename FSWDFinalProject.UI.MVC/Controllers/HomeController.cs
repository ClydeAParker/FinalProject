﻿using System.Web.Mvc;

namespace FSWDFinalProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
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

        [HttpGet]
        [Authorize]
        public ActionResult FranchiseMenu()
        {
            return View();
        }        
    }
}
