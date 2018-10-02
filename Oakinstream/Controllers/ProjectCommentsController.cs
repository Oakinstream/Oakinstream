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
    public class ProjectCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectComments
        public ActionResult Index()
        {
            return View(db.ProjectComments.ToList());
        }

        // GET: ProjectComments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Comment,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] ProjectComment projectComment)
        {
            projectComment.CreatedDate = DateTime.Now;
            projectComment.CreatedBy = User.Identity.Name;
            projectComment.UpdatedDate = DateTime.Now;
            projectComment.UpdatedBy = null;

            if (ModelState.IsValid)
            {
                db.ProjectComments.Add(projectComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectComment);
        }

        // GET: ProjectComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectComment projectComment = db.ProjectComments.Find(id);
            if (projectComment == null)
            {
                return HttpNotFound();
            }
            return View(projectComment);
        }

        // POST: ProjectComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Comment,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] ProjectComment projectComment)
        {
            projectComment.UpdatedBy = User.Identity.Name;
            projectComment.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(projectComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectComment);
        }

        // GET: ProjectComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectComment projectComment = db.ProjectComments.Find(id);
            if (projectComment == null)
            {
                return HttpNotFound();
            }
            return View(projectComment);
        }

        // POST: ProjectComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectComment projectComment = db.ProjectComments.Find(id);
            db.ProjectComments.Remove(projectComment);
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
