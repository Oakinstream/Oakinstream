using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Oakinstream.DAL;
using Oakinstream.Models;

namespace Oakinstream.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (User.Identity.GetUserId() != null)
            {
                var userId = User.Identity.GetUserId();
                var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().ID;
                var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(userId);
                ViewBag.Pin = user.Pin;
            }
            return View();
        }
    }
}