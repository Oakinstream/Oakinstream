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

        // GET: About/Edit/5
        public ActionResult Edit(int? id)
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
            if (TryUpdateModel(aboutToUpdate, "", new string[] { "Name", "AboutImageID","Info1", "Info2", "Info3", "Age", "UpdatedBy", "UpdatedDate" }))
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

                if (aboutToUpdate.Name == null)
                {
                    aboutToUpdate.Name = "[Enter a Name]";
                }

                if (aboutToUpdate.Age == null)
                {
                    aboutToUpdate.Age = 1337;
                }

                if (aboutToUpdate.Info1 == null)
                {
                    aboutToUpdate.Info1 = "No information";
                }

                if (aboutToUpdate.Info2 == null)
                {
                    aboutToUpdate.Info2 = "No information";
                }

                if (aboutToUpdate.Info3 == null)
                {
                    aboutToUpdate.Info3 = "No information";
                }

                aboutToUpdate.UpdatedBy = User.Identity.Name;
                aboutToUpdate.UpdatedDate = DateTime.Now;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
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
