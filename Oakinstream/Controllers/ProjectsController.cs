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
using PagedList;

namespace Oakinstream.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [AllowAnonymous]
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            SearchIndexViewModel viewModel = new SearchIndexViewModel();
            var project = db.Projects.Include(p => p.ProjectCategory);
            if (!string.IsNullOrEmpty(search))
            {
                project = project.Where(p => p.Name.Contains(search) ||
                p.Description.Contains(search) ||
                p.ProjectCategory.Name.Contains(search));
                viewModel.Search = search;
            }
            viewModel.CategoryWithCount = from matchingProjects in project
                                          where
                                              matchingProjects.ProjectCategoryID != null
                                          group matchingProjects by
                                              matchingProjects.ProjectCategory.Name into
                                          catGroup
                                          select new CategoryWithCount()
                                          {
                                              CategoryName = catGroup.Key,
                                              Count = catGroup.Count()
                                          };
            if (!string.IsNullOrEmpty(category))
            {
                project = project.Where(p => p.ProjectCategory.Name == category);
                viewModel.Category = category;
            }
            switch (sortBy)
            {
                case "A-Ö":
                    project = project.OrderBy(p => p.Name);
                    break;
                case "Ö-A":
                    project = project.OrderByDescending(p => p.Name);
                    break;
                default:
                    project = project.OrderBy(p => p.Name);
                    break;
            }
            if (page > (project.Count() / Constants.ItemsPerPage))
            {
                page = (int)Math.Ceiling(project.Count() / (float)Constants.ItemsPerPage);
            }
            int currentPage = (page ?? 1);
            viewModel.Projects = project.ToPagedList(currentPage, Constants.ItemsPerPage);
            viewModel.SortBy = sortBy;
            viewModel.Sorts = new Dictionary<string, string>
            {
                {"A To Ö", "A-Ö" },
                {"Ö To A", "Ö-A" }
            };
            return View(viewModel);
        }

        // GET: Projects/Details/5
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ProjectViewModel viewModel = new ProjectViewModel();
            viewModel.ProjectCategoryList = new SelectList(db.ProjectCategorys, "ID", "Name");
            viewModel.ProjectImageList = new SelectList(db.ProjectImages, "ID", "FileName");
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ProjectViewModel viewModel)
        {
            Project project = new Project();
            project.Name = viewModel.Name;
            project.ProjectCategoryID = viewModel.ProjectCategoryID;
            project.ProjectImageID = viewModel.ProjectImageID;
            project.Description = viewModel.Description;
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
            project.CreatedBy = User.Identity.Name;
            project.UpdatedDate = DateTime.Now;
            project.UpdatedBy = null;

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.ProjectCategoryList = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
            viewModel.ProjectImageList = new SelectList(db.ProjectImages, "ID", "FileName", project.ProjectImageID);
            viewModel.FileList = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProjectFiles; i++)
            {
                viewModel.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName",
                viewModel.ProjectFiles[i]));
            }
            return View(viewModel);
        }


        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
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
              ProjectViewModel viewModel = new ProjectViewModel();
              viewModel.ProjectCategoryList = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
              viewModel.ProjectImageList = new SelectList(db.ProjectImages, "ID", "FileName", project.ProjectImageID);
              viewModel.FileList = new List<SelectList>();
              foreach (var imageMapping in project.ProjectFileMappings.OrderBy(pim => pim.FileNumber))
              {
                  viewModel.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName", imageMapping.ProjectFileID));
              }
              for (int i = viewModel.FileList.Count; i < Constants.NumberOfBlogImages; i++)
              {
                  viewModel.FileList.Add(new SelectList(db.ProjectFiles, "ID", "FileName"));
              }
              ViewBag.ProjectCategoryID = new SelectList(db.ProjectCategorys, "ID", "Name", project.ProjectCategoryID);
              viewModel.ID = project.ID;
              viewModel.Name = project.Name;
              viewModel.Description = project.Description;
              viewModel.CreatedDate = project.CreatedDate;
              viewModel.CreatedBy = project.CreatedBy;

            return View(viewModel);
          }

          // POST: Projects/Edit/5
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
          // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          [Authorize(Roles = "Admin")]
          public ActionResult Edit(ProjectViewModel viewModel)
          {
              var projectToUpdate = db.Projects.Include(p => p.ProjectFileMappings).
                  Where(p => p.ID == viewModel.ID).Single();
              if (TryUpdateModel(projectToUpdate, "", new string[] {"Name", "Description", "BlogCategoryID", "UpdatedBy", "UpdatedDate" }))
              {
                  if (projectToUpdate.ProjectFileMappings == null)
                  {
                      projectToUpdate.ProjectFileMappings = new List<ProjectFileMapping>();
                  }
                  string[] projectfiles = viewModel.ProjectFiles.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                  for (int i = 0; i < projectfiles.Length; i++)
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

                projectToUpdate.UpdatedBy = User.Identity.Name;
                projectToUpdate.UpdatedDate = DateTime.Now;

                db.SaveChanges();
                  return RedirectToAction("Index");
            }
              return View(viewModel);
        }


           // GET: Projects/Delete/5
          [Authorize(Roles = "Admin")]
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
          [Authorize(Roles = "Admin")]
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
          }
    }
}
