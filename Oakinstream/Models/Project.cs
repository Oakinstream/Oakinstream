using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Oakinstream.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ProjectImageID { get; set; }
        public virtual ProjectImage ProjectImage { get; set; }
        public int? ProjectCategoryID { get; set; }
        public virtual ProjectCategory ProjectCategory { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public virtual ICollection<ProjectFileMapping> ProjectFileMappings { get; set; }
    }
}