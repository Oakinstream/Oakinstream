using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class ProjectFileMapping
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int ProjectFileID { get; set; }
        public int FileNumber { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProjectFile ProjectFile { get; set; }
    }
}