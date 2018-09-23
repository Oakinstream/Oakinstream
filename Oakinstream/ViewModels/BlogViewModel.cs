using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oakinstream.ViewModels
{
    public class BlogViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The name can not be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 50 characters in length")]
        [RegularExpression(@"^[a-zåäöA-ZÅÄÖ0-9]+[a-zåäöA-ZÅÄÖ0-9'-'---\s]*$", ErrorMessage = "Product name can not contain special characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public List<SelectList> ImageLists { get; set; }
        public string[] BlogImages { get; set; }
        [Required(ErrorMessage = "The description can not be blank")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The description must be between 10 and 200 characters in length")]
        [RegularExpression(@"^[a-zåäöA-ZÅÄÖ0-9]+[,.a-zåäöA-ZÅÄÖ0-9'-'---\s]*$", ErrorMessage = "Product description can not contain special characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int BlogCategoryID { get; set; }
        public SelectList BlogCategoryList { get; set; }

    }
}