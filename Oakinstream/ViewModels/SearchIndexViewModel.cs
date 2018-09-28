using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Oakinstream.Models;
using PagedList;

namespace Oakinstream.ViewModels
{
    public class SearchIndexViewModel
    {
        public IPagedList<Project> Projects { get; set; }
        public IPagedList<Blog> Blogs { get; set; }
        public IPagedList<SearchItem> SearchResult { get; set; }
        public IEnumerable<CategoryWithCount> CategoryWithCount { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public Dictionary<string, string> Sorts { get; set; }
        public string Category { get; set; }
        public int? CategoryID { get; set; }
        public IEnumerable<SelectListItem> CategoryFilterItems
        {
            get
            {
                var allCategories = CategoryWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CategoryNameWithCount
                });
                return allCategories;
            }
        }
    }
    public class CategoryWithCount
    {
        public int Count { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameWithCount
        {
            get
            {
                return CategoryName + " (" + Count.ToString() + ")";
            }
        }
    }
}