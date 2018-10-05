using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Oakinstream.Models;

namespace Oakinstream.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The project must have a name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 100 characters in length")]
        public string Name { get; set; }
        public int? ProjectImageID { get; set; }
        public ProjectImage ProjectImage { get; set; }
        public SelectList ProjectImageList { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Choose a category")]
        public int? ProjectCategoryID { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public SelectList ProjectCategoryList { get; set; }
        public string Date { get; set; }
        public string[] ProjectFiles { get; set; }
        public List<SelectList> FileList { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
    }
}