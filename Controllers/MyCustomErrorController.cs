using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Controllers
{
    public class MyCustomErrorController : Controller
    {
        // GET: MyCustomErrorController
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult PageNotFound()
        {
            return View();
        }
        public ViewResult UnAuthorized() //When not authorzed
        {
            return View();
        }
    }
}