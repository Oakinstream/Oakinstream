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
        public ActionResult Index()
        {
            return View();
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogModels blogModels = db.Blogs.Find(id);
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
            BlogModels blogPost = new BlogModels();
            blogPost.Title = viewModel.Title;
            blogPost.Description = viewModel.Description;
            blogPost.BlogCategoryID = viewModel.BlogCategoryID;
            blogPost.BlogImageMappings = new List<BlogImageMapping>();
            string[] blogImages = viewModel.BlogImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < blogImages.Length; i++)
            {
                blogPost.BlogImageMappings.Add(new BlogImageMapping()
                {
                    BlogImage = db.BlogImages.Find(int.Parse(blogImages[i])),
                    ImageNumber = i
                });
            }
            if (ModelState.IsValid)
            {
                db.Blogs.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.BlogCategoryList = new SelectList(db.BlogCategorys, "ID", "Name", blogPost.BlogCategoryID);
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
            BlogModels blogModels = db.Blogs.Find(id);
            if (blogModels == null)
            {
                return HttpNotFound();
            }
            BlogViewModel viewModel = new BlogViewModel();
            viewModel.BlogCategoryList = new SelectList(db.BlogCategorys, "ID", "Name", blogModels.BlogCategoryID);
            viewModel.ImageLists = new List<SelectList>();
            foreach (var imageMapping in blogModels.BlogImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.BlogImages, "ID", "FileName", imageMapping.BlogImageID));
            }
            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfBlogImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.BlogImages, "ID", "FileName"));
            }
            viewModel.Title = blogModels.Title;
            viewModel.Description = blogModels.Description;
            viewModel.ID = blogModels.ID;
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
                    new string[] { "Name", "Description", "BlogCategoryID" }))
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
            BlogModels blogModels = db.Blogs.Find(id);
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
            BlogModels blogModels = db.Blogs.Find(id);
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
