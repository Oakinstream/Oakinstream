using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class ProjectFile
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public virtual ICollection<ProjectFileMapping> ProjectFileMappings { get; set; }
    }
}