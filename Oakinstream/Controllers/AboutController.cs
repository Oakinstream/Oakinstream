using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oakinstream.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Message = "Right now this is a template for someone on github who is interested in this project";

            return View();
        }
    }
}