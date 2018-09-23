using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oakinstream.Models
{
    public class BlogModels
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public virtual ICollection<BlogImageMapping> BlogImageMappings { get; set; }
        public string Description { get; set; }
        public int? BlogCategoryID { get; set; }
        public virtual BlogCategoryModels BlogCategoryModels { get; set; }

    }
}