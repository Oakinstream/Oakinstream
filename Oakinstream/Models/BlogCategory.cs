using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class BlogCategory
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public ICollection<Blog> BlogPost { get; set; }
    }
}