using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class ProjectModels
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-ZÅÄÖ]+[a-zåäöA-ZÅÄÖ'-'---\s]*$", ErrorMessage = "Name must begin with a capital letter and can not contain special character or numbers")]
        [Display(Name = "Project Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The description can not be blank")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The description must be between 10 and 200 characters in length")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int? ProjectCategoryID { get; set; }
        public virtual ProjectCategoryModels ProjectCategory { get; set; }
    }
}