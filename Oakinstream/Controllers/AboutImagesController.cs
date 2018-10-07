using System;
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
    public class AboutImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AboutImages
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.AboutImages.ToList());
        }


        // GET: AboutImages/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Upload()
        {
            return View();
        }

        // POST: AboutImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            if (files[0] != null)
            {
                if (files.Length <= 10)
                {
                    foreach (var file in files)
                    {
                        if (!ValidateImageFile(file))
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
                                SaveImageToDisk(file);
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
                    var aboutToAdd = new AboutImage { FileName = file.FileName };
                    try
                    {
                        db.AboutImages.Add(aboutToAdd);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        SqlException innerException = e.InnerException.InnerException as SqlException;
                        if (innerException != null && innerException.Number == 2601)
                        {
                            duplicateFiles += file.FileName + " ";
                            duplicates = true;
                            db.Entry(aboutToAdd).State = EntityState.Detached;
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

        // GET: AboutImages/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutImage aboutImage = db.AboutImages.Find(id);
            if (aboutImage == null)
            {
                return HttpNotFound();
            }
            return View(aboutImage);
        }

        // POST: AboutImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            AboutImage aboutImage = db.AboutImages.Find(id);
            db.AboutImages.Remove(aboutImage);
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

        #region IMAGES
        private bool ValidateImageFile(HttpPostedFileBase file)
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

        private void SaveImageToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width < Constants.AboutImageMinWidth)
            {
                throw new BadImageFormatException();
            }

            if (img.Width > Constants.AboutImageMaxWidth)
            {
                img.Resize(Constants.AboutImageMaxWidth, img.Height);
            }
            img.Save(Constants.AboutImagePath + file.FileName);

            if (img.Width > Constants.AboutThumbnailMaxWidth)
            {
                img.Resize(Constants.AboutThumbnailMaxWidth, img.Height);
            }
            img.Save(Constants.AboutThumbnailPath + file.FileName);
        }
        #endregion
    }
}
