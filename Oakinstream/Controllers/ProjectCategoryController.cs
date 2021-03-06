﻿using System;
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
    public class ProjectCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectCategory
        public ActionResult Index()
        {
            return View(db.ProjectCategorys.ToList());
        }

        // GET: ProjectCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategory projectCategoryModels = db.ProjectCategorys.Find(id);
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
        public ActionResult Create([Bind(Include = "ID,Name")] ProjectCategory projectCategoryModels)
        {
            if (ModelState.IsValid)
            {
                db.ProjectCategorys.Add(projectCategoryModels);
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
            ProjectCategory projectCategoryModels = db.ProjectCategorys.Find(id);
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
        public ActionResult Edit([Bind(Include = "ID,Name")] ProjectCategory projectCategoryModels)
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
            ProjectCategory projectCategoryModels = db.ProjectCategorys.Find(id);
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
            ProjectCategory projectCategoryModels = db.ProjectCategorys.Find(id);
            db.ProjectCategorys.Remove(projectCategoryModels);
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
