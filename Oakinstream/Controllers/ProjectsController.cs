using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Oakinstream.Models;
using Oakinstream.DAL;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using Oakinstream.ViewModels;

namespace Oakinstream.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index()
        {
            var project = db.Projects.Include(p => p.ProjectCategory);
            return View(project.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ProjectViewModel viewModel = new ProjectViewModel();
            viewModel.ProjectCategoryList = new SelectList(db.BlogCategorys, "ID", "Name");
            viewModel.FileList = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProjectFiles; i++)
            {
                viewModel.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectViewModel viewModel, HttpPostedFileBase ImageFile)
        {
            Project project = new Project();
            project.Name = viewModel.Name;
            project.ProjectCategoryID = viewModel.ProjectCategoryID;
            project.Description = viewModel.Description;
            project.Date = viewModel.Date;
            project.ProjectFileMappings = new List<ProjectFileMapping>();
            string[] projectFiles  = viewModel.ProjectFiles.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < projectFiles.Length; i++)
            {
                project.ProjectFileMappings.Add(new ProjectFileMapping()
                {
                    ProjectFile = db.ProjectFiles.Find(int.Parse(projectFiles[i])),
                    FileNumber = i
                });
            }
            project.CreatedDate = DateTime.Now;
            project.CreatedBy = viewModel.CreatedBy;
            project.UpdatedDate = DateTime.Now;
            project.UpdatedBy = viewModel.CreatedBy;

            if (ImageFile != null)
            {
                if (ValidateImageFile(ImageFile))
                {
                    try
                    {
                        SaveImageToDisk(ImageFile);
                        project.ProjectImage = ImageFile.FileName;
                    }
                    catch (BadImageFormatException bife)
                    {
                        ModelState.AddModelError("FileName",
                            ImageFile.FileName + " was to small, must be at least 200px wide. [" + bife.Message + "]");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("FileName",
                            "An error occured while saving files to disk! " + e.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("FileName",
                        "All files must be gif, jpg or png and less than 2MB. " +
                        "The following files are not valid: ");
                }
            }
            else
            {
                ModelState.AddModelError("FileName", "Please choose a file.");
            }

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.ProjectCategoryList = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
            viewModel.FileList = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProjectFiles; i++)
            {
                viewModel.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName",
                viewModel.ProjectFiles[i]));
            }
            return View(viewModel);
        }


        /*  // GET: Projects/Edit/5
          public ActionResult Edit(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Project project = db.Projects.Find(id);
              if (project == null)
              {
                  return HttpNotFound();
              }
              project.ProjectCategoryList = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
              project.FileList = new List<SelectList>();
              foreach (var imageMapping in project.ProjectFileMappings.OrderBy(pim => pim.FileNumber))
              {
                  project.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName", imageMapping.ProjectFileID));
              }
              for (int i = project.FileList.Count; i < Constants.NumberOfBlogImages; i++)
              {
                  project.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName"));
              }
              ViewBag.ProjectCategoryID = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
              return View(project);
          }

          // POST: Projects/Edit/5
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
          // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Edit(int ID, string Name, string Description, string ProjectCategoryID, string Date, string CreatedBy, string CreatedDate, string UpdatedBy, HttpPostedFileBase[] ProjectFiles, HttpPostedFileBase ImageFile)
          {

              Project project = new Project();

              project.ID = ID;
              project.Name = Name;
              project.ProjectCategoryID = db.ProjectCategorys.Where(s => s.Name == ProjectCategoryID).Single().ID;
              project.Description = Description;
              project.Date = Date;
              project.ProjectFileMappings = new List<ProjectFileMapping>();
              string[] projectFiles  = project.ProjectFiles.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
              for (int i = 0; i < projectFiles.Length; i++)
              {
                  project.ProjectFileMappings.Add(new ProjectFileMapping()
                  {
                      ProjectFile = db.ProjectFiles.Find(int.Parse(projectFiles[i])),
                      FileNumber = i
                  });
              }
              project.CreatedBy = CreatedBy;
              project.CreatedDate = DateTime.Parse(CreatedDate);
              project.UpdatedBy = UpdatedBy;
              project.UpdatedDate = DateTime.Now;

              if (ImageFile != null)
              {
                  if (ValidateImageFile(ImageFile))
                  {
                      try
                      {
                          if (ModelState.IsValid)
                          {
                              SaveImageToDisk(ImageFile);
                              project.ProjectImage = ImageFile.FileName;
                          }
                      }
                      catch (Exception e)
                      {
                          ModelState.AddModelError("FileName", "An error occured while saving files to disk! " + e.Message);
                      }
                  }
                  else
                  {
                      ModelState.AddModelError("FileName",
                          "All files must be gif, jpg or png and less than 2MB. " +
                          "The following files are not valid: ");
                  }
              }
              else if (!string.IsNullOrEmpty(project.ProjectImage))
              {
                  project.ProjectImage = ImageFile.FileName;
              }

              var projectToUpdate = db.Projects.Include(p => p.ProjectFileMappings).
                  Where(p => p.ID == project.ID).Single();
              if (TryUpdateModel(projectToUpdate, "", new string[] {"Name", "Description", "BlogCategoryID"}))
              {
                  if (projectToUpdate.ProjectFileMappings == null)
                  {
                      projectToUpdate.ProjectFileMappings = new List<ProjectFileMapping>();
                  }
                  string[] projectfiles = project.ProjectFiles.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                  for (int i = 0; i < projectFiles.Length; i++)
                  {
                      var imageMappingToEdit = projectToUpdate.ProjectFileMappings.Where(pim => pim.FileNumber == i)
                          .FirstOrDefault();
                      var file = db.ProjectFiles.Find(int.Parse(projectfiles[i]));
                      if (imageMappingToEdit == null)
                      {
                          projectToUpdate.ProjectFileMappings.Add(new ProjectFileMapping
                          {
                              FileNumber = i,
                              ProjectFile = file,
                              ProjectFileID = file.ID
                          });
                      }
                      else
                      {
                          if (imageMappingToEdit.ProjectFileID != int.Parse(projectfiles[i]))
                          {
                              imageMappingToEdit.ProjectFile = file;
                          }
                      }
                  }

                  for (int i = projectfiles.Length; i < Constants.NumberOfProjectFiles; i++)
                  {
                      var imageMappingToEdit = projectToUpdate.ProjectFileMappings
                          .Where(pim => pim.FileNumber == i).FirstOrDefault();
                      if (imageMappingToEdit != null)
                      {
                          db.ProjectFileMappins.Remove(imageMappingToEdit);
                      }
                  }
              }
              if (ModelState.IsValid)
              {
                  db.Entry(project).State = EntityState.Modified;
                  db.SaveChanges();
                  return RedirectToAction("Details", new { id = ID });
              }
              ViewBag.ProjectCategoryID = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
              return View(project);
          }


          // GET: Projects/Delete/5
          public ActionResult Delete(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Project project = db.Projects.Find(id);
              if (project == null)
              {
                  return HttpNotFound();
              }
              return View(project);
          }

          // POST: Projects/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public ActionResult DeleteConfirmed(int id)
          {
              Project project = db.Projects.Find(id);
              db.Projects.Remove(project);
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
          }*/

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
            if (img.Width < Constants.ImageMinWidth)
            {
                throw new BadImageFormatException();
            }

            if (img.Width > Constants.ImageMaxWidth)
            {
                img.Resize(Constants.ImageMaxWidth, img.Height);
            }
            img.Save(Constants.ProjectImagePath + file.FileName);

            if (img.Width > Constants.ThumbnailMaxWidth)
            {
                img.Resize(Constants.ThumbnailMaxWidth, img.Height);
            }
            img.Save(Constants.ProjectThumbnailPath + file.FileName);
        }
        #endregion
    }
}
