using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oakinstream.Models
{
    public class Blog
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public virtual ICollection<BlogImageMapping> BlogImageMappings { get; set; }
        public string Description { get; set; }
        public int? BlogCategoryID { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public virtual BlogCategory BlogCategoryModels { get; set; }
    }
}