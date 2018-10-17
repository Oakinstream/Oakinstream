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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.Homes.ToList());
        }

        // GET: Home/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.ImageLists = new List<SelectList>();
            foreach (var imageMapping in home.HomeImagesMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.HomeImages, "ID", "FileName", imageMapping.HomeImageID));
            }
            for (int i = 0; i < Constants.NumberOfHomeImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.HomeImages, "ID", "FileName"));
            }

            viewModel.ID = home.ID;
            viewModel.Title = home.Title;
            viewModel.Info1 = home.Info1;
            viewModel.Info2 = home.Info2;
            viewModel.Info3 = home.Info3;

            viewModel.CreatedDate = home.CreatedDate;
            viewModel.CreatedBy = home.CreatedBy;

            return View(viewModel);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(HomeViewModel viewModel)
        {
            var homeToUpdate = db.Homes.Include(p => p.HomeImagesMappings).Where(p => p.ID == viewModel.ID).Single();
            if (TryUpdateModel(homeToUpdate, "", new string[] { "Title", "Info1", "Info2", "Info3", "UpdatedBy", "UpdatedDate" }))
            {
                if (homeToUpdate.HomeImagesMappings == null)
                {
                    homeToUpdate.HomeImagesMappings = new List<HomeImageMapping>();
                }
                string[] homeimages = viewModel.HomeImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < homeimages.Length; i++)
                {
                    var imageMappingToEdit = homeToUpdate.HomeImagesMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    var image = db.HomeImages.Find(int.Parse(homeimages[i]));
                    if (imageMappingToEdit == null)
                    {
                        homeToUpdate.HomeImagesMappings.Add(new HomeImageMapping
                        {
                            ImageNumber = i,
                            HomeImage = image,
                            HomeImageID = image.ID
                        });
                    }
                    else
                    {
                        if (imageMappingToEdit.HomeImageID != int.Parse(homeimages[i]))
                        {
                            imageMappingToEdit.HomeImage = image;
                        }
                    }
                }

                for (int i = homeimages.Length; i < Constants.NumberOfHomeImages; i++)
                {
                    var imageMappingToEdit = homeToUpdate.HomeImagesMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    if (imageMappingToEdit != null)
                    {
                        db.HomeImageMappings.Remove(imageMappingToEdit);
                    }
                }

                homeToUpdate.UpdatedBy = User.Identity.Name;
                homeToUpdate.UpdatedDate = DateTime.Now;

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
