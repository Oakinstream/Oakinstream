using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class ProjectModels
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ProjectCategoryID { get; set; }
        public virtual ProjectCategoryModels ProjectCategory { get; set; }
    }
}