﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Oakinstream.Models;

namespace Oakinstream.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ProjectImage { get; set; }
        public string Description { get; set; }
        public int? ProjectCategoryID { get; set; }
        public ProjectCategoryModels ProjectCategory { get; set; }
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