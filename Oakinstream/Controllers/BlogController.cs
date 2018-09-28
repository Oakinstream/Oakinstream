using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Oakinstream.DAL;
using Oakinstream.Models;
using Oakinstream.ViewModels;
using PagedList;

namespace Oakinstream.Controllers
{
    public class BlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blog
        [AllowAnonymous]
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            SearchIndexViewModel viewModel = new SearchIndexViewModel();
            var blog = db.Blogs.Include(p => p.BlogCategoryModels);
            if (!string.IsNullOrEmpty(search))
            {
                blog = blog.Where(p => p.Title.Contains(search) ||
                p.Description.Contains(search) ||
                p.BlogCategoryModels.Name.Contains(search));
                viewModel.Search = search;
            }
            viewModel.CategoryWithCount = from matchingBlogs in blog
                                          where
                                              matchingBlogs.BlogCategoryID != null
                                          group matchingBlogs by
                                              matchingBlogs.BlogCategoryModels.Name into
                                          catGroup
                                          select new CategoryWithCount()
                                          {
                                              CategoryName = catGroup.Key,
                                              Count = catGroup.Count()
                                          };
            if (!string.IsNullOrEmpty(category))
            {
                blog = blog.Where(p => p.BlogCategoryModels.Name == category);
                viewModel.Category = category;
            }
            switch (sortBy)
            {
                case "New-Old":
                    blog = blog.OrderByDescending(p => p.CreatedDate);
                    break;
                case "Old-New":
                    blog = blog.OrderBy(p => p.CreatedDate);
                    break;
                default:
                    blog = blog.OrderByDescending(p => p.CreatedDate);
                    break;
            }
            if (page > (blog.Count() / Constants.ItemsPerPage))
            {
                page = (int)Math.Ceiling(blog.Count() / (float)Constants.ItemsPerPage);
            }
            int currentPage = (page ?? 1);
            viewModel.Blogs = blog.ToPagedList(currentPage, Constants.ItemsPerPage);
            viewModel.SortBy = sortBy;
            viewModel.Sorts = new Dictionary<string, string>
            {
                {"New -> Old", "New-Old" },
                {"Old -> New", "Old-New" }
            };
            return View(viewModel);
        }
        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blogModels = db.Blogs.Find(id);
            if (blogModels == null)
            {
                return HttpNotFound();
            }
            return View(blogModels);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            BlogViewModel viewModel = new BlogViewModel();
            viewModel.BlogCategoryList = new SelectList(db.BlogCategorys, "ID", "Name");
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfBlogImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BlogImages, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogViewModel viewModel)
        {
            Blog blog = new Blog();
            blog.Title = viewModel.Title;
            blog.Description = viewModel.Description;
            blog.BlogCategoryID = viewModel.BlogCategoryID;
            blog.BlogImageMappings = new List<BlogImageMapping>();
            blog.Link = viewModel.Link;
            string[] blogImages = viewModel.BlogImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < blogImages.Length; i++)
            {
                blog.BlogImageMappings.Add(new BlogImageMapping()
                {
                    BlogImage = db.BlogImages.Find(int.Parse(blogImages[i])),
                    ImageNumber = i
                });
            }
            blog.CreatedDate = DateTime.Now;
            blog.CreatedBy = User.Identity.Name;
            blog.UpdatedDate = DateTime.Now;
            blog.UpdatedBy = null;

            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.BlogCategoryList = new SelectList(db.BlogCategorys, "ID", "Name", blog.BlogCategoryID);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfBlogImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BlogImages, "ID", "FileName",
                    viewModel.BlogImages[i]));
            }
            return View(viewModel);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            BlogViewModel viewModel = new BlogViewModel();
            viewModel.BlogCategoryList = new SelectList(db.BlogCategorys, "ID", "Name", blog.BlogCategoryID);
            viewModel.ImageLists = new List<SelectList>();
            foreach (var imageMapping in blog.BlogImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.BlogImages, "ID", "FileName", imageMapping.BlogImageID));
            }
            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfBlogImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BlogImages, "ID", "FileName"));
            }
            viewModel.Title = blog.Title;
            viewModel.Description = blog.Description;
            viewModel.ID = blog.ID;
            viewModel.Link = blog.Link;
            viewModel.CreatedDate = blog.CreatedDate;
            viewModel.CreatedBy = blog.CreatedBy;

            return View(viewModel);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogViewModel viewModel)
        {
            var blogToUpdate = db.Blogs.Include(p => p.BlogImageMappings).
                    Where(p => p.ID == viewModel.ID).Single();
            if (TryUpdateModel(blogToUpdate, "",
                    new string[] { "Name", "Description", "BlogCategoryID", "UpdatedBy", "UpdatedDate" }))
            {
                if (blogToUpdate.BlogImageMappings == null)
                {
                    blogToUpdate.BlogImageMappings = new List<BlogImageMapping>();
                }
                string[] blogImages = viewModel.BlogImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for (int i = 0; i < blogImages.Length; i++)
                {
                    var imageMappingToEdit = blogToUpdate.BlogImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    var image = db.BlogImages.Find(int.Parse(blogImages[i]));
                    if (imageMappingToEdit == null)
                    {
                        blogToUpdate.BlogImageMappings.Add(new BlogImageMapping
                        {
                            ImageNumber = i,
                            BlogImage = image,
                            BlogImageID = image.ID
                        });
                    }
                    else
                    {
                        if (imageMappingToEdit.BlogImageID != int.Parse(blogImages[i]))
                        {
                            imageMappingToEdit.BlogImage = image;
                        }
                    }
                }
                for (int i = blogImages.Length; i < Constants.NumberOfBlogImages; i++)
                {
                    var imageMappingToEdit = blogToUpdate.BlogImageMappings
                        .Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    if (imageMappingToEdit != null)
                    {
                        db.BlogImageMappings.Remove(imageMappingToEdit);
                    }
                }

                blogToUpdate.UpdatedBy = User.Identity.Name;
                blogToUpdate.UpdatedDate = DateTime.Now;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blogModels = db.Blogs.Find(id);
            if (blogModels == null)
            {
                return HttpNotFound();
            }
            return View(blogModels);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blogModels = db.Blogs.Find(id);
            db.Blogs.Remove(blogModels);
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
