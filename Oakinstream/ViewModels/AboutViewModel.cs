using Oakinstream.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oakinstream.ViewModels
{
    public class AboutViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The project must have a name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 100 characters in length")]
        public string Name { get; set; }
        public int? AboutImageID { get; set; }
        public About AboutImage { get; set; }
        public SelectList AboutImageList { get; set; }
        public int Age { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info1 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info2 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info3 { get; set; }
        public string[] AboutFiles { get; set; }
        public List<SelectList> FileList { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
    }
}