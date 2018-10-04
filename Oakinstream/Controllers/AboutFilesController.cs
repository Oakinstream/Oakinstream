using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Oakinstream.Models;

namespace Oakinstream.Controllers
{
    public class AboutFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AboutFiles
        public ActionResult Index()
        {
            return View(db.AboutFiles.ToList());
        }

        // GET: AboutFiles/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: AboutFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] Files)
        {
            bool allFilesIsVailid = true;
            string inValidFiles = "";

            if (Files[0] != null)
            {
                if (Files.Length <= 8)
                {
                    foreach (var file in Files)
                    {
                        if (!ValidateFile(file))
                        {
                            allFilesIsVailid = false;
                            inValidFiles += file.FileName + " ";
                        }
                    }

                    if (allFilesIsVailid)
                    {
                        foreach (var file in Files)
                        {
                            try
                            {
                                SaveToDisk(file);
                            }
                            catch (Exception e)
                            {
                                ModelState.AddModelError("Filename",
                                    "An error occured while saving the files to disk! " + e.Message);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("FileName",
                            "All files must be pdf, odt or txt and less than" + Constants.MaxFileSizeMB + "MB" +
                            "The following files are not valid: " + inValidFiles);
                    }

                }
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to eight images at once.");
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

                foreach (var file in Files)
                {
                    var fileToAdd = new AboutFile { FileName = file.FileName };
                    try
                    {
                        db.AboutFiles.Add(fileToAdd);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        SqlException innerException = e.InnerException.InnerException as SqlException;
                        if (innerException != null && innerException.Number == 2601)
                        {
                            duplicateFiles += file.FileName + " ";
                            duplicates = true;
                            db.Entry(fileToAdd).State = EntityState.Detached;
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


        // GET: AboutFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutFile aboutFile = db.AboutFiles.Find(id);
            if (aboutFile == null)
            {
                return HttpNotFound();
            }
            return View(aboutFile);
        }

        // POST: AboutFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AboutFile aboutFile = db.AboutFiles.Find(id);
            var mappings = aboutFile.AboutFileMappings.Where(pim => pim.AboutFileID == id);
            foreach (var mapping in mappings)
            {
                var mappingsToUpdate = db.AboutFileMappings.Where(pim => pim.AboutID == mapping.AboutID);
                foreach (var mappingToUpdate in mappingsToUpdate)
                {
                    if (mappingToUpdate.FileNumber > mapping.FileNumber)
                    {
                        mappingToUpdate.FileNumber--;
                    }
                }
            }
            System.IO.File.Delete(Request.MapPath(Constants.AboutFilePath + aboutFile.FileName));
            System.IO.File.Delete(Request.MapPath(Constants.AboutThumbnailPath + aboutFile.FileName));
            db.AboutFiles.Remove(aboutFile);
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

        #region DOCFILES
        private bool ValidateFile(HttpPostedFileBase file)
        {
            string[] allowedFileTypes = { ".pdf", ".odt", ".doc", ".txt" };
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

            if (allowedFileTypes.Contains(fileExtension))
            {
                if (file.ContentLength > 0 && file.ContentLength < Constants.MegabytesToBytes(Constants.MaxFileSizeMB))
                {
                    return true;
                }
            }

            return false;
        }

        private void SaveToDisk(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath(Constants.AboutFilePath + file.FileName));
        }
        #endregion
    }
}
