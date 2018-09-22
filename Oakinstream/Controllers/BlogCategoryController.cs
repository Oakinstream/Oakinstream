using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Oakinstream.Models;

namespace Oakinstream.Controllers
{
    public class BlogCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogCategory
        public ActionResult Index()
        {
            return View(db.BlogCategoryModels.ToList());
        }

        // GET: BlogCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategoryModels blogCategoryModels = db.BlogCategoryModels.Find(id);
            if (blogCategoryModels == null)
            {
                return HttpNotFound();
            }
            return View(blogCategoryModels);
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
        public ActionResult Create([Bind(Include = "ID,Name")] BlogCategoryModels blogCategoryModels)
        {
            if (ModelState.IsValid)
            {
                db.BlogCategoryModels.Add(blogCategoryModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogCategoryModels);
        }

        // GET: BlogCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategoryModels blogCategoryModels = db.BlogCategoryModels.Find(id);
            if (blogCategoryModels == null)
            {
                return HttpNotFound();
            }
            return View(blogCategoryModels);
        }

        // POST: BlogCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] BlogCategoryModels blogCategoryModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogCategoryModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogCategoryModels);
        }

        // GET: BlogCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategoryModels blogCategoryModels = db.BlogCategoryModels.Find(id);
            if (blogCategoryModels == null)
            {
                return HttpNotFound();
            }
            return View(blogCategoryModels);
        }

        // POST: BlogCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogCategoryModels blogCategoryModels = db.BlogCategoryModels.Find(id);
            db.BlogCategoryModels.Remove(blogCategoryModels);
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
