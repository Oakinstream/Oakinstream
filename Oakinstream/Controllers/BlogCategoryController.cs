using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Oakinstream.DAL;
using Oakinstream.Models;

namespace Oakinstream.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogCategory
        public ActionResult Index()
        {
            return View(db.BlogCategorys.ToList());
        }

        // GET: BlogCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] BlogCategory blogCategory)
        {
            if (ModelState.IsValid)
            {
                db.BlogCategorys.Add(blogCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogCategory);
        }

        // GET: BlogCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategory blogCategory = db.BlogCategorys.Find(id);
            if (blogCategory == null)
            {
                return HttpNotFound();
            }
            return View(blogCategory);
        }

        // POST: BlogCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogCategory blogCategory= db.BlogCategorys.Find(id);
            db.BlogCategorys.Remove(blogCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
