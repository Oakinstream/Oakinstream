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
        [Required(ErrorMessage = "The name can not be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 50 characters in length")]
        [Display(Name = "Category")]
        public string Name { get; set; }
        public ICollection<Blog> BlogPost { get; set; }
    }
}