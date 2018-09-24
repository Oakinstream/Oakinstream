using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oakinstream.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ProjectImage { get; set; }
        public int? ProjectCategoryID { get; set; }
        public virtual ProjectCategoryModels ProjectCategory { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public virtual ICollection<ProjectFileMapping> ProjectFileMappings { get; set; }


    }
}