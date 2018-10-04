using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class About
    {
        public int ID { get; set; }
        public int? AboutImageID { get; set; }
        public virtual AboutImage AboutImage { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public virtual ICollection<AboutFileMapping> AboutFileMappings { get; set; }
    }
}