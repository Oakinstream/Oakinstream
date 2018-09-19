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
    public class ProjectCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectCategory
        public ActionResult Index()
        {
            return View(db.ProjectCategoryModels.ToList());
        }

        // GET: ProjectCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategoryModels projectCategoryModels = db.ProjectCategoryModels.Find(id);
            if (projectCategoryModels == null)
            {
                return HttpNotFound();
            }
            return View(projectCategoryModels);
        }

        // GET: ProjectCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] ProjectCategoryModels projectCategoryModels)
        {
            if (ModelState.IsValid)
            {
                db.ProjectCategoryModels.Add(projectCategoryModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectCategoryModels);
        }

        // GET: ProjectCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategoryModels projectCategoryModels = db.ProjectCategoryModels.Find(id);
            if (projectCategoryModels == null)
            {
                return HttpNotFound();
            }
            return View(projectCategoryModels);
        }

        // POST: ProjectCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] ProjectCategoryModels projectCategoryModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectCategoryModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectCategoryModels);
        }

        // GET: ProjectCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategoryModels projectCategoryModels = db.ProjectCategoryModels.Find(id);
            if (projectCategoryModels == null)
            {
                return HttpNotFound();
            }
            return View(projectCategoryModels);
        }

        // POST: ProjectCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectCategoryModels projectCategoryModels = db.ProjectCategoryModels.Find(id);
            db.ProjectCategoryModels.Remove(projectCategoryModels);
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
