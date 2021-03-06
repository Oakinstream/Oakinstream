﻿using System;
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
        public string Title { get; set; }
        public List<SelectList> ImageLists { get; set; }
        public string[] BlogImages { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Link { get; set; }
        [Required(ErrorMessage = "Choose a Category")]
        public int BlogCategoryID { get; set; }
        public SelectList BlogCategoryList { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
    }
}