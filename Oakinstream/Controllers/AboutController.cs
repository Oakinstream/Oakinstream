using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Oakinstream.Models;
using Oakinstream.ViewModels;

namespace Oakinstream.Controllers
{
    public class AboutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: About
        public ActionResult Index()
        {
            var abouts = db.Abouts.Include(a => a.AboutImage);
            return View(abouts.ToList());
        }

        // GET: About/Create
        public ActionResult Create()
        {
            AboutViewModel viewModel = new AboutViewModel();
            viewModel.AboutImageList = new SelectList(db.AboutImages, "ID", "FileName");
            viewModel.FileList = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfAboutFiles; i++)
            {
                viewModel.FileList.Add(new SelectList(db.AboutFiles, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: About/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AboutViewModel viewModel)
        {
            About about = new About();
            about.Name = viewModel.Name;
            about.AboutImageID = viewModel.AboutImageID;
            about.Age = viewModel.Age;
            about.Info1 = viewModel.Info1;
            about.Info2 = viewModel.Info2;
            about.Info3 = viewModel.Info3;
            about.AboutFileMappings = new List<AboutFileMapping>();
            string[] aboutFiles = viewModel.AboutFiles.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < aboutFiles.Length; i++)
            {
                about.AboutFileMappings.Add(new AboutFileMapping()
                {
                    AboutFile = db.AboutFiles.Find(int.Parse(aboutFiles[i])),
                    FileNumber = i
                });
            }
            about.CreatedDate = DateTime.Now;
            about.CreatedBy = User.Identity.Name;
            about.UpdatedDate = DateTime.Now;
            about.UpdatedBy = null;

            if (ModelState.IsValid)
            {
                db.Abouts.Add(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.AboutImageList = new SelectList(db.ProjectImages, "ID", "FileName", about.AboutImageID);
            viewModel.FileList = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfAboutFiles; i++)
            {
                viewModel.FileList.Add(new SelectList(db.AboutFiles, "ID", "FileName",
                viewModel.AboutFiles[i]));
            }
            return View(viewModel);
        }

        // GET: About/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            AboutViewModel viewModel = new AboutViewModel();
            viewModel.AboutImageList = new SelectList(db.AboutImages, "ID", "FileName", about.AboutImageID);
            viewModel.FileList = new List<SelectList>();
            foreach (var imageMapping in about.AboutFileMappings.OrderBy(pim => pim.FileNumber))
            {
                viewModel.FileList.Add(new SelectList(db.AboutFiles, "ID", "FileName", imageMapping.AboutFileID));
            }
            for (int i = viewModel.FileList.Count; i < Constants.NumberOfAboutFiles; i++)
            {
                viewModel.FileList.Add(new SelectList(db.AboutFiles, "ID", "FileName"));
            }

            viewModel.ID = about.ID;
            viewModel.Name = about.Name;
            viewModel.Age = about.Age;
            viewModel.Info1 = about.Info1;
            viewModel.Info2 = about.Info2;
            viewModel.Info3 = about.Info3;
            viewModel.CreatedDate = about.CreatedDate;
            viewModel.CreatedBy = about.CreatedBy;

            return View(viewModel);
        }

        // POST: About/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AboutViewModel viewModel)
        {
            var aboutToUpdate = db.Abouts.Include(p => p.AboutFileMappings).
                Where(p => p.ID == viewModel.ID).Single();
            if (TryUpdateModel(aboutToUpdate, "", new string[] { "Name", "Info1", "Info2", "Info3", "Age", "UpdatedBy", "UpdatedDate" }))
            {
                if (aboutToUpdate.AboutFileMappings == null)
                {
                    aboutToUpdate.AboutFileMappings = new List<AboutFileMapping>();
                }
                string[] aboutfiles = viewModel.AboutFiles.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < aboutfiles.Length; i++)
                {
                    var imageMappingToEdit = aboutToUpdate.AboutFileMappings.Where(pim => pim.FileNumber == i).FirstOrDefault();
                    var file = db.AboutFiles.Find(int.Parse(aboutfiles[i]));
                    if (imageMappingToEdit == null)
                    {
                        aboutToUpdate.AboutFileMappings.Add(new AboutFileMapping
                        {
                            FileNumber = i,
                            AboutFile = file,
                            AboutFileID = file.ID
                        });
                    }
                    else
                    {
                        if (imageMappingToEdit.AboutFileID != int.Parse(aboutfiles[i]))
                        {
                            imageMappingToEdit.AboutFile = file;
                        }
                    }
                }

                for (int i = aboutfiles.Length; i < Constants.NumberOfAboutFiles; i++)
                {
                    var imageMappingToEdit = aboutToUpdate.AboutFileMappings
                        .Where(pim => pim.FileNumber == i).FirstOrDefault();
                    if (imageMappingToEdit != null)
                    {
                        db.AboutFileMappings.Remove(imageMappingToEdit);
                    }
                }

                aboutToUpdate.UpdatedBy = User.Identity.Name;
                aboutToUpdate.UpdatedDate = DateTime.Now;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: About/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: About/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            About about = db.Abouts.Find(id);
            db.Abouts.Remove(about);
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
