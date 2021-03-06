﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Oakinstream.Models;

namespace Oakinstream.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HomeImages
        public ActionResult Index()
        {
            return View(db.HomeImages.ToList());
        }

        // GET: HomeImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: HomeImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "ImageHeader,ImageCaption,ImageLink")] HomeImage homeimage, HttpPostedFileBase[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            if (files[0] != null)
            {
                if (files.Length <= 10)
                {
                    foreach (var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles += file.FileName + " ";
                        }
                    }
                    if (allValid)
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                SaveToDisk(file);
                            }
                            catch (BadImageFormatException bife)
                            {
                                ModelState.AddModelError("FileName",
                                    file.FileName + " was to small, must be at least 200px wide.");
                            }
                            catch (Exception e)
                            {
                                ModelState.AddModelError("FileName",
                                    "An error occured while saving files to disk!");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("FileName",
                            "All files must be gif, jpg or png and less than 2MB. " +
                            "The following files are not valid: " + inValidFiles);
                    }
                }
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to ten images at once.");
                }
            }
            else
            {
                ModelState.AddModelError("FileName", "Please choose a file.");
            }
            if (ModelState.IsValid)
            {
                bool duplicates = false;
                bool otherDbError = false;
                string duplicateFiles = "";

                foreach (var file in files)
                {
                    var imageToAdd = new HomeImage { FileName = file.FileName, ImageHeader = homeimage.ImageHeader, ImageCaption = homeimage.ImageCaption, ImageLink = homeimage.ImageLink };
                    try
                    {
                        db.HomeImages.Add(imageToAdd);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        SqlException innerException = e.InnerException.InnerException as SqlException;
                        if (innerException != null && innerException.Number == 2601)
                        {
                            duplicateFiles += file.FileName + " ";
                            duplicates = true;
                            db.Entry(imageToAdd).State = EntityState.Detached;
                        }
                        else
                        {
                            otherDbError = true;
                        }
                    }
                }
                if (duplicates)
                {
                    ModelState.AddModelError("FileName",
                        "All files uploaded except " + duplicateFiles + ". Please delete and" +
                        "upload again if you wish to re-add them.");
                    return View();
                }
                else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Error occured while " +
                        "saving to database. Please try again.");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        
        // GET: HomeImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeImage homeImage = db.HomeImages.Find(id);
            if (homeImage == null)
            {
                return HttpNotFound();
            }
            return View(homeImage);
        }

        // POST: HomeImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HomeImage homeimage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homeimage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(homeimage);
        }
        
        // GET: HomeImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeImage homeImage = db.HomeImages.Find(id);
            if (homeImage == null)
            {
                return HttpNotFound();
            }
            return View(homeImage);
        }

        // POST: HomeImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HomeImage homeImage = db.HomeImages.Find(id);
            var mappings = homeImage.HomeImageMappings.Where(pim => pim.HomeImageID == id);
            foreach (var mapping in mappings)
            {
                var mappingsToUpdate = db.HomeImageMappings.Where(pim => pim.HomeID == mapping.HomeID);
                foreach (var mappingToUpdate in mappingsToUpdate)
                {
                    if (mappingToUpdate.ImageNumber > mapping.ImageNumber)
                    {
                        mappingToUpdate.ImageNumber--;
                    }
                }
            }
            System.IO.File.Delete(Request.MapPath(Constants.HomeImagePath + homeImage.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.HomeThumbnailPath + homeImage.FileName));
            db.HomeImages.Remove(homeImage);
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

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string[] allowedFileTypes = { ".gif", ".jpg", ".jpeg", ".png" };
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            if (allowedFileTypes.Contains(fileExtension))
            {
                if (file.ContentLength > 0 && file.ContentLength < 2097152)
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width < Constants.HomeImageMinWidth)
            {
                throw new BadImageFormatException();
            }
            if (img.Width > Constants.HomeImageMaxWidth)
            {
                img.Resize(Constants.HomeImageMaxWidth, img.Height);
            }
            img.Save(Constants.HomeImagePath + file.FileName);

            if (img.Width > Constants.HomeThumbnailMaxWidth)
            {
                img.Resize(Constants.HomeThumbnailMaxWidth, img.Height);
            }

            img.Save(Constants.HomeThumbnailPath + file.FileName);
        }
    }
}
