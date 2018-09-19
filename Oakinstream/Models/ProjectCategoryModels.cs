using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class ProjectCategoryModels
    {
        public int ID { get; set; }
        public  string Name { get; set; }
        public ICollection<ProjectModels> Project { get; set; }
    }
}